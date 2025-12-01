---
marp: true
theme: default
style: |
  section {
    background-color: #ebdd8dff; 
  }

  .Header {
    position: absolute;
    top: 20px;           
    left: 50px;          
    font-size: 28px;     
    font-weight: bold;   
    color: #170263ff;     
    text-transform: uppercase;
    letter-spacing: 2px; 
  }

  .Grid {
    display: grid;
    grid-template-columns: 1fr 1fr; 
    gap: 20px;
    align-items: start;
  }

  .Left {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: flex-start;
  }

  .Text1 {
    font-size: 20px;
    font-weight: bold;
    color: #333;
    margin-bottom: 10px;
    font-family: "Arial";
  }

  .Text2 {
    font-size: 15px;
    color: #555;
    line-height: 1.5;
    font-family: "Arial";
  }

  .Right {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: flex-start;
  }


  .Image {
    max-width: 500px;
    height: auto;
    border: 3px solid #0056b3;
    border-radius: 8px;
  }

  .Page_Number {
    position: absolute;
    right: 30px;
    bottom: 20px;
    font-size: 14px;
    font-weight: bold;
    color: #0056b3;
  }

  .TextBlock {
  background-color: #ffffff;
  border-radius: 12px;
  padding: 10px 20px;
  margin: 0 0 15px 0;
  box-shadow: 0 4px 8px rgba(0,0,0,0.1);
  font-size: 16px;
  color: #333;
  line-height: 1.6;
  width: 100%;
  box-sizing: border-box;
  }


  .Caption {
  font-size: 13px;
  color: #444;
  margin-top: 6px;
  font-style: italic;
  text-align: center;
  }

---
<div class="Header">Nội dung trình bày</div>

<div class="Grid">


  <div class="Left">
    <div class="TextBlock">
      <div class="Text2">
        1. Giới thiệu đề tài<br>
        1.1 Thực tế giao hàng hiện nay<br>
        1.2 Giới thiệu về robot giao hàng<br>
        1.3 Thực tế triển khai robot giao hàng
      </div>
    </div>

  <div class="TextBlock">
    <div class="Text2">
      2. Định hướng đề tài<br>
      2.1 Mục tiêu thử nghiệm<br>
      2.2 Giới hạn phần cứng<br>
      2.3 Giới hạn phần mềm
    </div>
  </div>
  </div> 


  <div class="Right" style="display:block; padding:30px 10px;">
    <div class="TextBlock">
      <div class="Text2">
        3. Phân tích đề tài<br>
        3.1 Phần cứng<br>
        3.2 Phần mềm<br>
        3.3. Phân tích hệ thống<br>
      </div>
    </div>

  <div class="TextBlock">
    <div class="Text2">
      4. Quá trình thực nghiệm<br>
      4.1 Thực nghiệm phần cứng<br>
      4.2 Thực nghiệm phần mềm
    </div>
  </div>
  </div> 

</div> 

<div class="Page_Number">1</div>

---
<div class="Header">1. Giới thiệu đề tài</div>
<div class="Text1">1.1. Thực tế giao hàng hiện nay</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">Nhân viên giao hàng</div>
    <div class="TextBlock"><div class="Text2">Hiện nay, phần lớn đơn hàng thương mại điện tử được giao bởi nhân viên giao hàng truyền thống</div></div>

  <div class="Text1">Ví dụ Shopee Express (SPX)</div>
  <div class="TextBlock"><div class="Text2">Nhân viên SPX đảm nhận việc lấy hàng từ kho, di chuyển bằng xe máy và giao trực tiếp đến khách hàng</div></div>

<div class="Text1">Đặc điểm</div>
<div class="TextBlock"><div class="Text2">Linh hoạt, xử lý nhiều tình huống thực tế</div></div>
<div class="TextBlock"><div class="Text2">Tốn nhân lực, chi phí vận hành cao</div></div>
<div class="TextBlock"><div class="Text2">Phụ thuộc vào giao thông và thời tiết</div></div>

  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image\NhanVien_Spx.jpg" class="Image" />
      <div class="Caption">Nhân viên Shopee Express (SPX) đang giao hàng</div>
    </div>
  </div>
</div>

<div class="Page_Number">2</div>


---
<div class="Header">1. Giới thiệu đề tài</div>
<div class=Text1>1.2. Giới thiệu về robot giao hàng</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">Robot giao hàng</div>
    <div class="TextBlock"><div class="Text2">Là phương tiện di chuyển tự động dùng công nghệ định vị, cảm biến và trí tuệ nhân tạo để vận chuyển hàng hóa</div></div>

  <div class="Text1">Cách robot giao hàng hoạt động</div>
  <div class="TextBlock"><div class="Text2">Robot di chuyển bằng GPS, camera và cảm biến để xác định lộ trình, tránh chướng ngại vật và giao hàng đến đúng địa điểm</div></div>

  <div class="Text1">Lợi ích của robot giao hàng</div>
  <div class="TextBlock"><div class="Text2">Robot giúp giảm chi phí nhân công, tăng hiệu quả giao nhận, hạn chế tiếp xúc trực tiếp và thân thiện môi trường</div></div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Jd_Robot.jpg" class="Image" />
      <div class="Caption">Robot giao hàng thử nghiệm của JD.com tại Trung Quốc</div>
    </div>
  </div>
</div>

<div class="Page_Number">3</div>

---
<div class="Header">1. Giới thiệu đề tài</div>
<div class="Text1">1.3. Thực tế triển khai robot giao hàng</div>
<div class="Grid">
  <div class="Left">
    <div class="Text1">Trên thế giới</div>
    <div class="TextBlock"><div class="Text2">Amazon, JD.com, Starship đã triển khai robot giao hàng tại Mỹ, Trung Quốc và châu Âu</div></div>
    <div class="Text1">Tại Việt Nam</div>
    <div class="TextBlock"><div class="Text2">Một số doanh nghiệp như Viettel, VNPost đã thử nghiệm robot giao hàng trong khu đô thị và khu công nghệ cao</div></div>
    <div class="Text1">Thách thức</div>
    <div class="TextBlock"><div class="Text2">Hạ tầng đường phố chưa đồng bộ, chi phí đầu tư cao và cần khung pháp lý phù hợp</div></div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Robot_Viettel.jpg" class="Image" />
      <div class="Caption">Robot giao hàng ViettelPost</div>
    </div>
  </div>

<div class="Page_Number">4</div>

