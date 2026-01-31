# LOGIC NGHIỆP VỤ: NGƯỜI DÙNG

## 1. ĐỊNH NGHĨA VÀ PHÂN LOẠI
- **Người chơi (User):** Tài khoản thường, tham gia thi đấu, xếp hạng.
- **Quản trị viên (Admin/Staff):** Có quyền truy cập trang quản trị.
  - **Super Admin:** Toàn quyền.
  - **Admin:** Quản lý chung, không được xóa/khóa Super Admin.
  - **Moderator:** Duyệt câu hỏi, xử lý báo cáo.
  - **Editor:** Soạn thảo câu hỏi.

## 2. QUY TẮC ĐỊNH DANH
- **Tên đăng nhập (Username):** Unique, 6-20 ký tự, a-z, 0-9, gạch dưới. Không cho phép sửa sau khi tạo.
- **Tên hiển thị (Display Name):** Max 15 ký tự, cho phép ký tự đặc biệt/có dấu. Được phép đổi (có thể giới hạn thời gian đổi hoặc tốn phí).
- **Mật khẩu:** Tối thiểu 6 ký tự, mã hóa (Bcrypt/Argon2).

## 3. TRẠNG THÁI TÀI KHOẢN
- **Active:** Bình thường.
- **Inactive:** Chưa kích hoạt (nếu có confirm email).
- **Banned:** Bị khóa. Khi đăng nhập sẽ nhận thông báo lỗi kèm lý do và thời hạn.
- **Deleted (Soft):** Đã xóa. Thông tin cá nhân bị ẩn/anonymize, nhưng lịch sử đấu giữ lại.

## 4. CẤP ĐỘ VÀ KINH NGHIỆM (EXP)
- **Công thức lên cấp:** `EXP_Next_Level = 100 * (Current_Level * 1.2)` (Dự kiến).
- **Nguồn EXP:**
  - Thắng trận: 100% EXP cơ bản + Bonus.
  - Thua trận: 20% EXP cơ bản.
  - Hoàn thành nhiệm vụ/Sự kiện.

## 5. BẢO MẬT & PHIÊN
- **Token:** Sử dụng JWT. Access Token (ngắn hạn) + Refresh Token (dài hạn).
- **Đa thiết bị:** Cho phép/Không cho phép đăng nhập cùng lúc (Cấu hình hệ thống). Mặc định: Login nới mới sẽ kick nơi cũ.
