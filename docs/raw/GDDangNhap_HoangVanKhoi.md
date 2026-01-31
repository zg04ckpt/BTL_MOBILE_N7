# Mô tả chức năng: Đăng nhập

## 1. Giới hạn phạm vi chức năng
* **Mục đích:** Xác thực danh tính người chơi để truy cập vào hệ thống thi đấu, bảng xếp hạng và lịch sử đấu.
* **Chức năng bao gồm:**
    * Đăng nhập bằng Tên đăng nhập và Mật khẩu.
    * Kiểm tra dữ liệu đầu vào phía Client.
    * Xử lý điều hướng sang Đăng ký hoặc Quên mật khẩu.
* **Chức năng KHÔNG bao gồm:**
    * Đăng nhập bằng sinh trắc học (Vân tay/FaceID)

## 2. Các thành phần UI
* **Background:**
    * Hình nền mang tính chất trí tuệ/đối kháng.
    * Có thể sử dụng hiệu ứng mờ để làm nổi bật trang đăng nhập.
* **Logo Game:** Đặt vị trí trung tâm, phía trên cùng (khoảng 1/4 màn hình).
* **Form nhập liệu:**
    * **Input Username:**
        * Placeholder: "Tên đăng nhập / Email".
        * Icon trái: User icon.
    * **Input Password:**
        * Placeholder: "Mật khẩu".
        * Icon trái: Lock icon.
        * Icon phải: Eye icon (để ẩn/hiện mật khẩu).
        * Input type: Password (che ký tự).
* **Nút chức năng:**
    * **Nút Đăng nhập** Màu chủ đạo của app (Ví dụ: Xanh dương hoặc Cam), bo tròn góc. Hiệu ứng gợn sóng/hover khi nhấn.
    * **Link "Quên mật khẩu?":** Căn phải, chữ nhỏ, màu phụ.
* **Khu vực điều hướng:**
    * Text: "Bạn chưa có tài khoản?"
    * Button/Link "Đăng ký ngay": Màu nổi bật, chuyển hướng sang màn hình Đăng ký.
* **Thông báo lỗi:**
    * Dòng text đỏ hiển thị ngay dưới ô input bị lỗi hoặc hiển thị Toast/Popup thông báo chung.

## 3. Luồng hoạt động
### 3.1. Luồng Đăng nhập thành công
1.  **Khởi động:** Người dùng mở ứng dụng.
    * *Hệ thống:* Kiểm tra Token. Nếu còn hạn -> Vào **GĐ Trang chủ**. Nếu không -> Vào **GĐ Đăng nhập**.
2.  **Nhập liệu:** Người dùng nhập **Tên đăng nhập** và **Mật khẩu**.
3.  **Hành động:** Nhấn nút **"Đăng nhập"**.
4.  **Phản hồi UI:** Nút chuyển trạng thái **Loading**, vô hiệu hóa các nút khác.
5.  **Kết quả:** Server xác thực thành công -> Ẩn Loading, chuyển sang **GĐ Trang chủ**.

### 3.2. Luồng Chuyển sang Đăng ký
1.  **Hành động:** Người dùng nhấn nút **"Đăng ký ngay"**.
2.  **Phản hồi UI:** Hệ thống chuyển cảnh sang màn hình **Giao diện đăng ký**.

### 3.3. Luồng Quên mật khẩu
1.  **Hành động:** Người dùng nhấn liên kết **"Quên mật khẩu?"**.
2.  **Phản hồi UI:** Hệ thống chuyển cảnh sang màn hình **Giao diện quên mật khẩu**.
### 3.4. Luồng Xử lý lỗi
* **Lỗi bỏ trống:** Nhấn "Đăng nhập" khi thiếu thông tin -> Viền đỏ ô input, hiện text cảnh báo.
* **Lỗi sai thông tin:** Server báo sai User/Pass -> Tắt Loading, hiện thông báo *"Tên đăng nhập hoặc mật khẩu không chính xác"*.
* **Lỗi mạng:** Mất kết nối -> Hiện thông báo *"Vui lòng kiểm tra lại kết nối mạng"*.