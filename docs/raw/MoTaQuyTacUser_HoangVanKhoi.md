# Mô tả: Quy tắc liên quan đến user

## 1. Định nghĩa và Phân loại User
* **Người chơi:**
    * Đã đăng ký với Tên đăng nhập/Mật khẩu.
    * Dữ liệu đồng bộ trên Server.
    * Được tham gia đầy đủ tính năng: Xếp hạng, Kết bạn, Lịch sử đấu.
* **Admin:**
    * Có quyền truy cập trang quản trị.
    * Quyền hạn: Quản lý câu hỏi, khóa tài khoản người chơi, xem thống kê hệ thống.

## 2. Quy tắc về Thông tin định danh
* **Tên đăng nhập:**
    * Độ dài: Từ 6 đến 20 ký tự.
    * Chỉ chứa chữ cái không dấu, số và dấu gạch dưới (_).
    * Không trùng với người khác.
* **Tên hiển thị:**
    * Độ dài: Tối đa 15 ký tự.
    * Cho phép ký tự có dấu, khoảng trắng.
* **Mật khẩu:**
    * Tối thiểu 6 ký tự.
    * Mã hóa 1 chiều trước khi lưu vào Database.

## 3. Quy tắc về Trạng thái
Hệ thống cần ghi nhận các trạng thái của User để phục vụ tính năng Ghép trận và Bạn bè:
* **Offline:** Người dùng không mở ứng dụng hoặc mất kết nối quá 30s.
* **Online (Sẵn sàng):** Đang mở ứng dụng và ở sảnh chờ/menu chính.
* **In-game (Đang chơi):** Đang trong một trận đấu.
* **Banned (Bị khóa):**
    * Tài khoản vi phạm quy tắc (gian lận).
    * Khi đăng nhập sẽ nhận thông báo lỗi kèm thời gian mở khóa.

## 4. Cơ chế Cấp độ và Kinh nghiệm
* **Điểm kinh nghiệm (EXP):**
    * Người chơi nhận được EXP sau mỗi trận đấu (dù thắng hay thua).
    * Thắng: Nhận 100% EXP cơ bản + Bonus.
    * Thua: Nhận 20% EXP cơ bản.
* **Công thức lên cấp (Dự kiến):**
    * Level 1 -> 2: Cần 100 EXP.
    * Level n -> n+1: Cần `100 * (Level hiện tại * 1.2)` EXP.
* **Quyền lợi theo cấp:**
    * Level càng cao, khung Avatar càng đẹp (thay đổi theo mốc Level 10, 20, 50).

## 5. Quy tắc về Tài sản (Nếu có)
* **Điểm Xếp hạng (Elo/Rank Point):**
    * Khởi điểm: 100 điểm (Mức Đồng).
    * Thắng cộng điểm, thua trừ điểm.
    * Không âm điểm (Tối thiểu là 0).