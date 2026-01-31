# GIAO DIỆN TRANG CHỦ

## 1. MỤC ĐÍCH

Giao diện Trang chủ là điểm truy cập chính của người chơi sau khi đăng nhập, cung cấp các chức năng chính và thông tin tổng quan về tài khoản.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Avatar người dùng: Hiển thị ở góc trái hoặc góc phải
- Tên hiển thị: Hiển thị bên cạnh avatar
- Level hiện thị: Hiển thị dưới tên
- Thanh kinh nghiệm: Hiển thị tiến độ lên cấp tiếp theo
- Nút Thông báo: Hiển thị số lượng thông báo chưa đọc

### 2.2. Phần Thông Tin Nhanh
- Điểm xếp hạng hiện tại:
  - Nhãn: "Điểm xếp hạng"
  - Giá trị: Hiển thị số điểm
  
- Thứ hạng hiện tại:
  - Nhãn: "Thứ hạng"
  - Giá trị: Vị trí trên bảng xếp hạng
  
- Số trận đã đấu:
  - Nhãn: "Số trận"
  - Giá trị: Tổng số trận đã chơi
  
- Tỉ lệ thắng:
  - Nhãn: "Tỉ lệ thắng"
  - Giá trị: Phần trăm thắng (%)

### 2.3. Phần Chức Năng Chính
- Nút Bắt đầu thi đấu:
  - Text: "Bắt đầu thi đấu"
  - Kiểu: Primary button, kích thước lớn
  - Hành động: Chuyển đến Giao diện Cấu hình thi đấu
  
- Nút Bảng xếp hạng:
  - Text: "Bảng xếp hạng"
  - Kiểu: Secondary button
  - Hành động: Chuyển đến Giao diện Bảng xếp hạng
  
- Nút Lịch sử thi đấu:
  - Text: "Lịch sử thi đấu"
  - Kiểu: Secondary button
  - Hành động: Hiển thị lịch sử các trận đấu

### 2.4. Phần Sự Kiện (Banner List)
- Tiêu đề: "Sự kiện đang diễn ra"
- Danh sách sự kiện:
  - Kiểu hiển thị: Carousel (Slide trượt ngang)
  - Mỗi item sự kiện bao gồm:
    - Ảnh banner sự kiện
    - Tên sự kiện
- Hành động: Nhấn vào banner để xem chi tiết Popup

### 2.5. Phần Menu Dưới (Bottom Navigation)
- Trang chủ (Active)
- Thi đấu
- Xếp hạng (My Rank)
- Cá nhân

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Bắt Đầu Thi Đấu
1. Người dùng nhấn nút "Bắt đầu thi đấu"
2. Hệ thống chuyển đến Giao diện Cấu hình thi đấu

### 3.2. Luồng Xem Chi Tiết Sự Kiện
1. Người dùng nhấn vào banner sự kiện
2. Hệ thống hiển thị Popup chi tiết

### 3.3. Luồng Chuyển Tab
1. Người dùng nhấn các tab ở Bottom Bar
2. Hệ thống chuyển màn hình tương ứng
