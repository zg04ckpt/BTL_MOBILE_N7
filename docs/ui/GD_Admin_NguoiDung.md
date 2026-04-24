# GIAO DIỆN QUẢN LÝ NGƯỜI DÙNG (ADMIN)

## 1. MỤC ĐÍCH

Giao diện cho phép quản trị viên tìm kiếm, phân trang, thêm mới, cập nhật và xóa tài khoản người dùng.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header & Bộ Lọc
- Ô tìm kiếm theo `Tên` và `Email`
- Nút `Tìm kiếm`, `Đặt lại`
- Nút `Thêm tài khoản user`

### 2.2. Danh Sách Người Dùng (Data Table)
- Cột hiển thị: `ID`, `Tên hiển thị`, `Email`, `Trạng thái`, `Quyền`, `Hành động`
- Avatar hiển thị trong cột tên
- Trạng thái được tô màu theo từng giá trị (`Active/Inactive/Banned/Deleted`)
- Có phân trang và đổi kích thước trang

### 2.3. Hành Động (Cột Action)
- `Sửa`: mở modal và tải chi tiết user theo ID
- `Xóa`: popup xác nhận trước khi gọi API xóa

### 2.4. Modal Thêm / Cập Nhật User
- Chế độ thêm mới:
  - Tên, email, số điện thoại, mật khẩu, quyền (`roleId`)
- Chế độ cập nhật:
  - Tên, số điện thoại, quyền, trạng thái, level/rank/rankScore/exp
  - Cho phép thay ảnh đại diện (upload ảnh và preview)
  - Email bị khóa sửa trong mode edit

## 3. LUỒNG THAO TÁC

### 3.1. Tìm kiếm danh sách
1. Nhập tên/email.
2. Nhấn `Tìm kiếm`.
3. Hệ thống gọi API theo điều kiện + phân trang.

### 3.2. Thêm mới user
1. Nhấn `Thêm tài khoản user`.
2. Nhập các trường bắt buộc.
3. Nhấn lưu để tạo tài khoản mới.

### 3.3. Cập nhật user
1. Nhấn `Sửa` tại dòng user.
2. Hệ thống tải chi tiết user.
3. Chỉnh sửa thông tin và lưu.

### 3.4. Xóa user
1. Nhấn `Xóa`.
2. Xác nhận ở popup.
3. Gọi API xóa và tải lại danh sách.

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Validation UI đang có
- Bắt buộc các trường cơ bản: tên, email, số điện thoại, quyền
- Khi thêm mới bắt buộc có mật khẩu
- Có xác nhận trước khi xóa

### 4.2. Phạm vi chưa có trên UI hiện tại
- Chưa có flow reset mật khẩu
- Chưa có trang/modal riêng xem lịch sử hoạt động chi tiết
- Chưa có workflow khóa user theo lý do/thời hạn riêng

## 5. RESPONSIVE
- Giao diện hiện tại tập trung cho desktop với bảng dữ liệu và modal lớn.
