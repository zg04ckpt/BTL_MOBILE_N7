# GIAO DIỆN SỰ KIỆN (EVENTS)

## 1. MỤC ĐÍCH

Tạo nội dung tươi mới, giới hạn thời gian để tăng tương tác người dùng, cung cấp các chế độ chơi đặc biệt và phần thưởng hấp dẫn.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Banner Sự Kiện (Home Banner List)
- Vị trí: Trên Trang chủ.
- Kiểu hiển thị: Slide trượt ngang các sự kiện đang Active.
- Nội dung: Hình ảnh đồ họa bắt mắt, Tên sự kiện.

### 2.2. Popup Chi Tiết Sự Kiện
- Banner & Tiêu đề: Hình ảnh chủ đạo và tên sự kiện.
- Mô tả & Luật chơi:
  - Text mô tả nội dung sự kiện.
  - Luật chơi đặc biệt (nếu có).
- Đồng hồ đếm ngược: Thời gian còn lại của sự kiện.
- Danh sách phần thưởng (Reward List): Các mốc quà đạt được.
- Nút "Tham gia ngay":
  - Hiển thị số vé/lượt còn lại (Ví dụ: "3/3 lượt miễn phí").
  - Kêu gọi hành động chính.

### 2.3. Thanh Tiến Trình Sự Kiện (In-game Progress)
- Vị trí: Trong popup sự kiện hoặc giao diện riêng của sự kiện.
- Nội dung: Thanh progress bar hiển thị điểm tích lũy và các mốc quà (Milestones) có thể nhận.
- Trạng thái:
  - Đã nhận: Sáng lên, có dấu check.
  - Có thể nhận: Sáng lên, có hiệu ứng nhắc nhở.
  - Chưa đạt: Mờ đi.

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Xem Và Tham Gia
1. Người dùng nhấn vào Banner sự kiện ở Trang chủ.
2. Hệ thống hiển thị Popup chi tiết sự kiện.
3. Người dùng xem thông tin và nhấn "Tham gia ngay".
4. Hệ thống kiểm tra điều kiện (Vé/Lượt chơi):
   - Nếu đủ: Trừ vé -> Chuyển sang màn hình thi đấu (Bộ câu hỏi riêng của Event).
   - Nếu thiếu: Hiển thị gợi ý mua thêm vé hoặc xem quảng cáo (nếu có).

### 3.2. Luồng Nhận Thưởng
1. Sau khi kết thúc game sự kiện, hệ thống tổng kết điểm event.
2. Cập nhật thanh tiến trình sự kiện.
3. Nếu đạt mốc quà: Hiển thị Popup "Nhận quà" ngay lập tức.

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Tính Chất Sự Kiện
- Giới hạn thời gian (Limited-time).
- Không tính vào Rank Point chính (tránh lạm phát ELO).
- Sử dụng hệ thống phần thưởng riêng (Currency, Khung avatar, Huy hiệu).

### 4.2. Cơ Chế Thưởng
- Milestone: Đạt mốc 5 câu/10 câu -> Nhận quà ngay.
- Event Leaderboard: Top 10 người cao điểm nhất sự kiện nhận Huy hiệu độc quyền sau khi sự kiện kết thúc.

## 5. RESPONSIVE
- Popup hiển thị tối ưu trên cả mobile (dạng Fullscreen dialog) và desktop (dạng Modal center).
