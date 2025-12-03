#include "esp_camera.h"
#include <WiFi.h>
#include <esp_now.h>
#include <esp_wifi.h>
#include <PubSubClient.h>
#include "camera_pins.h"
#include "esp_http_server.h"
#include <ArduinoOTA.h>
#include "esp_ota_ops.h"



const char* ssid     = "TP-LINK_0F54";
const char* password = "68377038";
const char* mqtt_server = "broker.hivemq.com";
const int   mqtt_port   = 1883;
const char* mqtt_topic  = "namha/iot";

uint8_t peerMacS3[] = {0xB8, 0xF8, 0x62, 0xE2, 0x81, 0xDC};

extern httpd_handle_t stream_httpd;


IPAddress static_ip(192, 168, 0, 220);
IPAddress gateway(192, 168, 0, 1);
IPAddress subnet(255, 255, 255, 0);
IPAddress primaryDNS(8, 8, 8, 8);      
IPAddress secondaryDNS(8, 8, 4, 4);    


class WifiConnection {
public:
  void connect() {


    if (!WiFi.config(static_ip, gateway, subnet, primaryDNS, secondaryDNS)) {
      Serial.println("Static IP config failed");
    }

    WiFi.begin(ssid, password);
    WiFi.setSleep(false);
    Serial.print("WiFi connecting");
    while (WiFi.status() != WL_CONNECTED) {
      delay(500);
      Serial.print(".");
    }
    Serial.println("WiFi connected");
    printInfo();
  }

  void printInfo() {
    Serial.print("IP: "); Serial.println(WiFi.localIP());
    Serial.print("MAC: "); Serial.println(WiFi.macAddress());
    Serial.print("Channel: "); Serial.println(WiFi.channel());
  }
};







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
    Serial.println("Camera initialized");
  }

  void startServer() {
    extern void startCameraServer();
    startCameraServer();
    Serial.println("Camera server started");
  }
};







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
    for (int i = 0; i < len; i++) msg += (char)data[i];
    msg.trim();
    Serial.print("ESP-NOW received: "); Serial.println(msg);
  }

private:
  static EspNowConnection* instance;
};
EspNowConnection* EspNowConnection::instance = nullptr;








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






class OtaModule {
public:
  void init() {
    printPartitionInfo();

    ArduinoOTA.setHostname("ESP32-CAM");
    ArduinoOTA.setPassword("1234");

    ArduinoOTA
      .onStart([]() { Serial.println("Start OTA update"); })
      .onEnd([]() { Serial.println("OTA update complete"); })
      .onProgress([](unsigned int progress, unsigned int total) {
        Serial.printf("Progress: %u%%\n", (progress * 100) / total);
      })
      .onError([](ota_error_t error) {
        Serial.printf("OTA Error[%u]\n", error);
      });

    ArduinoOTA.begin();
    Serial.println("OTA ready");
  }

  void loop() {
    ArduinoOTA.handle();
  }

private:
  void printPartitionInfo() {
    const esp_partition_t* running = esp_ota_get_running_partition();
    const esp_partition_t* next = esp_ota_get_next_update_partition(nullptr);
    if (running) {
      Serial.printf("[Partition] Running: %s at 0x%X, size=0x%X\n",
                    running->label, running->address, running->size);
    }
    if (next) {
      Serial.printf("[Partition] Next OTA target: %s at 0x%X, size=0x%X\n",
                    next->label, next->address, next->size);
    }
  }
};









WifiConnection wifi;
WebCamServer cam;
EspNowConnection espNow;
MqttConnection mqtt(&espNow);
OtaModule ota;

String logBuffer = "";


void addLog(const String& msg) {
  Serial.println(msg);
  logBuffer += msg + "\n";
  if (logBuffer.length() > 4000) {
    logBuffer.remove(0, logBuffer.length() - 4000);
  }
}



extern "C" int log_vprintf(const char* fmt, va_list args) {
  char buf[256];
  vsnprintf(buf, sizeof(buf), fmt, args);
  logBuffer += String(buf);
  if (logBuffer.length() > 4000) {
    logBuffer.remove(0, logBuffer.length() - 4000); 
  }
  return vprintf(fmt, args); 
}





void setup() {
  
Serial.begin(115200);


esp_log_set_vprintf(log_vprintf);



wifi.connect();
cam.initCamera();
cam.startServer();
espNow.init();
mqtt.init();
ota.init();

addLog("Test log to buffer");




}




void loop() {
  mqtt.loop();
  ota.loop();
  static unsigned long last = 0;
  if (millis() - last > 5000) {
    addLog("Hahuynam vip");
    last = millis();
  }
}