---
<div class="Header">2. Định hướng đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">2.1. Mục tiêu thử nghiệm</div>
    <div class="TextBlock"><div class="Text2">Làm thử một xe nhỏ để kiểm tra khả năng giao hàng trong phạm vi ngắn (< 1 km)</div></div>
    <div class="TextBlock"><div class="Text2">Xe chạy ổn định, quay pivot được, giao đúng chỗ khoảng 90-95%</div></div>
    <div class="TextBlock"><div class="Text2">Ghi lại dữ liệu cảm biến để rút kinh nghiệm và chỉnh thuật toán điều khiển</div></div>
  </div>

<div class="Page_Number">5</div>

---

<div class="Header">2. Định hướng đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">2.2. Giới hạn phần cứng</div>
    <div class="TextBlock"><div class="Text2">Dùng module cảm biến đo góc để xe biết hướng quay</div></div>
    <div class="TextBlock"><div class="Text2">Xe nhỏ, tải được vài kg, tốc độ chậm cho an toàn</div></div>
    <div class="TextBlock"><div class="Text2">Pin chỉ chạy được khoảng 1-2 giờ, đủ cho thử nghiệm</div></div>
    <div class="TextBlock"><div class="Text2">Không có GPS hay Lidar, chỉ chạy trong khuôn viên trường</div></div>
  </div>

  <div class="Right">
    <div class="Text1">2.3. Giới hạn phần mềm</div>
    <div class="TextBlock"><div class="Text2">Điều khiển cơ bản: tiến, lùi, quay pivot dựa trên dữ liệu cảm biến</div></div>
    <div class="TextBlock"><div class="Text2">Chạy theo tuyến cố định, không có bản đồ thông minh</div></div>
    <div class="TextBlock"><div class="Text2">Có người giám sát từ xa hoặc trực tiếp khi xe chạy</div></div>
    <div class="TextBlock"><div class="Text2">Lưu dữ liệu để sau này cải thiện thuật toán</div></div>
  </div>

</div>

<div class="Page_Number">6</div>

---
<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.1. Phần cứng</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>L298N - Mạch điều khiển động cơ</b><br>
      - Là module cầu H dùng để điều khiển động cơ DC và stepper<br>
      - Hỗ trợ điều khiển 2 động cơ độc lập, có thể đảo chiều quay<br>
      - Điện áp hoạt động: 5V - 35V, dòng tối đa ~2A<br>
      - Được dùng để điều khiển bánh xe của robot giao hàng<br>
      - Tích hợp diode bảo vệ chống dòng ngược từ động cơ<br>
      - Có chân ENA/ENB để điều chỉnh tốc độ bằng PWM<br>
      - Có thể kết hợp với vi điều khiển (ESP32, Arduino) để tạo thuật toán điều khiển phức tạp<br>
      - Hạn chế: hiệu suất thấp hơn so với driver MOSFET hiện đại, sinh nhiệt khi tải lớn<br>
    </div>
  </div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Các chân chính của L298N</b><br>
      - IN1, IN2: điều khiển chiều quay động cơ 1<br>
      - IN3, IN4: điều khiển chiều quay động cơ 2<br>
      - ENA, ENB: chân kích hoạt và điều chỉnh tốc độ bằng PWM<br>
      - OUT1, OUT2: ngõ ra nối với động cơ 1<br>
      - OUT3, OUT4: ngõ ra nối với động cơ 2<br>
      - Vcc: nguồn cấp cho động cơ (5V-35V)<br>
      - 5V: nguồn cấp logic cho IC<br>
      - GND: chân nối đất chung<br>
    </div>
  </div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Module_L298N.jpg" class="Image" />
      <div class="Caption">Mạch điều khiển động cơ L298N</div>
    </div>
  </div>

</div>

<div class="Page_Number">7</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

<div class="Left">
  <div class="Text1">3.1. Phần cứng</div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Tổng quan ESP32-S3 N16R8</b><br>
      - Dual-core Xtensa LX7, 240 MHz<br>
      - 16 MB Flash, 8 MB PSRAM<br>
      - Wi-Fi + Bluetooth LE<br>
      - Hỗ trợ AI/ML vector instructions
    </div>
  </div>


  <div class="TextBlock">
    <div class="Text2">
      <b>GPIO & Ngoại vi</b><br>
      - Strap pins: GPIO0, GPIO3, GPIO45, GPIO46<br>
      - UART: nhiều cổng, UART0 thường dùng debug<br>
      - SPI: kết nối Flash ngoài, cảm biến<br>
      - I2C: linh hoạt remap, thường GPIO8/GPIO9<br>
      - ADC: nhiều chân đọc tín hiệu analog<br>
      - PWM: xuất điều khiển động cơ<br>
      - USB OTG: GPIO19/GPIO20
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Chức năng đặc biệt Espressif</b><br>
      - ESP-NOW: giao tiếp trực tiếp không cần router<br>
      - Deep Sleep / Light Sleep: tiết kiệm năng lượng<br>
      - ULP co-processor: xử lý nhẹ khi chip ngủ<br>
      - Secure Boot & Flash Encryption: bảo mật firmware<br>
      - ESP-Mesh: tạo mạng mesh mở rộng phạm vi
    </div>
  </div>
</div>


  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Esp32s3.jpg" class="Image" />
      <div class="Caption">Module ESP32-S3 N16R8</div>
    </div>
  </div>

</div>

<div class="Page_Number">8</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">


  <div class="Left">
    <div class="Text1">3.1. Phần cứng</div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Tổng quan ESP32-CAM</b><br>
      - Module ESP32 tích hợp camera OV2640<br>
      - CPU: Dual-core Xtensa LX6, tốc độ 240 MHz<br>
      - Bộ nhớ: 4 MB Flash, hỗ trợ thẻ nhớ microSD<br>
      - Wi-Fi 2.4 GHz, Bluetooth LE<br>
      - Kích thước nhỏ gọn, giá thành thấp
    </div>
  </div>


  <div class="TextBlock">
    <div class="Text2">
      <b>GPIO & Ngoại vi</b><br>
      - Camera interface: kết nối trực tiếp OV2640<br>
      - Slot microSD: lưu trữ dữ liệu hình ảnh/video<br>
      - UART: dùng để nạp firmware và debug<br>
      - GPIO: có thể xuất PWM, đọc tín hiệu cảm biến<br>
      - SPI/I2C: hỗ trợ kết nối ngoại vi khác<br>
      - Flash LED: GPIO4 điều khiển đèn flash hỗ trợ chụp ảnh
    </div>
  </div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Chức năng đặc biệt Espressif</b><br>
      - Streaming video qua Wi-Fi (HTTP, RTSP)<br>
      - Hỗ trợ nhận diện hình ảnh cơ bản (AI tại thiết bị)<br>
      - Deep Sleep để tiết kiệm năng lượng khi không quay<br>
      - ESP-NOW: giao tiếp trực tiếp với các ESP khác<br>
      - Ứng dụng: giám sát từ xa, camera robot, IoT an ninh
    </div>
  </div>
  </div>


  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Esp32Cam.jpg" class="Image" />
      <div class="Caption">Module ESP32-CAM với camera OV2640</div>
    </div>
  </div>

