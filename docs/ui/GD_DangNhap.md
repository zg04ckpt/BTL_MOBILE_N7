# GIAO DIỆN ĐĂNG NHẬP

## 1. MỤC ĐÍCH

Giao diện Đăng nhập cho phép người chơi xác thực danh tính để truy cập vào hệ thống thi đấu, bảng xếp hạng và lịch sử đấu.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Logo ứng dụng: Hiển thị ở vị trí trung tâm, phía trên cùng
- Hình nền: Mang tính chất trí tuệ, đối kháng

### 2.2. Phần Form Nhập Liệu
- Trường Tên đăng nhập:
  - Nhãn: "Tên đăng nhập"
  - Kiểu nhập: Text
  - Placeholder: "Nhập tên đăng nhập hoặc email"
  - Bắt buộc nhập
  
- Trường Mật khẩu:
  - Nhãn: "Mật khẩu"
  - Kiểu nhập: Password (che ký tự)
  - Placeholder: "Nhập mật khẩu"
  - Nút hiển thị mật khẩu: Cho phép ẩn hoặc hiện mật khẩu
  - Bắt buộc nhập

### 2.3. Phần Chức Năng
- Nút Đăng nhập:
  - Text: "Đăng nhập"
  - Kiểu: Primary button
  - Hiệu ứng: Gợn sóng khi nhấn
  - Trạng thái Loading: Hiển thị khi đang xử lý
  
- Liên kết Quên mật khẩu:
  - Text: "Quên mật khẩu?"
  - Vị trí: Căn phải, dưới trường mật khẩu
  - Kiểu hiển thị: Text phụ (Secondary text)

### 2.4. Phần Điều Hướng
- Text thông tin: "Bạn chưa có tài khoản?"
- Nút Đăng ký ngay:
  - Text: "Đăng ký ngay"
  - Kiểu: Secondary button hoặc Link
  - Điều hướng: Chuyển đến Giao diện Đăng ký

### 2.5. Phần Thông Báo Lỗi
- Vị trí: Hiển thị dưới ô input bị lỗi hoặc Toast thông báo chung
- Kiểu hiển thị: Nổi bật (Error state), dòng text ngắn gọn hoặc Popup

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Đăng Nhập Thành Công
1. Người dùng mở ứng dụng
2. Hệ thống kiểm tra Token:
   - Nếu còn hạn: Chuyển đến Giao diện Trang chủ
   - Nếu hết hạn: Hiển thị Giao diện Đăng nhập
3. Người dùng nhập Tên đăng nhập và Mật khẩu
4. Người dùng nhấn nút "Đăng nhập"
5. Nút chuyển sang trạng thái Loading
6. Hệ thống xác thực thông tin:
   - Thành công: Lưu token, chuyển đến Giao diện Trang chủ
   - Thất bại: Hiển thị thông báo lỗi

### 3.2. Luồng Chuyển Sang Đăng Ký
1. Người dùng nhấn nút "Đăng ký ngay"
2. Hệ thống chuyển cảnh sang Giao diện Đăng ký

### 3.3. Luồng Quên Mật Khẩu
1. Người dùng nhấn liên kết "Quên mật khẩu?"
2. Hệ thống chuyển cảnh sang Giao diện Quên mật khẩu

### 3.4. Luồng Xử Lý Lỗi
- Lỗi bỏ trống: Hiển thị viền đỏ ô input và text cảnh báo "Trường này bắt buộc"
- Lỗi sai thông tin: Hiển thị thông báo "Tên đăng nhập hoặc mật khẩu không chính xác"
- Lỗi mạng: Hiển thị thông báo "Vui lòng kiểm tra lại kết nối mạng"
- Lỗi tài khoản bị khóa: Hiển thị thông báo "Tài khoản bị khóa. Liên hệ quản trị viên"

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Validation
- Tên đăng nhập: Không được để trống, tối thiểu 2 ký tự
- Mật khẩu: Không được để trống, tối thiểu 6 ký tự

### 4.2. Bảo Mật
- Mật khẩu mặc định che ký tự
- Token lưu trữ an toàn, có thời hạn hết hạn
- Không lưu mật khẩu dưới dạng plain text

### 4.3. Xử Lý Trạng Thái
- Loading: Disable tất cả các nút và input khi đang xử lý
- Timeout: Hiển thị lỗi nếu request quá 30 giây

## 5. RESPONSIVE

### 5.1. Desktop
- Form ở giữa màn hình
- Chiều rộng tối đa: 400-500px
- Logo lớn, rõ ràng

### 5.2. Mobile
- Form full width với padding 2 bên
- Nút full width
- Logo thu nhỏ
- Keyboard tự động hiển thị khi focus vào input
