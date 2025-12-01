#include "esp_camera.h"
#include <WiFi.h>
#include <esp_now.h>
#include <esp_wifi.h>
#include <PubSubClient.h>
#include "camera_pins.h"
#include "esp_http_server.h"
#include "OTA_Firmware.h"

// ===== Config =====
const char* ssid     = "TP-LINK_0F54";
const char* password = "68377038";
const char* mqtt_server = "broker.hivemq.com";
const int   mqtt_port   = 1883;
const char* mqtt_topic  = "namha/iot";

// MAC ESP32-S3 (peer)
uint8_t peerMacS3[] = {0xB8, 0xF8, 0x62, 0xE2, 0x81, 0xDC};

// handle httpd từ camera server
extern httpd_handle_t stream_httpd;

// ===== Class WifiConnection =====
class WifiConnection {
public:
  void connect() {
    WiFi.begin(ssid, password);
    WiFi.setSleep(false);
    Serial.print("WiFi connecting");
    while (WiFi.status() != WL_CONNECTED) {
      delay(500);
      Serial.print(".");
    }
    Serial.println("\nWiFi connected");
    printInfo();
  }

  void printInfo() {
    Serial.print("IP: "); Serial.println(WiFi.localIP());
    Serial.print("MAC: "); Serial.println(WiFi.macAddress());
    Serial.print("Channel: "); Serial.println(WiFi.channel());
  }
};

// ===== Class WebCamServer =====
class WebCamServer {
public:
  void initCamera() {
    camera_config_t config;
    config.ledc_channel = LEDC_CHANNEL_0;
    config.ledc_timer   = LEDC_TIMER_0;
    config.pin_d0       = Y2_GPIO_NUM;
    config.pin_d1       = Y3_GPIO_NUM;
    config.pin_d2       = Y4_GPIO_NUM;
    config.pin_d3       = Y5_GPIO_NUM;
    config.pin_d4       = Y6_GPIO_NUM;
    config.pin_d5       = Y7_GPIO_NUM;
    config.pin_d6       = Y8_GPIO_NUM;
    config.pin_d7       = Y9_GPIO_NUM;
    config.pin_xclk     = XCLK_GPIO_NUM;
    config.pin_pclk     = PCLK_GPIO_NUM;
    config.pin_vsync    = VSYNC_GPIO_NUM;
    config.pin_href     = HREF_GPIO_NUM;
    config.pin_sccb_sda = SIOD_GPIO_NUM;
    config.pin_sccb_scl = SIOC_GPIO_NUM;
    config.pin_pwdn     = PWDN_GPIO_NUM;
    config.pin_reset    = RESET_GPIO_NUM;
    config.xclk_freq_hz = 10000000;
    config.pixel_format = PIXFORMAT_JPEG;

    if (psramFound()) {
      config.frame_size   = FRAMESIZE_VGA;
      config.jpeg_quality = 12;
      config.fb_count     = 2;
      config.grab_mode    = CAMERA_GRAB_LATEST;
      config.fb_location  = CAMERA_FB_IN_PSRAM;
    } else {
      config.frame_size   = FRAMESIZE_QVGA;
      config.jpeg_quality = 15;
      config.fb_count     = 1;
      config.fb_location  = CAMERA_FB_IN_DRAM;
    }

    if (esp_camera_init(&config) != ESP_OK) {
      Serial.println("Camera init failed");
      return;
    }
    sensor_t *s = esp_camera_sensor_get();
    s->set_framesize(s, FRAMESIZE_QVGA);
  }

  void startServer() {
    extern void startCameraServer();
    startCameraServer();
    Serial.println("Camera server started");
  }
};

// ===== Class EspNowConnection =====
class EspNowConnection {
public:
  void init() {
    WiFi.mode(WIFI_STA);
    if (esp_now_init() != ESP_OK) {
      Serial.println("ESP-NOW init failed");
      return;
    }
    esp_now_peer_info_t peerInfo = {};
    memcpy(peerInfo.peer_addr, peerMacS3, 6);
    peerInfo.channel = WiFi.channel();
    peerInfo.encrypt = false;
    if (esp_now_add_peer(&peerInfo) == ESP_OK) {
      Serial.println("ESP-NOW peer added");
    }
    esp_now_register_recv_cb(onReceiveStatic);
    instance = this;
  }

