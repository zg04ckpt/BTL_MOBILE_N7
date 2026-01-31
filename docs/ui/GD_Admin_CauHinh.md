# GIAO DIỆN QUẢN LÝ CẤU HÌNH HỆ THỐNG (ADMIN)

## 1. MỤC ĐÍCH

Giao diện cho phép Admin cấp cao điều chỉnh các tham số vận hành của toàn bộ hệ thống mà không cần can thiệp vào code (Server restart).

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Nhóm Cấu Hinh Chung
- Chế độ bảo trì (Maintenance Mode): Toggle Bật/Tắt
  - Khi bật: Người dùng không thể đăng nhập, hiện thông báo bảo trì
  - Whitelist IP: Danh sách IP Admin vẫn truy cập được
- Phiên bản ứng dụng bắt buộc: Quy định version min client để ép update

### 2.2. Nhóm Cấu Hình Game
- Thời gian trả lời mặc định: (Giây)
- Điểm số thắng/thua cơ bản
- Hệ số ELO K-factor
- Số lượng câu hỏi mỗi trận đấu

### 2.3. Nhóm Cấu Hình Quảng Cáo / Monetization (Nếu có)
- Tần suất hiển thị quảng cáo
- Giá trị phần thưởng khi xem video

## 3. LUỒNG THAO TÁC
1. Admin thay đổi giá trị trong các ô input
2. Nhấn "Lưu cấu hình"
3. Hệ thống cache lại config mới và áp dụng ngay lập tức (hoặc sau vài phút tùy cơ chế caching)

## 4. QUY TẮC NGHIỆP VỤ
- Chỉ Super Admin mới có quyền truy cập trang này.
- Các thay đổi cần được log lại (Ai sửa, sửa cái gì, lúc nào).
- Cần có xác nhận 2 bước (Password hoặc OTP) khi thay đổi các cấu hình nhạy cảm như Chế độ bảo trì.
