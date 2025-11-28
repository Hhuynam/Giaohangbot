#include "esp_http_server.h"
#include "esp_camera.h"

httpd_handle_t stream_httpd = NULL;

static esp_err_t stream_handler(httpd_req_t *req) {
  camera_fb_t *fb;
  esp_err_t res = ESP_OK;
  char *part_buf[64];

  httpd_resp_set_type(req, "multipart/x-mixed-replace;boundary=frame");

  while (true) {
    fb = esp_camera_fb_get();
    if (!fb) {
      res = ESP_FAIL;
    } else {
      size_t hlen = snprintf((char *)part_buf, 64,
        "--frame\r\nContent-Type: image/jpeg\r\nContent-Length: %u\r\n\r\n",
        fb->len);
      res = httpd_resp_send_chunk(req, (const char *)part_buf, hlen);
      res = httpd_resp_send_chunk(req, (const char *)fb->buf, fb->len);
      res = httpd_resp_send_chunk(req, "\r\n", 2);
      esp_camera_fb_return(fb);
    }
    if (res != ESP_OK) break;
  }
  return res;
}

void startCameraServer() {
  httpd_config_t config = HTTPD_DEFAULT_CONFIG();
  httpd_uri_t stream_uri = { .uri="/stream", .method=HTTP_GET, .handler=stream_handler };
  if (httpd_start(&stream_httpd, &config) == ESP_OK) {
    httpd_register_uri_handler(stream_httpd, &stream_uri);
  }
}