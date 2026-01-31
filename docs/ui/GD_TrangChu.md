# GIAO DIỆN TRANG CHỦ

## 1. MỤC ĐÍCH

Giao diện Trang chủ là điểm truy cập chính của người chơi sau khi đăng nhập, cung cấp các chức năng chính và thông tin tổng quan về tài khoản.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Avatar người dùng: Hiển thị ở góc trái hoặc góc phải
- Tên hiển thị: Hiển thị bên cạnh avatar
- Level hiện thị: Hiển thị dưới tên
- Thanh kinh nghiệm: Hiển thị tiến độ lên cấp tiếp theo
- Nút Bạn bè: Link đến Giao diện Bạn bè (Danh sách, Tìm kiếm, Lời mời)
- Nút Thông báo: Hiển thị số lượng thông báo chưa đọc
- Nút Cài đặt: Truy cập các cài đặt hệ thống

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

### 2.4. Phần Sự Kiện
- Tiêu đề: "Sự kiện đang diễn ra"
- Danh sách sự kiện:
  - Kiểu hiển thị: Carousel hoặc List trượt ngang
  - Mỗi item sự kiện bao gồm:
    - Ảnh banner sự kiện
    - Tên sự kiện
    - Thời gian còn lại
    - Nút "Tham gia"
- Hành động: Nhấn vào sự kiện để xem chi tiết

### 2.5. Phần Menu Dưới (Bottom Navigation)
- Trang chủ:
  - Nhãn: "Trang chủ"
  - Hành động: Giữ nguyên ở trang hiện tại
  
- Thi đấu:
  - Nhãn: "Thi đấu"
  - Hành động: Chuyển đến Giao diện Cấu hình thi đấu
  
- Xếp hạng:
  - Nhãn: "Xếp hạng"
  - Hành động: Chuyển đến Giao diện Bảng xếp hạng
  
- Cá nhân:
  - Nhãn: "Cá nhân"
  - Hành động: Chuyển đến Giao diện Thông tin cá nhân

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Bắt Đầu Thi Đấu
1. Người dùng nhấn nút "Bắt đầu thi đấu"
2. Hệ thống chuyển đến Giao diện Cấu hình thi đấu

### 3.2. Luồng Xem Bảng Xếp Hạng
1. Người dùng nhấn nút "Bảng xếp hạng" hoặc tab "Xếp hạng"
2. Hệ thống chuyển đến Giao diện Bảng xếp hạng

### 3.3. Luồng Tham Gia Sự Kiện
1. Người dùng nhấn vào một sự kiện trong carousel
2. Hệ thống hiển thị popup chi tiết sự kiện:
   - Banner và Tên sự kiện
   - Mô tả quy tắc
   - Danh sách phần thưởng
   - Thời gian còn lại
   - Số vé hoặc lượt chơi còn lại
3. Người dùng nhấn nút "Tham gia":
   - Kiểm tra số dư vé/lượt chơi
   - Nếu đủ: Trừ vé, bắt đầu thi đấu với bộ câu hỏi sự kiện
   - Nếu thiếu: Gợi ý mua thêm hoặc xem quảng cáo

### 3.4. Luồng Xem Thông Tin Cá Nhân
1. Người dùng nhấn tab "Cá nhân" hoặc avatar
2. Hệ thống chuyển đến Giao diện Thông tin cá nhân

### 3.5. Luồng Tải Lại Dữ Liệu
1. Người dùng kéo màn hình xuống (Pull to refresh)
2. Hệ thống tải lại:
   - Thông tin người dùng
   - Danh sách sự kiện
   - Bảng xếp hạng nhanh

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Hiển Thị Thông Tin
- Thông tin người dùng phải được tải ngay khi vào trang
- Sự kiện hiển thị phải là các sự kiện đang hoạt động (Active)
- Bảng xếp hạng hiển thị top 3 hoặc top 5

### 4.2. Cập Nhật Dữ Liệu
- Thông tin người dùng tự động cập nhật sau mỗi trận đấu
- Danh sách sự kiện cập nhật mỗi khi vào trang
- Thông báo realtime nếu có sự kiện mới

### 4.3. Trạng Thái
- Loading: Hiển thị skeleton loading khi đang tải dữ liệu
- Lỗi: Hiển thị thông báo lỗi và nút "Thử lại"
- Trống: Hiển thị thông báo nếu không có sự kiện nào

## 5. RESPONSIVE

### 5.1. Desktop
- Layout 2 cột: Thông tin cá nhân bên trái, Chức năng chính bên phải
- Sự kiện hiển thị dưới dạng grid
- Bottom navigation có thể thay bằng Sidebar

### 5.2. Tablet
- Layout linh hoạt, có thể chuyển sang 1 cột khi chiều rộng nhỏ
- Sự kiện hiển thị dưới dạng carousel

### 5.3. Mobile
- Layout 1 cột, xếp dọc
- Sự kiện carousel trượt ngang
- Bottom navigation sticky
- Các nút full width hoặc padding 2 bên