</div>

<div class="Page_Number">9</div>

---
<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">


  <div class="Left">
    <div class="Text1">3.1. Phần cứng</div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Tổng quan MPU6050</b><br>
      - Cảm biến 6 trục (3 trục gia tốc + 3 trục con quay hồi chuyển)<br>
      - Giao tiếp qua I2C, địa chỉ mặc định 0x68<br>
      - Điện áp hoạt động: 3V - 5V<br>
      - Tích hợp bộ xử lý tín hiệu số (DMP) để lọc dữ liệu<br>
      - Kích thước nhỏ gọn, dễ tích hợp vào robot
    </div>
  </div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Chức năng chính</b><br>
      - Đo gia tốc theo 3 trục X, Y, Z<br>
      - Đo tốc độ quay (gyroscope) theo 3 trục<br>
      - Kết hợp dữ liệu để xác định góc nghiêng, hướng di chuyển<br>
      - DMP hỗ trợ tính toán góc Euler, giảm nhiễu<br>
      - Dùng để robot biết trạng thái cân bằng và hướng quay
    </div>
  </div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Ngoại vi & Ứng dụng</b><br>
      - Kết nối với ESP32-S3 qua I2C (GPIO8 SDA, GPIO9 SCL)<br>
      - Dữ liệu cảm biến được lưu để phân tích và cải thiện thuật toán<br>
      - Ứng dụng: điều khiển xe cân bằng, robot tự hành, drone<br>
      - Kết hợp với L298N để điều chỉnh tốc độ/quay chính xác hơn
    </div>
  </div>
  </div>


  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Module_Mpu6050.jpg" class="Image" />
      <div class="Caption">Module cảm biến MPU6050</div>
    </div>
  </div>

</div>

<div class="Page_Number">10</div>

---
<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">


  <div class="Left">
    <div class="Text1">3.1. Phần cứng</div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Tổng quan LM2596</b><br>
      - Module hạ áp DC-DC (buck converter)<br>
      - IC LM2596S, hiệu suất cao, dễ sử dụng<br>
      - Điện áp đầu vào: 4V - 40V<br>
      - Điện áp đầu ra: 1.25V - 35V (có thể điều chỉnh)<br>
      - Dòng tải tối đa: ~2A liên tục, 3A ngắn hạn<br>
      - Tích hợp biến trở để chỉnh điện áp đầu ra
    </div>
  </div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Chức năng chính</b><br>
      - Chuyển đổi nguồn pin (ví dụ pin Makita 21V) xuống mức phù hợp cho vi điều khiển<br>
      - Giữ điện áp ổn định, bảo vệ thiết bị điện tử<br>
      - Hiệu suất cao hơn so với dùng điện trở hạ áp<br>
      - Có diode Schottky và cuộn cảm để giảm nhiễu
    </div>
  </div>


  <div class="TextBlock">
    <div class="Text2">
      <b>Ứng dụng trong robot</b><br>
      - Cấp nguồn ổn định 5V cho ESP32-S3, ESP32-CAM<br>
      - Cấp nguồn 3.3V cho cảm biến MPU6050<br>
      - Giúp hệ thống chạy ổn định khi pin dao động<br>
      - Bảo vệ mạch khỏi quá áp, quá dòng
    </div>
  </div>
  </div>


  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Module_Lm2596.jpg" class="Image" />
      <div class="Caption">Module hạ áp LM2596 DC-DC Buck Converter</div>
    </div>
  </div>

</div>

<div class="Page_Number">11</div>


---
<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">


  <div class="Left">
    <div class="Text1">3.1. Phần cứng</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Khối pin cell 21V</b><br>
      - Pin Li-ion ghép nhiều cell tạo thành khối<br>
      - Điện áp danh định: 21V<br>
      - Dung lượng: 1500-3000 mAh<br>
      - Có mạch bảo vệ chống quá dòng, quá áp<br>
      - Thiết kế chắc chắn, dễ tháo lắp
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Thông số kỹ thuật</b><br>
      - Điện áp đầu ra: 21V DC<br>
      - Dòng xả tối đa: ~10-15A<br>
      - Thời gian sạc: 1-2 giờ<br>
      - Chu kỳ sạc/xả: ~500-800 lần<br>
      - Kết hợp LM2596 để hạ áp xuống 5V/3.3V
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Ứng dụng trong robot</b><br>
      - Nguồn chính cho hệ thống robot<br>
      - Cấp điện cho động cơ qua L298N<br>
      - Cấp nguồn ổn định cho ESP32-S3, ESP32-CAM, MPU6050<br>
      - Dễ thay thế, có thể mang pin dự phòng<br>
      - Hoạt động liên tục 1-2 giờ thử nghiệm
    </div>
  </div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Battery_21V.jpg" class="Image" />
      <div class="Caption">Khối pin cell 21V dùng làm nguồn cho robot</div>
    </div>
  </div>

</div>

<div class="Page_Number">12</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.1. Phần cứng</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Động cơ DC mini (Arduino)</b><br>
      - Loại động cơ DC nhỏ thường dùng trong kit Arduino<br>
      - Điện áp hoạt động: 5V - 9V<br>
      - Dòng tiêu thụ: khoảng 100-250 mA<br>
      - Tốc độ quay: ~6000-12000 vòng/phút<br>
      - Kích thước nhỏ gọn, dễ lắp đặt
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Chức năng chính</b><br>
      - Tạo lực kéo cho bánh xe robot<br>
      - Có thể điều khiển tốc độ và chiều quay qua L298N<br>
      - Phù hợp cho thử nghiệm robot nhỏ, tải nhẹ<br>
      - Hoạt động ổn định trong phạm vi điện áp thấp
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Ứng dụng trong robot</b><br>
      - Dùng làm động cơ chính cho xe thử nghiệm<br>
      - Kết hợp với cảm biến MPU6050 để kiểm soát hướng quay<br>
      - Cấp nguồn từ khối pin cell 21V qua LM2596 hạ áp<br>
      - Giúp robot di chuyển trong phạm vi ngắn, tốc độ vừa phải
    </div>
  </div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image\MotorMini.jpg" class="Image" />
      <div class="Caption">Động cơ DC mini dùng trong kit Arduino</div>
    </div>
  </div>

