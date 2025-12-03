#include "esp_http_server.h"
#include "esp_camera.h"
#include <Arduino.h>





extern String logBuffer;




httpd_handle_t stream_httpd = NULL;




static esp_err_t stream_handler(httpd_req_t *req) {
  camera_fb_t *fb;
  esp_err_t res = ESP_OK;
  char part_buf[64];

  httpd_resp_set_type(req, "multipart/x-mixed-replace;boundary=frame");

  while (true) {
    fb = esp_camera_fb_get();
    if (!fb) {
      res = ESP_FAIL;
    } else {
      size_t hlen = snprintf(part_buf, 64,
        "--frame\r\nContent-Type: image/jpeg\r\nContent-Length: %u\r\n\r\n",
        fb->len);
      res = httpd_resp_send_chunk(req, part_buf, hlen);
      res = httpd_resp_send_chunk(req, (const char *)fb->buf, fb->len);
      res = httpd_resp_send_chunk(req, "\r\n", 2);
      esp_camera_fb_return(fb);
    }
    if (res != ESP_OK) break;
  }
  return res;
}





static esp_err_t logs_handler(httpd_req_t *req) {
  httpd_resp_set_type(req, "text/plain");
  httpd_resp_sendstr(req, logBuffer.c_str());
  return ESP_OK;
}






void startCameraServer() {
  httpd_config_t config = HTTPD_DEFAULT_CONFIG();
  if (httpd_start(&stream_httpd, &config) == ESP_OK) {
    httpd_uri_t stream_uri = {
      .uri       = "/stream",
      .method    = HTTP_GET,
      .handler   = stream_handler,
      .user_ctx  = NULL
    };
    httpd_register_uri_handler(stream_httpd, &stream_uri);

    httpd_uri_t logs_uri = {
      .uri       = "/logs",
      .method    = HTTP_GET,
      .handler   = logs_handler,
      .user_ctx  = NULL
    };
    httpd_register_uri_handler(stream_httpd, &logs_uri);
  }
}
