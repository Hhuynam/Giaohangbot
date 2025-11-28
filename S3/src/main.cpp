#include <Arduino.h>
#include <WiFi.h>
#include <esp_wifi.h>
#include <esp_now.h>
#include <Adafruit_GFX.h>
#include <Adafruit_ST7789.h>
#include <SPI.h>
#include <Fonts/FreeSans12pt7b.h>
#include <Fonts/FreeSans9pt7b.h>
#include <qrcode.h>
#include <WebServer.h>


//  DISPLAY 
class Module_Display {
public:
  const int TFT_CS  = 38;
  const int TFT_DC  = 39;
  const int TFT_RST = 40;
  Adafruit_ST7789 tft = Adafruit_ST7789(&SPI, TFT_CS, TFT_DC, TFT_RST);

  void init() {
    SPI.begin(42, -1, 41);
    SPI.setFrequency(20000000);
    tft.init(240, 240);
    tft.setRotation(2);
    showSplash();
    Serial.println("[Display] init OK");
  }

  void showSplash() {
    tft.fillScreen(ST77XX_BLUE);
    tft.setTextColor(ST77XX_WHITE);
    tft.setFont(&FreeSans12pt7b);
    tft.setCursor(30, 100);
    tft.print("GIAOHANGBOT");
    uint16_t gray = tft.color565(128, 128, 128);
    tft.setTextColor(gray);
    tft.setFont(&FreeSans9pt7b);
    tft.setCursor(40, 130);
    tft.print("Powered by Namha");
  }

  void showQr(const String& raw) {
  // 1. Bỏ prefix "emvco=" nếu có
  String emvco = raw;
  if (emvco.startsWith("emvco=")) {
    emvco = emvco.substring(7);
  }
  emvco.trim();

  // 2. Cố định version 7 (dư sức chứa cho chuỗi ~106 ký tự)
  QRCode qrcode;
  int version = 7;
  int bufSize = qrcode_getBufferSize(version);
  uint8_t* qrcodeData = (uint8_t*)malloc(bufSize);
  if (!qrcodeData) {
    Serial.println("[Display] malloc failed");
    return;
  }

  if (qrcode_initText(&qrcode, qrcodeData, version, ECC_LOW, emvco.c_str()) != 0) {
    Serial.printf("[Display] QR init failed (version=%d)\n", version);
    free(qrcodeData);
    return;
  }

  // 3. Tính scale và offset để QR vừa màn hình, có quiet zone ≥4 modules
  const int minQuietModules = 4;
  int modules = qrcode.size;
  int maxScaleW = tft.width()  / (modules + 2 * minQuietModules);
  int maxScaleH = tft.height() / (modules + 2 * minQuietModules);
  int scale = min(maxScaleW, maxScaleH);
  if (scale < 1) scale = 1;

  int offsetX = (tft.width()  - modules * scale) / 2;
  int offsetY = (tft.height() - modules * scale) / 2;

  // 4. Vẽ QR
  tft.fillScreen(ST77XX_WHITE);
  for (int y = 0; y < modules; y++) {
    for (int x = 0; x < modules; x++) {
      if (qrcode_getModule(&qrcode, x, y)) {
        tft.fillRect(offsetX + x * scale, offsetY + y * scale,
                     scale, scale, ST77XX_BLACK);
      }
    }
  }

  free(qrcodeData);
  Serial.printf("[Display] QR rendered (version=%d, scale=%d, len=%d)\n",
                version, scale, emvco.length());
}


};

//  MOTOR DRIVER 
class Module_MotorDriver {
public:
  const int ENA = 16, IN1 = 15, IN2 = 7, IN3 = 6, IN4 = 5, ENB = 4;
  const int channelA = 0, channelB = 1, freq = 1000, resolution = 8;
  int sustainTarget = 50, boostTarget = 90, currentDuty = 0;

  void init() {
    pinMode(IN1, OUTPUT); pinMode(IN2, OUTPUT);
    pinMode(IN3, OUTPUT); pinMode(IN4, OUTPUT);
    pinMode(ENA, OUTPUT); pinMode(ENB, OUTPUT);
    ledcSetup(channelA, freq, resolution);
    ledcAttachPin(ENA, channelA);
    ledcSetup(channelB, freq, resolution);
    ledcAttachPin(ENB, channelB);
    stop();
    Serial.println("[Driver] init OK");
  }
  void setSustain(int duty) { duty = constrain(duty, 0, 255); sustainTarget = duty; Serial.printf("[Driver] sustain=%d\n", sustainTarget); }
  void setBoost(int duty) { duty = constrain(duty, 0, 255); boostTarget = duty; Serial.printf("[Driver] boost=%d\n", boostTarget); }
  void rampToSpeed(int target) {
    target = constrain(target, 0, 255);
    ledcWrite(channelA, target);
    ledcWrite(channelB, target);
    currentDuty = target;
  }
  void drive(int in1, int in2, int in3, int in4) {
    digitalWrite(IN1, in1); digitalWrite(IN2, in2);
    digitalWrite(IN3, in3); digitalWrite(IN4, in4);
    rampToSpeed(boostTarget);
    delay(200);
    rampToSpeed(sustainTarget);
  }
  void forward() { drive(HIGH, LOW, HIGH, LOW); Serial.println("[Driver] FORWARD"); }
  void backward(){ drive(LOW, HIGH, LOW, HIGH); Serial.println("[Driver] BACKWARD"); }
  void left()    { drive(LOW, HIGH, HIGH, LOW); Serial.println("[Driver] LEFT"); }
  void right()   { drive(HIGH, LOW, LOW, HIGH); Serial.println("[Driver] RIGHT"); }
  void stop() {
    rampToSpeed(0);
    digitalWrite(IN1, LOW); digitalWrite(IN2, LOW);
    digitalWrite(IN3, LOW); digitalWrite(IN4, LOW);
    Serial.println("[Driver] STOP");
  }
};