</div>

<div class="Page_Number">13</div>

---
<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.1. Phần cứng</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Servo SG90</b><br>
      - Loại servo mini phổ biến trong các dự án Arduino<br>
      - Điện áp hoạt động: 4.8V - 6V<br>
      - Momen xoắn: ~1.8 kg·cm<br>
      - Góc quay: 0° - 180° (điều khiển bằng PWM)<br>
      - Kích thước nhỏ gọn, trọng lượng nhẹ
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Chức năng chính</b><br>
      - Điều khiển góc quay chính xác<br>
      - Phù hợp cho robot nhỏ, cánh tay gắp, cơ cấu lái<br>
      - Có thể điều khiển trực tiếp bằng ESP32 hoặc Arduino<br>
      - Tín hiệu PWM từ vi điều khiển quyết định vị trí servo
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Ứng dụng trong robot</b><br>
      - Dùng để điều khiển cơ cấu mở/đóng hộp hàng<br>
      - Hỗ trợ cơ cấu lái hoặc gắp vật<br>
      - Kết hợp với cảm biến để tạo chuyển động chính xác<br>
      - Hoạt động ổn định trong phạm vi điện áp thấp
    </div>
  </div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image\Servo_Sg90.jpg" class="Image" />
      <div class="Caption">Servo mini SG90</div>
    </div>
  </div>

</div>

<div class="Page_Number">14</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  
  <div class="Left">
    <div class="Text1">3.2. Phần mềm</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Framework .NET C#</b><br>
      - Framework mạnh mẽ để xây dựng ứng dụng đa nền tảng (Windows, Android, iOS)<br>
      - Ngôn ngữ chính: C# với cú pháp hiện đại, dễ học<br>
      - Hỗ trợ thư viện phong phú, dễ kết nối phần cứng qua UART/I2C/SPI<br>
      - Tích hợp công cụ quản lý bộ nhớ, bảo mật, và xử lý đa luồng<br>
      - Ứng dụng: viết phần mềm điều khiển, giám sát robot, xử lý dữ liệu cảm biến
    </div>
  </div>

  <div style="text-align: center; margin-top:20px;">
    <img src="image\DotNet.jpg" class="Image" />
    <div class="Caption">Microsoft .NET Framework </div>
  </div>
  </div>

  
  <div class="Right">
    <div class="TextBlock">
      <div class="Text2">
        <b>Visual Studio 2022 Community</b><br>
        - IDE miễn phí của Microsoft, hỗ trợ đầy đủ tính năng<br>
        - Tích hợp công cụ debug, testing, quản lý project<br>
        - Hỗ trợ nhiều ngôn ngữ: C#, C++, Python<br>
        - Tích hợp Git, dễ dàng triển khai ứng dụng<br>
        - Giao diện trực quan, giúp lập trình viên phát triển nhanh chóng
      </div>
    </div>

  <div style="text-align: center; margin-top:20px;">
    <img src="image/Visual_Studio_2022.jpg" class="Image" />
    <div class="Caption">Visual Studio 2022 Community - công cụ phát triển phần mềm</div>
  </div>
  </div>

</div>

<div class="Page_Number">15</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

<div class="Left">
<div class="Text1">3.2. Phần mềm</div>

<div class="TextBlock">
<div class="Text2">
<b>MongoDB</b><br>
- Hệ quản trị cơ sở dữ liệu NoSQL phổ biến, lưu trữ dữ liệu dưới dạng document (BSON/JSON)<br>
- Cho phép lưu trữ linh hoạt, dễ mở rộng, phù hợp với hệ thống robot giao hàng<br>
- Hỗ trợ truy vấn động, cấu trúc dữ liệu không cố định như bảng SQL<br>
- Là nền tảng lõi mà cả Atlas và Compass đều sử dụng để thao tác và triển khai
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>MongoDB Atlas</b><br>
- Dịch vụ triển khai MongoDB trên nền tảng đám mây<br>
- Cho phép quản lý, mở rộng và bảo mật dữ liệu dễ dàng<br>
- Hỗ trợ lưu trữ đơn hàng, sản phẩm, người dùng trong hệ thống<br>
- Kết nối trực tiếp với ứng dụng MAUI App Android qua driver MongoDB
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>MongoDB Compass</b><br>
- Công cụ giao diện đồ họa (GUI) để quản lý MongoDB<br>
- Cho phép xem, chỉnh sửa, và phân tích dữ liệu trực quan<br>
- Hỗ trợ tạo collection, document, và truy vấn dữ liệu dễ dàng<br>
- Phù hợp cho việc kiểm tra trạng thái đơn hàng, sản phẩm trong quá trình thử nghiệm
</div>
</div>
</div>

<div class="Right">
<div style="text-align: center; margin-bottom:20px;">
<img src="image/MongoDb_Atlas_Compass.jpg" class="Image" />
<div class="Caption">MongoDB - nền tảng lõi, Atlas triển khai cloud, Compass thao tác trực quan</div>
</div>
</div>

</div>

<div class="Page_Number">16</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

<div class="Left">
  <div class="Text1">3.2. Phần mềm</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>API VietQR</b><br>
      - API chính thức tại <code>https://api.vietqr.io/v2/generate</code><br>
      - Cho phép tạo mã QR thanh toán dựa trên thông tin tài khoản nhận tiền<br>
      - Yêu cầu chứng thực bằng <code>x-client-id</code> và <code>x-api-key</code> trong header<br>
      - Hỗ trợ nhiều ngân hàng nội địa qua mã BIN (acqId)<br>
      - Ứng dụng: tích hợp vào hệ thống robot giao hàng để khách thanh toán nhanh chóng
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Các tham số chính trong Request</b><br>
      - <code>accountNo</code>: Số tài khoản (6–19 ký tự)<br>
      - <code>accountName</code>: Tên tài khoản (5–50 ký tự, viết hoa, không dấu)<br>
      - <code>acqId</code>: Mã BIN ngân hàng (6 chữ số)<br>
      - <code>amount</code>: Số tiền (tối đa 13 ký tự)<br>
      - <code>addInfo</code>: Nội dung chuyển khoản (≤ 25 ký tự, không dấu)<br>
      - <code>format</code>: Định dạng trả về (text, image, dataURI)<br>
      - <code>template</code>: Mẫu QR (compact, compact2, qr_only, print)
    </div>
  </div>
