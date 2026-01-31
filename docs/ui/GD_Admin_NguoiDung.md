# GIAO DIỆN QUẢN LÝ NGƯỜI DÙNG (ADMIN)

## 1. MỤC ĐÍCH

Giao diện Quản lý người dùng cho phép Quản trị viên theo dõi, tìm kiếm và thực hiện các tác vụ quản lý tài khoản người chơi trên hệ thống.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header & Bộ Lọc
- Tiêu đề trang: "Quản lý người dùng"
- Ô tìm kiếm: Tìm theo Tên đăng nhập, Email, Số điện thoại
- Bộ lọc trạng thái: Tất cả, Đang hoạt động, Bị khóa
- Bộ lọc thời gian: Ngày đăng ký (Từ ngày - Đến ngày)
- Nút "Tìm kiếm" và "Đặt lại"

### 2.2. Danh Sách Người Dùng (Data Table)
- Các cột thông tin:
  - ID: Mã định danh
  - Avatar & Tên đăng nhập
  - Email / Số điện thoại
  - Cấp độ (Level)
  - Ngày đăng ký
  - Trạng thái (Active/Banned)
  - Hành động
- Phân trang: Số bản ghi/trang, Chuyển trang

### 2.3. Các Hành Động (Cột Action)
- Xem chi tiết: Icon con mắt
- Chỉnh sửa: Icon bút chì (Chỉ sửa thông tin cơ bản)
- Khóa/Mở khóa: Icon ổ khóa
- Reset mật khẩu: Icon chìa khóa / mail

### 2.4. Modal/Trang Chi Tiết Người Dùng
- Thông tin hồ sơ: Avatar, Tên, Email, SĐT, Ngày sinh
- Thống kê: Số trận đấu, Tỉ lệ thắng, Điểm xếp hạng
- Lịch sử hoạt động: Log đăng nhập, Log thi đấu gần nhất
- Các nút tác vụ quản trị:
  - Khóa tài khoản (Kèm lý do)
  - Xóa tài khoản (Yêu cầu xác nhận cao)

## 3. LUỒNG THAO TÁC

### 3.1. Tìm Kiếm Người Dùng
1. Admin nhập từ khóa vào ô tìm kiếm
2. Nhấn "Tìm kiếm"
3. Bảng dữ liệu cập nhật kết quả tương ứng

### 3.2. Khóa Tài Khoản
1. Admin nhấn log khóa trên dòng người dùng tương ứng
2. Hiển thị Popup "Xác nhận khóa tài khoản":
   - Nhập lý do khóa (Vi phạm quy tắc, gian lận, spam...)
   - Chọn thời hạn (7 ngày, 30 ngày, vĩnh viễn)
3. Nhấn "Xác nhận"
4. Cập nhật trạng thái người dùng sang "Bị khóa"

### 3.3. Xem Chi Tiết
1. Admin nhấn icon Xem chi tiết
2. Chuyển sang màn hình Chi tiết hoặc mở Modal lớn
3. Hiển thị toàn bộ thông tin và lịch sử của user đó

### 3.4. Reset Mật Khẩu
1. Admin chọn chức năng Reset mật khẩu
2. Hệ thống tạo một mật khẩu ngẫu nhiên hoặc gửi link reset về email người dùng
3. Hiển thị thông báo "Đã gửi yêu cầu reset mật khẩu thành công"

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Quyền Hạn
- Chỉ Super Admin hoặc Admin được phân quyền mới thấy trang này
- Không thể khóa hoặc xóa tài khoản của Admin khác (trừ Super Admin)

### 4.2. Hiển Thị
- Ẩn mật khẩu, chỉ hiển thị hash hoặc dấu *
- Sắp xếp mặc định theo Ngày đăng ký mới nhất

### 4.3. Validation
- Lý do khóa là bắt buộc khi thực hiện khóa tài khoản
- Không thể xóa tài khoản nếu người dùng đã có giao dịch nạp tiền (thay vào đó là Vô hiệu hóa)

## 5. RESPONSIVE

### 5.1. Desktop
- Bảng dữ liệu full các cột
- Các bộ lọc nằm hàng ngang phía trên

### 5.2. Mobile
- Ẩn bớt các cột ít quan trọng (ID, Ngày đăng ký)
- Sử dụng Cards thay vì Table nếu cần thiết
- Nút hành động gom vào menu "..."