//  BUZZER 
class Module_Buzzer {
public:
  const int buzzerPin = 17;
  const int channel   = 2;
  const int freq      = 1000;
  const int resolution = 8;

  void init() {
    ledcSetup(channel, freq, resolution);
    ledcAttachPin(buzzerPin, channel);
    off();
  }
  void on() { ledcWrite(channel, 255); }
  void off(){ ledcWrite(channel, 0); }
  void beep(unsigned long ms = 200) {
    on();
    delay(ms);
    off();
  }
};

//  RELAY 
class Module_Relay {
public:
  const int relayPin = 18;
  void init() { pinMode(relayPin, OUTPUT); off(); Serial.println("[Relay] init OK"); }
  void on() { digitalWrite(relayPin, HIGH); Serial.println("[Relay] ON"); }
  void off(){ digitalWrite(relayPin, LOW);  Serial.println("[Relay] OFF"); }
};

//  SERVO 
class Module_Servo {
public:
  const int servoPin = 14, channel = 3, freq = 50, resolution = 14;
  void init() { ledcSetup(channel, freq, resolution); ledcAttachPin(servoPin, channel); off(); Serial.println("[Servo] init OK"); }
  void writeAngle(int angle) {
    angle = constrain(angle, 0, 180);
    int us = map(angle, 0, 180, 500, 2500);
    int duty = (int)((us * (1 << resolution) * freq) / 1000000);
    ledcWrite(channel, duty);
    Serial.printf("[Servo] angle=%d (duty=%d)\n", angle, duty);
  }
  void on() { writeAngle(90); }
  void off(){ writeAngle(0); }
};

//  ESP-NOW 
class Module_EspNow {
public:
  typedef void (*CmdHandler)(const String&);
  CmdHandler handler = nullptr;
  static Module_EspNow* instance;

  void init(CmdHandler cb) {
    handler = cb;
    WiFi.mode(WIFI_STA);
    WiFi.disconnect(); // không join AP, chỉ ESP-NOW
    Serial.println("[WiFi] Station mode set");

    uint8_t channel = 2; // ép cứng channel 2
    esp_wifi_set_channel(channel, WIFI_SECOND_CHAN_NONE);

    if (esp_now_init() != ESP_OK) {
      Serial.println("[ESP-NOW] init failed");
      return;
    }
    esp_now_peer_info_t peerInfo = {};
    uint8_t peerMac[6] = {0xF8, 0xB3, 0xB7, 0x7A, 0xC2, 0xF4}; // MAC của ESP32-CAM
    memcpy(peerInfo.peer_addr, peerMac, 6);
    peerInfo.channel = 2;
    peerInfo.encrypt = false;
    if (esp_now_add_peer(&peerInfo) == ESP_OK)
      Serial.println("[ESP-NOW] Peer ESP32-CAM added");
    else
      Serial.println("[ESP-NOW] Failed to add peer");

    instance = this;
    esp_now_register_recv_cb(_onRecvStatic);
    Serial.println("[ESP-NOW] Receiver ready on channel 2");
  }

  static void _onRecvStatic(const uint8_t* mac, const uint8_t* data, int len) {
    if (!instance || !instance->handler) return;
    String msg;
    for (int i = 0; i < len; i++) msg += (char)data[i];
    msg.trim();
        Serial.printf("[ESP-NOW] received: '%s'\n", msg.c_str());
    instance->handler(msg);
  }
};

//  STATIC INSTANCE 
Module_EspNow* Module_EspNow::instance = nullptr;

//  GLOBAL MODULES 
Module_Display display;
Module_MotorDriver driver;
Module_Buzzer buzzer;
Module_Relay relay;
Module_Servo servo;
Module_EspNow espnow;

//  COMMAND HANDLER 
void onCommand(const String& cmdRaw) {
  String cmd = cmdRaw;
  cmd.trim();

  if (cmd.startsWith("sustain=")) {
    driver.setSustain(cmd.substring(8).toInt());
  } else if (cmd.startsWith("boost=")) {
    driver.setBoost(cmd.substring(6).toInt());
  } else if (cmd.equals("forward")) {
    driver.forward();
  } else if (cmd.equals("backward")) {
    driver.backward();
  } else if (cmd.equals("left")) {
    driver.left();
  } else if (cmd.equals("right")) {
    driver.right();
  } else if (cmd.equals("stop")) {
    driver.stop();
  } else if (cmd.equals("buzzer_on")) {
    buzzer.on();
  } else if (cmd.equals("buzzer_off")) {
    buzzer.off();
  } else if (cmd.equals("buzzer_beep")) {
    buzzer.beep();
  } else if (cmd.equals("relay_on")) {
    relay.on();
  } else if (cmd.equals("relay_off")) {
    relay.off();
  } else if (cmd.startsWith("emvco=")) {
    String emvco = cmd.substring(cmd.indexOf('=')+1);
    emvco.trim();
    display.showQr(emvco);
  } else if (cmd.equals("servo_on")) {
    servo.on();
  } else if (cmd.equals("servo_off")) {
    servo.off();
  } else {
    Serial.printf("[CMD] Unknown: '%s'\n", cmd.c_str());
  }
}


//  SETUP & LOOP 
void setup() {
  Serial.begin(115200);
  display.init();
  driver.init();
  buzzer.init();
  relay.init();
  servo.init();

  driver.stop();
  buzzer.off();
  relay.off();
  servo.off();

  espnow.init(onCommand);
}

void loop() {
  // loop trống, ESP-NOW callback sẽ xử lý
}