</div>

<div class="Right">
  <div class="TextBlock">
    <div class="Text2">
      <b>Kết quả trả về</b><br>
      - <code>qrCode</code>: Chuỗi dữ liệu QR<br>
      - <code>qrDataURL</code>: Ảnh QR dạng Data URI (có thể hiển thị trực tiếp)<br>
      - <code>accountName</code>, <code>accountNo</code>: Thông tin tài khoản nhận<br>
      - <code>amount</code>, <code>addInfo</code>: Thông tin giao dịch<br><br>
      <b>Lợi ích tích hợp</b><br>
      - Giúp khách hàng thanh toán nhanh chóng, không nhập tay<br>
      - Giảm sai sót, tăng độ tin cậy<br>
      - Tích hợp trực tiếp vào ứng dụng giám sát robot giao hàng
    </div>
  </div>

  <div style="text-align: center; margin-top:20px;">
    <img src="image/VietQr_QrImage.jpg" class="Image" />
    <div class="Caption">Ảnh minh họa QR Code tạo bởi API VietQR</div>
  </div>
</div>

</div>

<div class="Page_Number">18</div>

---


<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.2. Phần mềm</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Giao thức HTTP</b><br>
      - HTTP (Hypertext Transfer Protocol) là giao thức truyền tải dữ liệu phổ biến trên Internet<br>
      - Hoạt động theo mô hình client - server: client gửi request, server trả về response<br>
      - Hỗ trợ truyền dữ liệu dạng text, hình ảnh, video, JSON<br>
      - Dễ dàng tích hợp với các thiết bị IoT nhờ tính đơn giản và phổ biến<br>
      - Được dùng làm nền tảng cho việc truyền dữ liệu video từ ESP32-CAM về máy tính hoặc thiết bị di động
    </div>
  </div>
  </div>

  <div class="Right">
    <div class="TextBlock">
      <div class="Text2">
        <b>Ứng dụng HTTP cho ESP32-CAM</b><br>
        - ESP32-CAM có thể khởi tạo web server nội bộ<br>
        - Camera stream video trực tiếp qua HTTP, client chỉ cần truy cập địa chỉ IP<br>
        - Định dạng phản hồi: multipart/x-mixed-replace; boundary=frame<br>
        - Dữ liệu video được gửi dưới dạng MJPEG hoặc HTTP stream<br>
        - Tích hợp dễ dàng với ứng dụng giám sát xe: hiển thị video trên máy tính, điện thoại<br>
        - Ưu điểm: đơn giản, không cần phần mềm phức tạp, chỉ cần trình duyệt web<br>
        - Ứng dụng: giám sát hành trình xe, quan sát môi trường xung quanh robot
      </div>
    </div>
  </div>

  <div style="text-align: center; margin-top:20px;">
    <img src="image\HTTP.jpg" class="Image" />
    <div class="Caption">Giao thức HTTP</div>
  </div>
  </div>

</div>

<div class="Page_Number">16</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.2. Phần mềm</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Giao thức MQTT</b><br>
      - MQTT (Message Queuing Telemetry Transport) là giao thức truyền thông nhẹ dành cho IoT<br>
      - Hoạt động theo mô hình publish - subscribe, giúp nhiều thiết bị trao đổi dữ liệu qua broker<br>
      - Tiêu tốn ít băng thông, phù hợp cho thiết bị nhỏ như ESP32<br>
      - Hỗ trợ truyền dữ liệu cảm biến, trạng thái thiết bị theo thời gian thực<br>
      - Được thiết kế để hoạt động ổn định ngay cả khi mạng không ổn định
    </div>
  </div>
  </div>

  <div class="Right">
    <div class="TextBlock">
      <div class="Text2">
        <b>Ứng dụng MQTT trong robot giao hàng</b><br>
        - Dùng để gửi dữ liệu cảm biến từ xe về server giám sát<br>
        - Cho phép nhiều client (máy tính, điện thoại) cùng nhận dữ liệu từ robot<br>
        - Hỗ trợ điều khiển xe từ xa qua các topic riêng biệt<br>
        - Ưu điểm: nhẹ, nhanh, dễ mở rộng khi có nhiều robot cùng hoạt động<br>
        - Ứng dụng: giám sát trạng thái pin, tốc độ, hướng di chuyển, cảnh báo sự cố
      </div>
    </div>
  </div>

<div style="text-align: center; margin-top:20px;">
    <img src="image\MQTT.jpg" class="Image" />
    <div class="Caption">Giao thức MQTT</div>
  </div>
  </div>

</div>

<div class="Page_Number">17</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.2. Phần mềm</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Giao thức ESP-NOW</b><br>
      - ESP-NOW là giao thức truyền thông không dây nội bộ do Espressif phát triển<br>
      - Cho phép các thiết bị ESP32 giao tiếp trực tiếp với nhau mà không cần router Wi-Fi<br>
      - Dựa trên chuẩn Wi-Fi 802.11 nhưng nhẹ hơn nhiều so với TCP/IP<br>
      - Hỗ trợ truyền dữ liệu nhanh, độ trễ thấp, tiết kiệm năng lượng<br>
      - Phù hợp cho các ứng dụng IoT, robot, cảm biến phân tán
    </div>
  </div>
  </div>

  <div class="Right">
    <div class="TextBlock">
      <div class="Text2">
        <b>Ứng dụng ESP-NOW trong robot giao hàng</b><br>
        - ESP32-CAM nhận lệnh điều khiển từ HiveMQ qua MQTT<br>
        - Sau đó gửi lệnh qua ESP-NOW đến ESP32-S3 để điều khiển động cơ<br>
        - ESP32-S3 có thể phản hồi trạng thái cảm biến ngược lại qua ESP-NOW<br>
        - Ưu điểm: không cần dây UART, không phụ thuộc mạng Wi-Fi<br>
        - Giúp hệ thống gọn nhẹ, linh hoạt, dễ triển khai trong môi trường thử nghiệm
      </div>
    </div>
  </div>

  <div style="text-align: center; margin-top:20px;">
    <img src="image\ESP_NOW.jpg" class="Image" />
    <div class="Caption">Giao tiếp nội bộ giữa ESP32-CAM và ESP32-S3 bằng ESP-NOW</div>
  </div>
