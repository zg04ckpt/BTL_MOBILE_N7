# GIAO DIỆN ĐĂNG KÝ TÀI KHOẢN

## 1. MỤC ĐÍCH

Giao diện Đăng ký cho phép người chơi tạo tài khoản mới để tham gia hệ thống thi đấu.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Logo ứng dụng: Hiển thị ở vị trí trung tâm, phía trên cùng
- Hình nền: Tương đồng với giao diện Đăng nhập

### 2.2. Phần Form Nhập Liệu
- Trường Tên đăng nhập:
  - Nhãn: "Tên đăng nhập"
  - Kiểu nhập: Text
  - Placeholder: "Nhập tên đăng nhập"
  - Bắt buộc nhập
  - Quy tắc: 6-20 ký tự, chỉ chứa chữ cái không dấu, số và gạch dưới
  
- Trường Số điện thoại:
  - Nhãn: "Số điện thoại"
  - Kiểu nhập: Tel
  - Placeholder: "Nhập số điện thoại liên hệ"
  - Bắt buộc nhập
  - Quy tắc: Định dạng số điện thoại hợp lệ
  
- Trường Mật khẩu:
  - Nhãn: "Mật khẩu"
  - Kiểu nhập: Password
  - Placeholder: "Nhập mật khẩu"
  - Bắt buộc nhập
  - Quy tắc: Tối thiểu 6 ký tự
  - Nút hiển thị mật khẩu: Cho phép ẩn hoặc hiện mật khẩu
  
- Trường Xác nhận mật khẩu:
  - Nhãn: "Xác nhận mật khẩu"
  - Kiểu nhập: Password
  - Placeholder: "Nhập lại mật khẩu"
  - Bắt buộc nhập
  - Quy tắc: Phải trùng khớp với mật khẩu đã nhập
  - Nút hiển thị mật khẩu: Cho phép ẩn hoặc hiện mật khẩu

### 2.3. Phần Chức Năng
- Nút Đăng ký:
  - Text: "Đăng ký"
  - Kiểu: Primary button
  - Hiệu ứng: Gợn sóng khi nhấn
  - Trạng thái Loading: Hiển thị khi đang xử lý

### 2.4. Phần Điều Hướng
- Text thông tin: "Bạn đã có tài khoản?"
- Nút hoặc Link Đăng nhập ngay:
  - Text: "Đăng nhập ngay"
  - Kiểu: Secondary button hoặc Link
  - Điều hướng: Chuyển đến Giao diện Đăng nhập

### 2.5. Phần Thông Báo Lỗi
- Vị trí: Hiển thị dưới ô input bị lỗi hoặc Toast thông báo chung
- Kiểu hiển thị: Nổi bật (Error state), dòng text ngắn gọn

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Đăng Ký Thành Công
1. Người dùng điền đầy đủ thông tin:
   - Tên đăng nhập
   - Số điện thoại
   - Mật khẩu
   - Xác nhận mật khẩu
2. Người dùng nhấn nút "Đăng ký"
3. Nút chuyển sang trạng thái Loading, vô hiệu hóa thao tác
4. Hệ thống validate dữ liệu phía Client
5. Gửi request đến Server
6. Server tạo tài khoản thành công:
   - Ẩn Loading
   - Hiển thị thông báo "Đăng ký thành công"
   - Chuyển sang Giao diện Đăng nhập (hoặc tự động đăng nhập luôn tùy logic)

### 3.2. Luồng Chuyển Sang Đăng Nhập
1. Người dùng nhấn nút "Đăng nhập ngay"
2. Hệ thống chuyển cảnh về Giao diện Đăng nhập

### 3.3. Luồng Xử Lý Lỗi
- Lỗi bỏ trống: 
  - Viền đỏ ô input
  - Hiển thị text cảnh báo "Trường này bắt buộc"
  
- Lỗi mật khẩu không khớp:
  - Hiển thị lỗi ở trường "Xác nhận mật khẩu"
  - Text: "Mật khẩu xác nhận không khớp"
  
- Lỗi trùng lặp:
  - Server trả về lỗi
  - Hiển thị thông báo "Tên đăng nhập đã được sử dụng" hoặc "Số điện thoại đã tồn tại"
  
- Lỗi định dạng:
  - Mật khẩu quá ngắn: "Mật khẩu phải có ít nhất 6 ký tự"
  - Số điện thoại sai định dạng: "Số điện thoại không hợp lệ"
  - Tên đăng nhập không hợp lệ: "Tên đăng nhập chỉ chứa chữ cái không dấu, số và gạch dưới (6-20 ký tự)"

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Validation Phía Client
- Tên đăng nhập:
  - Bắt buộc
  - Độ dài: 6-20 ký tự
  - Chỉ chứa: chữ cái không dấu (a-z, A-Z), số (0-9), gạch dưới (_)
  
- Số điện thoại:
  - Bắt buộc
  - Định dạng: Số điện thoại Việt Nam hợp lệ (10-11 số)
  
- Mật khẩu:
  - Bắt buộc
  - Độ dài: Tối thiểu 6 ký tự
  
- Xác nhận mật khẩu:
  - Bắt buộc
  - Phải trùng khớp với Mật khẩu

### 4.2. Validation Phía Server
- Kiểm tra tên đăng nhập không trùng
- Kiểm tra số điện thoại không trùng
- Mã hóa mật khẩu trước khi lưu database

### 4.3. Xử Lý Sau Khi Đăng Ký
- Lưu thông tin người dùng vào database
- Khởi tạo các giá trị mặc định:
  - Level: 1
  - Điểm xếp hạng: 100
  - Kinh nghiệm: 0
- Gửi email xác nhận (nếu có)


