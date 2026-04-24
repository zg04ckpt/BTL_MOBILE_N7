# Mô tả: Giao diện Thông tin cá nhân

## 1. Giới hạn phạm vi chức năng
* **Mục đích:** Hiển thị thông tin cá nhân của người chơi và chức năng quản lý tài khoản.
* **Chức năng bao gồm:**
    * Xem thông tin cơ bản: Avatar, Tên hiển thị, Cấp độ.
    * Xem thống kê thành tích: Tổng số trận, Tỉ lệ thắng, Điểm xếp hạng.
    * Chức năng: Đăng xuất, Chỉnh sửa thông tin, Đổi mật khẩu.
* **Chức năng KHÔNG bao gồm:**
    * Xem chi tiết từng câu hỏi trong lịch sử đấu.

## 2. Các thành phần UI
* **Header:**
    * Tiêu đề "Hồ sơ" ở giữa.
* **Khu vực Thông tin chính (Top Section):**
    * **Avatar:** Hình tròn lớn.
    * **Tên hiển thị:** Font to, đậm.
    * **Level:** Ví dụ "Lv.10" (hiển thị dưới tên).
    * **Thanh kinh nghiệm:** Hiển thị tiến độ lên cấp tiếp theo.
* **Khu vực Thống kê:** Chia làm 3 hoặc 4 ô:
    * Điểm xếp hạng (Point).
    * Số trận đã đấu.
    * Tỉ lệ thắng (%).
* **Khu vực Chức năng:**
    * Nút **"Chỉnh sửa hồ sơ"**: Cho phép đổi tên, avatar.
    * Nút **"Đăng xuất"**: Màu đỏ hoặc xám, nằm dưới cùng.

## 3. Luồng hoạt động (UI Flow)
### 3.1. Luồng Hiển thị thông tin
1.  **Vào màn hình:** Người dùng truy cập tab/màn hình "Cá nhân".
2.  **Trạng thái chờ:** Giao diện hiển thị hiệu ứng khung xương (Skeleton Loading) tại các vị trí: Avatar, Tên, Chỉ số thống kê.
3.  **Hiển thị dữ liệu:**
    * Sau khi tải xong -> Các khung xương biến mất, hiển thị đầy đủ thông tin thực tế.
    * Nếu lỗi mạng -> Hiển thị thông báo "Không thể tải dữ liệu" kèm nút "Thử lại" ở giữa màn hình.

### 3.2. Luồng Chỉnh sửa thông tin (Trực tiếp)
1.  **Mở chỉnh sửa:** Người dùng nhấn nút **"Sửa hồ sơ"**.
2.  **Hiển thị Popup:**
    * Một **Popup (Dialog)** hoặc **Bottom Sheet** trượt lên/hiện ra đè lên màn hình hiện tại.
    * Nền phía sau tối đi (Dimmed background).
3.  **Thao tác nhập liệu:**
    * Người dùng nhấn vào Avatar -> Mở thư viện ảnh để chọn ảnh mới.
    * Người dùng nhập Tên mới hoặc chọn lại Ngày sinh.
4.  **Lưu thay đổi:**
    * Nhấn nút **"Lưu thay đổi"** -> Nút chuyển sang trạng thái Loading (xoay vòng).
    * **Thành công:** Popup tự đóng lại. Ảnh đại diện và Tên trên màn hình chính thay đổi ngay lập tức sang dữ liệu mới.
    * **Thất bại:** Popup giữ nguyên, hiển thị dòng text báo lỗi màu đỏ (ví dụ: "Tên không hợp lệ").

### 3.3. Luồng Đăng xuất
1.  **Thao tác:** Người dùng nhấn nút **"Đăng xuất"**.
2.  **Xác nhận:** Hiển thị hộp thoại (Dialog) ở giữa màn hình: *"Bạn có chắc chắn muốn đăng xuất?"* với 2 nút: **Hủy** và **Đồng ý**.
3.  **Điều hướng:**
    * Nhấn **Hủy** -> Đóng hộp thoại, ở lại màn hình hiện tại.
    * Nhấn **Đồng ý** -> Chuyển cảnh ngay lập tức về màn hình **Giao diện đăng nhập**.