</div>

<div class="Page_Number">18</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.3. Phân tích hệ thống</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Sơ đồ nguyên lý - sơ đồ mạch phần cứng</b><br>
      - Hệ thống robot giao hàng được xây dựng từ các module chính:<br>
      + ESP32-S3: vi điều khiển trung tâm, nhận lệnh và điều khiển động cơ<br>
      + ESP32-CAM: camera giám sát, gateway nhận lệnh từ MQTT<br>
      + L298N: mạch cầu H điều khiển động cơ DC<br>
      + MPU6050: cảm biến gia tốc và con quay hồi chuyển<br>
      + LM2596: mạch hạ áp cấp nguồn ổn định<br>
      + Pin Li-ion 21V: nguồn chính cho toàn hệ thống<br>
      - Các kết nối chính:<br>
      + ESP32-S3 giao tiếp MPU6050 qua I2C<br>
      + ESP32-S3 xuất PWM điều khiển L298N - động cơ<br>
      + LM2596 hạ áp từ pin 21V xuống 5V/3.3V cấp cho ESP32 và cảm biến<br>
      + ESP32-CAM kết nối Wi-Fi để truyền video, đồng thời giao tiếp ESP32-S3 qua ESP-NOW<br>
    </div>
  </div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image/Device_Diagram.jpg" style="width:4800px; height:1200px;" />
      <div class="Caption">Sơ đồ nguyên lý mạch phần cứng robot giao hàng</div>
    </div>
  </div>

</div>

<div class="Page_Number">19</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.3. Phân tích hệ thống</div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Use Case tổng quát</b><br>
      - Hệ thống có 3 nhóm người dùng chính:<br>
      + <b>User</b>: đặt hàng, thanh toán, theo dõi trạng thái<br>
      + <b>Seller</b>: xác nhận đơn, quản lý sản phẩm<br>
      + <b>Admin</b>: giám sát hệ thống, theo dõi giao hàng<br>
      - Mỗi nhóm người dùng tương tác với hệ thống qua các chức năng riêng biệt<br>
      - Các chức năng được phân chia rõ ràng để đảm bảo bảo mật và hiệu quả vận hành
    </div>
  </div>

  <div class="TextBlock">
    <div class="Text2">
      <b>Use Case chi tiết</b><br>
      - <b>User Functions</b>:<br>
      + Đặt hàng - lưu vào MongoDB<br>
      + Thanh toán bằng mã QR - gọi VietQR API<br>
      + Xem danh sách sản phẩm - từ Seller Portal<br>
      + Theo dõi trạng thái đơn - nhận từ Robot qua MQTT<br><br>
      - <b>Seller Functions</b>:<br>
      + Quản lý sản phẩm - thêm/xóa/sửa<br>
      + Xác nhận đơn - gửi trạng thái đến hệ thống<br><br>
      - <b>Admin Functions</b>:<br>
      + Giám sát giao hàng - nhận dữ liệu từ Robot<br>
      + Quản lý hệ thống - kiểm tra trạng thái các module
    </div>
  </div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="../Maui_App_UseCase.png" class="Image" style="width:100%; height:auto;" />
      <div class="Caption">Sơ đồ Use Case tổng quát của hệ thống</div>

  <img src="../Maui_App_UseCaseAll.png" class="Image" style="width:100%; height:auto; margin-top:20px;" />
  <div class="Caption">Sơ đồ kiến trúc hệ thống - luồng xử lý giữa các thành phần</div>
  </div>
  </div>

</div>

<div class="Page_Number">20</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">


<div class="Left">
  <div class="TextBlock">
    <div class="Text2">
      <b>Kiến trúc tầng trong MAUI App Android</b>
      <p>Ứng dụng MAUI Android được thiết kế theo mô hình nhiều lớp để dễ bảo trì và mở rộng.</p>

  <p><b>1. UI Layer - Giao diện người dùng:</b> gồm các thành phần chính như MainPage (trang chính), PaymentPage (thanh toán), và QrScanPage (quét mã QR).</p>

  <p><b>2. Service Layer - Xử lý nghiệp vụ:</b> bao gồm CommunicationService (giao tiếp với hệ thống), PaymentService (xử lý thanh toán), và MongoDbService (lưu trữ đơn hàng)<p>

  <p><b>3. Data Layer - Quản lý dữ liệu:</b> quản lý các mô hình dữ liệu như OrderModel (dữ liệu đơn hàng) và ProductModel (dữ liệu sản phẩm)./p>

  <p><b>4. System Integration - Tích hợp hệ thống ngoài:</b> kết nối với các dịch vụ bên ngoài như MQTT Broker (gửi lệnh đến robot), VietQR API (tạo mã QR thanh toán), và MongoDB (lưu trữ dữ liệu đơn hàng).</p>
</div>
</div>
</div>

<div class="Right">
  <div style="text-align: center;">
      <img src="../Maui_App_Layer.png" class="Image" style="width:100%; height:auto;" />
      <div class="Caption">Kiến trúc các tầng trong MAUI App Android</div>
</div>
</div>
</div>

<div class="Page_Number">21</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">

  <div class="Left">
    <div class="Text1">3.3. Phân tích hệ thống</div>

<div class="TextBlock">
  <div class="Text2">
<b>Sơ đồ lớp (Class Diagram)</b>
<p>
  Sơ đồ lớp thể hiện các đối tượng chính trong hệ thống giao hàng và mối quan hệ giữa chúng. 
  Mỗi lớp đại diện cho một thực thể như người dùng, người bán, sản phẩm, và đơn hàng.
</p>

<p>
  <b>1. Lớp User:</b> chứa thông tin người dùng như user_id, username, password, email, và lịch sử đơn hàng.
</p>

<p>
  <b>2. Lớp Seller:</b> quản lý thông tin người bán gồm seller_id, tên cửa hàng, tài khoản, và danh sách sản phẩm.
</p>

<p>
  <b>3. Lớp Product:</b> mô tả sản phẩm với product_id, tên, mô tả, và giá.
</p>

<p>
  <b>4. Lớp Order:</b> lưu thông tin đơn hàng gồm order_id, user_id, product_id, seller_id, địa chỉ, trạng thái, và thời gian tạo.
</p>

