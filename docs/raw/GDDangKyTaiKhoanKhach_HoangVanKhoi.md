# Mô tả chức năng: Đăng ký tài khoản

## 1. Giới hạn phạm vi chức năng
* **Mục đích:** Tạo tài khoản mới để người chơi tham gia hệ thống.
* **Chức năng bao gồm:**
    * Đăng ký bằng Tên đăng nhập, Số điện thoại và Mật khẩu.
    * Xác nhận lại mật khẩu để tránh sai sót.
    * Kiểm tra dữ liệu đầu vào phía Client.
* **Chức năng KHÔNG bao gồm:**
    * Xác thực OTP qua Email/SMS (Phase 2).

## 2. Các thành phần UI
* **Background & Logo:** Tương đồng với giao diện Đăng nhập.
* **Form nhập liệu:**
    * **Input Username:**
        * Placeholder: "Tên đăng nhập".
        * Icon trái: User icon.
    * **Input Số điện thoại:**
        * Placeholder: "Số điện thoại liên hệ".
        * Icon trái: Phone icon.
    * **Input Password:**
        * Placeholder: "Mật khẩu".
        * Icon trái: Lock icon.
        * Icon phải: Eye icon.
    * **Input Confirm Password:**
        * Placeholder: "Nhập lại mật khẩu".
        * Icon trái: Lock icon.
        * Icon phải: Eye icon.
* **Nút chức năng:**
    * **Nút Đăng ký:** Màu chủ đạo, bo tròn, hiệu ứng gợn sóng.
* **Khu vực điều hướng:**
    * Text: "Bạn đã có tài khoản?"
    * Button/Link "Đăng nhập ngay": Chuyển hướng về màn hình Đăng nhập.
* **Thông báo lỗi:**
    * Hiển thị text đỏ dưới ô input hoặc Toast thông báo chung.

## 3. Luồng hoạt động

### 3.1. Luồng Đăng ký thành công
1.  **Nhập liệu:** Người dùng điền đầy đủ: Tên đăng nhập, Số điện thoại, Mật khẩu, Nhập lại mật khẩu.
2.  **Hành động:** Nhấn nút **"Đăng ký"**.
3.  **Phản hồi UI:** Nút chuyển trạng thái **Loading**, vô hiệu hóa thao tác.
4.  **Kết quả:** Server tạo tài khoản thành công -> Ẩn Loading, hiển thị thông báo *"Đăng ký thành công"*.
5.  **Điều hướng:** Tự động chuyển sang **Giao diện Đăng nhập** (hoặc tự động đăng nhập luôn tùy logic).

### 3.2. Luồng Chuyển sang Đăng nhập
1.  **Hành động:** Người dùng nhấn nút **"Đăng nhập ngay"**.
2.  **Phản hồi UI:** Hệ thống chuyển cảnh về **Giao diện Đăng nhập**.

### 3.3. Luồng Xử lý lỗi
* **Lỗi bỏ trống:** Nhấn "Đăng ký" khi thiếu thông tin -> Viền đỏ ô input, hiện text cảnh báo.
* **Lỗi mật khẩu không khớp:** Ô "Nhập lại mật khẩu" khác ô "Mật khẩu" -> Báo lỗi *"Mật khẩu xác nhận không khớp"*.
* **Lỗi trùng lặp:** Server báo Username/Số điện thoại đã tồn tại -> Hiện thông báo *"Tên đăng nhập đã được sử dụng"*.
* **Lỗi định dạng:** Mật khẩu quá ngắn hoặc Số điện thoại sai định dạng -> Báo lỗi cụ thể dưới ô input.