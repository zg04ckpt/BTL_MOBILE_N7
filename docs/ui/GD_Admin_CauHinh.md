# GIAO DIỆN QUẢN LÝ CẤU HÌNH HỆ THỐNG (ADMIN)

## 1. MỤC ĐÍCH

Trang cho phép quản trị viên cập nhật nhanh các tham số vận hành hệ thống và trận đấu từ UI admin.

## 2. CÁC THÀNH PHẦN GIAO DIỆN (THEO CODE HIỆN TẠI)

### 2.1. Header trang
- Tiêu đề: `Quản lý cấu hình`
- Nút hành động: `Lưu thay đổi`

### 2.2. Nhóm `Cấu hình chung`
- `Maintenance mode` (switch bật/tắt)
- `Whitelist IP` (input chuỗi, nhập theo dạng phân tách dấu phẩy)
- `Thời gian sống phiên đăng nhập (phút)` (`loginLiveTime`)
- `Cập nhật lần cuối` (readonly)

### 2.3. Nhóm `Cấu hình trận đấu`
- `Thời gian trả lời (giây)` (`questionTimeLimit`)
- `Số câu hỏi mỗi trận` (`questionsPerMatch`)
- `Điểm thắng cơ bản` (`baseWinPoints`)
- `Điểm thua cơ bản` (`baseLosePoints`)
- `Hệ số ELO K-factor` (`eloKFactor`)

## 3. LUỒNG THAO TÁC
1. Khi vào trang, hệ thống gọi API lấy toàn bộ cấu hình hiện tại.
2. Admin chỉnh sửa giá trị tại 2 nhóm cấu hình.
3. Nhấn `Lưu thay đổi` để gửi payload cập nhật.
4. Hiển thị thông báo thành công/thất bại theo phản hồi API.

## 4. QUY TẮC/PHẠM VI ĐANG ÁP DỤNG
- Chưa có luồng xác nhận 2 bước (OTP/password) trong UI hiện tại.
- Chưa có nút khôi phục mặc định ở giao diện (đã để comment trong code).
- Tài liệu này phản ánh đúng phạm vi đang implement, không mô tả tính năng chưa có.