<p>
  Các mối quan hệ giữa lớp:<br>
  • Một User có thể tạo nhiều Order<br>
  • Một Seller quản lý nhiều Product và xác nhận nhiều Order<br>
  • Một Order có thể chứa nhiều Product<br>
  • Mỗi Product thuộc về một Seller
</p>
</div>
</div>
</div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="../Maui_App_Class.png" class="Image" style="width:100%; height:auto;" />
      <div class="Caption">Sơ đồ lớp hệ thống giao hàng</div>
    </div>
  </div>

</div>

<div class="Page_Number">22</div>

---

<div class="Header">3. Phân tích đề tài</div>
<div class="Grid">


<div class="Left">
<div class="Text1">3.3. Phân tích hệ thống</div>

<div class="TextBlock">
<div class="Text2">
  <b>Sơ đồ trình tự (Sequence Diagram)</b>
  <p>
    Sơ đồ trình tự mô tả luồng xử lý đơn hàng từ lúc người dùng chọn sản phẩm đến khi robot nhận lệnh giao hàng. 
    Các thành phần tham gia gồm: User, MAUI App Android, MongoDB, API VietQR, MQTT/HTTP Broker và Robot.
  </p>

  <p>
    <b>Quy trình xử lý:</b><br>
    1. User chọn sản phẩm và xác nhận đơn hàng trên MAUI App<br>
    2. MAUI App lưu thông tin đơn hàng vào MongoDB<br>
    3. MAUI App gọi API VietQR để tạo mã QR thanh toán<br>
    4. User quét mã QR trên QrScanPage<br>
    5. MAUI App xác thực kết quả thanh toán với API VietQR<br>
    6. API VietQR trả về trạng thái thanh toán<br>
    7. MAUI App cập nhật trạng thái đơn hàng trong MongoDB<br>
    8. MAUI App gửi thông tin đơn hàng đến Robot qua MQTT/HTTP Broker<br>
    9. Broker truyền dữ liệu đơn hàng đến Robot<br>
    10. MAUI App phản hồi trạng thái giao hàng cho User
  </p>
</div>
</div>
</div>

<div class="Right">
<div style="text-align: center;">
<img src="../Maui_App_Flow.png" class="Image" />
<div class="Caption">Sơ đồ trình tự xử lý đơn hàng và giao hàng</div>
</div>
</div>

</div>

<div class="Page_Number">23</div>

---

<div class="Header">4. Quá trình thực nghiệm</div>
<div class="Grid">

<div class="Left">
<div class="Text1">4.1. Thực nghiệm phần cứng</div>

<div class="TextBlock">
<div class="Text2">
<b>Quá trình lắp ráp thiết bị</b><br>
- Các module phần cứng được kết nối theo sơ đồ nguyên lý đã thiết kế<br>
- ESP32-S3 được gắn vào khung xe, kết nối với L298N để điều khiển động cơ<br>
- ESP32-CAM được cố định phía trước xe để giám sát hành trình<br>
- Cảm biến MPU6050 được đặt gần tâm xe để đo góc nghiêng và hướng quay<br>
- Module LM2596 hạ áp từ pin 21V xuống 5V và 3.3V cấp cho các thiết bị<br>
- Servo SG90 được gắn vào cơ cấu mở hộp hàng<br>
- Các dây nguồn, tín hiệu được hàn và cố định bằng keo nhiệt để đảm bảo chắc chắn
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>Kiểm tra kết nối</b><br>
- Sau khi lắp ráp, tiến hành kiểm tra từng module bằng nguồn thử<br>
- Đảm bảo các chân GPIO, nguồn cấp và tín hiệu điều khiển hoạt động đúng<br>
- Kiểm tra khả năng quay động cơ, phản hồi cảm biến, truyền dữ liệu camera<br>
- Ghi nhận các lỗi kết nối và khắc phục trước khi chạy thử toàn hệ thống
</div>
</div>
</div>

<div class="Right">
<div style="text-align: center;">
<img src="image/Device.jpg" class="Image" />
<div class="Caption">Hình ảnh thực tế quá trình lắp ráp thiết bị phần cứng</div>
</div>
</div>

</div>

<div class="Page_Number">18</div>

---

<div class="Header">4. Quá trình thực nghiệm</div>
<div class="Grid">

<div class="Left">
<div class="Text1">4.1. Thực nghiệm phần cứng</div>

<div class="TextBlock">
<div class="Text2">
<b>Mô hình xe thử nghiệm (MVP)</b><br>
- Xe được lắp ráp từ các module phần cứng đã phân tích<br>
- Khung xe đơn giản, tập trung vào chức năng hơn thẩm mỹ<br>
- Có gắn thanh khe tản nhiệt ở đáy để tránh nước mưa từ trên nóc chảy xuống<br>
- Các linh kiện (ESP32-S3, ESP32-CAM, L298N, MPU6050, LM2596, pin 21V, động cơ, servo) được bố trí theo sơ đồ nguyên lý<br>
- Dây điện và module được cố định bằng keo nhiệt và khung nhựa<br>
- Mục tiêu: kiểm chứng khả năng vận hành, không đặt nặng hình thức
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>Ý nghĩa thực nghiệm</b><br>
- Dù mô hình chưa đẹp, nhưng phản ánh tư duy giải quyết vấn đề thực tế<br>
- Chống nước, chống rung, đảm bảo an toàn cho mạch điện<br>
- Là bước quan trọng để kiểm tra tính khả thi trước khi phát triển phiên bản hoàn thiện
</div>
</div>
</div>

<div class="Right">
<div style="text-align: center;">
<img src="image/MVP.jpg" class="Image" />
<div class="Caption">Mô hình xe thử nghiệm (MVP) – tập trung vào chức năng</div>
</div>
</div>

</div>

<div class="Page_Number">19</div>

---

<div class="Header">4. Quá trình thực nghiệm</div>
<div class="Grid">

<div class="Left">
<div class="Text1">4.1. Thực nghiệm phần cứng</div>

