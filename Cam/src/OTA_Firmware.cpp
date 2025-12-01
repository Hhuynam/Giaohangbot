#include <ArduinoOTA.h>
#include "OTA_Firmware.h"

void OTA_Setup(const char* ssid, const char* password, const char* hostname) {
    
    ArduinoOTA.setHostname(hostname);

    ArduinoOTA.onStart([]() {
        Serial.println("Start updating firmware...");
    });
    ArduinoOTA.onEnd([]() {
        Serial.println("\nUpdate finished!");
    });
    ArduinoOTA.onProgress([](unsigned int progress, unsigned int total) {
        Serial.printf("Progress: %u%%\r", (progress / (total / 100)));
    });
    ArduinoOTA.onError([](ota_error_t error) {
        Serial.printf("Error[%u]\n", error);
    });

    ArduinoOTA.begin();
    Serial.println("OTA ready. You can upload new firmware via WiFi.");
}

void OTA_Handle() {
    ArduinoOTA.handle();
}