  void send(const String& msg) {
    esp_now_send(peerMacS3, (const uint8_t*)msg.c_str(), msg.length());
    Serial.print("ESP-NOW sent: "); Serial.println(msg);
  }

  static void onReceiveStatic(const uint8_t* mac, const uint8_t* data, int len) {
    if (!instance) return;
    String msg;
    for (int i=0;i<len;i++) msg += (char)data[i];
    msg.trim();
    Serial.print("ESP-NOW received: "); Serial.println(msg);
  }

private:
  static EspNowConnection* instance;
};
EspNowConnection* EspNowConnection::instance = nullptr;

// ===== Class MqttConnection =====
class MqttConnection {
public:
  MqttConnection(EspNowConnection* espNow) : espNow(espNow), client(espClient) {}

  void init() {
    client.setServer(mqtt_server, mqtt_port);
    client.setCallback(callbackStatic);
    instance = this;
    connect();
  }

  void loop() {
    if (!client.connected()) connect();
    client.loop();
  }

private:
  WiFiClient espClient;
  PubSubClient client;
  EspNowConnection* espNow;
  static MqttConnection* instance;

  void connect() {
    while (!client.connected()) {
      String cid = "ESP32CAM-" + WiFi.macAddress();
      if (client.connect(cid.c_str())) {
        client.subscribe(mqtt_topic);
        Serial.println("MQTT connected & subscribed");
      } else {
        delay(2000);
      }
    }
  }

  static void callbackStatic(char* topic, byte* payload, unsigned int length) {
    if (!instance) return;
    String msg;
    for (unsigned int i=0;i<length;i++) msg += (char)payload[i];
    msg.trim();
    Serial.print("MQTT received: "); Serial.println(msg);
    instance->espNow->send(msg);
  }
};
MqttConnection* MqttConnection::instance = nullptr;

// ===== Class HttpConnection =====
class HttpConnection {
public:
  void init(EspNowConnection* espNow) {
    this->espNow = espNow;
    if (stream_httpd) {
      httpd_uri_t cmd_uri = {
        .uri       = "/cmd",
        .method    = HTTP_POST,
        .handler   = cmd_handler,
        .user_ctx  = this
      };
      httpd_register_uri_handler(stream_httpd, &cmd_uri);
      Serial.println("[HTTP] route /cmd registered on camera server");
    }
  }

private:
  EspNowConnection* espNow;

  static esp_err_t cmd_handler(httpd_req_t *req) {
    HttpConnection* self = (HttpConnection*)req->user_ctx;
    char buf[1024];
    int len = httpd_req_recv(req, buf, sizeof(buf)-1);
    if (len <= 0) {
        httpd_resp_send_err(req, HTTPD_400_BAD_REQUEST, "No data received");
        return ESP_FAIL;
    }
    buf[len] = 0;
    String msg = String(buf);
    msg.trim();

    Serial.printf("[HTTP] received: %s\n", msg.c_str());
    if (self && self->espNow) {
        self->espNow->send(msg);
    }

    httpd_resp_set_type(req, "application/json");
    httpd_resp_sendstr(req, "{\"status\":\"ok\"}");
    return ESP_OK;
  }
};

// ===== Global objects =====
WifiConnection wifi;
WebCamServer cam;
EspNowConnection espNow;
MqttConnection mqtt(&espNow);
HttpConnection http;

// ===== Setup & Loop =====
void setup() {
  Serial.begin(115200);
  wifi.connect();
  cam.initCamera();
  cam.startServer();   
  espNow.init();
  http.init(&espNow);  
  mqtt.init();

  OTA_Setup(ssid, password, "esp32cam");
}

void loop() {
  mqtt.loop();
  OTA_Handle();
}