<div class="TextBlock">
<div class="Text2">
<b>Thách thức và điều chỉnh thiết kế</b><br>
- Ban đầu định hướng dùng nhiều module nhưng phải bỏ bớt khi thực nghiệm<br>
- Bỏ module <b>A7680C</b> (4G) vì phức tạp, không cần thiết cho thử nghiệm<br>
- Bỏ driver <b>BTS7960</b> vì quá tải, thay bằng L298N đơn giản hơn<br>
- Bỏ động cơ 12V DC, chỉ dùng động cơ DC mini phù hợp tải nhẹ<br>
- MPU6050 chỉ dùng bản 3 trục thay vì 9 trục để giảm chi phí<br>
- Không dùng mạch PCB lỗ, thay bằng breadboard để test nhanh, sau đó cố định bằng keo nóng<br>
- Dùng dây điện AWG 14 để đảm bảo tải dòng cho motor<br>
- Chia nguồn riêng cho L298N (động cơ) và nguồn điều khiển (ESP32, cảm biến)
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>Bài học rút ra</b><br>
- Luôn cần linh hoạt, sẵn sàng thay đổi thiết kế khi gặp vấn đề thực tế<br>
- Ưu tiên tính khả thi và ổn định hơn là hình thức<br>
- Thử nghiệm nhanh bằng breadboard và keo nóng giúp tiết kiệm thời gian<br>
- Việc chọn dây điện, chia nguồn đúng cách là yếu tố quan trọng để tránh hỏng mạch
</div>
</div>
</div>

<div class="Right">
<div style="text-align: center; margin-bottom:20px;">
<img src="image/BTS7960.jpg" class="Image" />
<div class="Caption">Module BTS7960 – driver công suất cao, đã bỏ trong quá trình thử nghiệm</div>
</div>

<div style="text-align: center;">
<img src="image/A7680C.jpg" class="Image" />
<div class="Caption">Module A7680C – modem 4G, không dùng trong phiên bản thử nghiệm</div>
</div>
</div>

</div>

<div class="Page_Number">21</div>

---


<div class="Header">4. Quá trình thực nghiệm</div>
<div class="Grid">

<div class="Left">
<div class="Text1">4.2. Thực nghiệm phần mềm</div>

<div class="TextBlock">
<div class="Text2">
<b>Giao diện MAUI App viết bằng C#</b><br>
- Ứng dụng được xây dựng bằng .NET MAUI, ngôn ngữ C#<br>
- Chạy trên Android, giao tiếp với robot qua HTTP và MQTT<br>
- Giao diện đơn giản, dễ thao tác, phù hợp cho thử nghiệm
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>Các màn hình chính</b><br>
- <b>Trang chủ:</b> hiển thị danh sách sản phẩm, đơn hàng, nút thanh toán<br>
- <b>Chi tiết đơn hàng:</b> hiển thị mã đơn, tên sản phẩm, giá tiền<br>
- <b>Quét QR:</b> dùng camera để quét mã đơn hàng<br>
- <b>Giám sát robot:</b> hiển thị trạng thái robot, video từ ESP32-CAM<br>
- <b>Cài đặt:</b> cấu hình địa chỉ MQTT, HTTP, thông tin người dùng
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>Ý nghĩa</b><br>
- Giao diện giúp người dùng thao tác nhanh, trực quan<br>
- Là cầu nối giữa người dùng và hệ thống robot<br>
- Dễ mở rộng thêm chức năng như theo dõi vị trí, trạng thái pin, cảnh báo
</div>
</div>
</div>

<div class="Right">
<div style="text-align: center;">
<img src="image\UI_Test.jpg" class="Image" />
<div class="Caption">Giao diện MAUI App Android – màn hình danh sách sản phẩm và đơn hàng</div>
</div>
</div>

</div>

<div class="Page_Number">22</div>

---

<div class="Header">4. Quá trình thực nghiệm</div>
<div class="Grid">

<div class="Left">
<div class="Text1">4.2. Thực nghiệm phần mềm</div>

<div class="TextBlock">
<div class="Text2">
<b>Tính năng thanh toán QR</b><br>
- Ứng dụng MAUI App tích hợp API VietQR Sandbox để tạo mã QR thanh toán<br>
- Mỗi đơn hàng có mã riêng (ví dụ: DH_003, DH_004...)<br>
- Khi người dùng nhấn “Thanh toán”, app gửi yêu cầu tạo QR đến VietQR<br>
- QR hiển thị thông tin người nhận, số tiền, nội dung chuyển khoản chứa mã đơn hàng
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>Demo xác nhận thanh toán</b><br>
- Sau khi người dùng quét QR và chuyển khoản, hệ thống kiểm tra giao dịch<br>
- So sánh nội dung chuyển khoản với mã đơn hàng (ví dụ: “DH_003”)<br>
- Nếu trùng khớp, đơn hàng được đánh dấu là “Đã thanh toán”<br>
- Quá trình xác nhận được thực hiện qua API kiểm tra lịch sử giao dịch (mock từ VietQR Sandbox)
</div>
</div>

<div class="TextBlock">
<div class="Text2">
<b>Ý nghĩa</b><br>
- Giúp người dùng thanh toán nhanh chóng, không cần nhập tay<br>
- Tăng độ tin cậy và tự động hóa quy trình xác nhận đơn hàng<br>
- Là bước thử nghiệm quan trọng để tích hợp thanh toán thật trong tương lai
</div>
</div>
</div>

<div class="Right">
<div style="text-align: center;">
<img src="image/UI_Test.jpg" style="width:100%; max-width:500px;" />
<div class="Caption">Giao diện MAUI App – danh sách sản phẩm và nút thanh toán QR</div>
</div>
</div>

</div>

<div class="Page_Number">23</div>

---


<div class="Header">4. Quét đơn hàng thành công</div>

<div class="Grid">
  <div class="Left">
    <div class="Text1">Minh chứng xác nhận thanh toán</div>
    <div class="TextBlock">
      <div class="Text2">
        Sau khi khách hàng quét mã QR và thanh toán thành công, hệ thống hiển thị xác nhận đơn hàng đã được thanh toán. Cơ cấu mở hatch hàng được kích hoạt tự động.
      </div>
    </div>

  <div class="Text1">Thông điệp hiển thị</div>
  <div class="TextBlock">
    <div class="Text2">
      "Order DH_003 confirmed as paid!<br>
      Cargo hatch OPENED"
    </div>
  </div>

  <div class="Text1">Ý nghĩa</div>
  <div class="TextBlock">
    <div class="Text2">
      Hệ thống tích hợp giữa phần mềm xác nhận giao dịch và phần cứng điều khiển hatch giúp đảm bảo quy trình giao hàng tự động, an toàn và chính xác.
    </div>
  </div>
  </div>

  <div class="Right">
    <div style="text-align: center;">
      <img src="image/Scan_Success.jpg" class="Image" />
      <div class="Caption">Màn hình xác nhận đơn hàng DH_003 đã thanh toán thành công</div>
    </div>
  </div>
</div>

<div class="Page_Number">19</div>


---
