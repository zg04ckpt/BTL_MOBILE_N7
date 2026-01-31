# GIAO DIỆN THÔNG TIN CÁ NHÂN

## 1. MỤC ĐÍCH

Hiển thị thông tin cá nhân của người chơi, thống kê thành tích, lịch sử đấu và cung cấp các chức năng quản lý tài khoản.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header (Tiêu Đề)
- Tiêu đề: "Hồ sơ" hoặc "Cá nhân".
- Nút Cài đặt (Icon bánh răng): Góc phải, mở menu cài đặt ứng dụng.

### 2.2. Khu Vực Thông Tin Chính (Top Section)
- Avatar: Hình tròn lớn, có thể kèm khung viền theo cấp độ.
- Tên hiển thị: Font to, đậm.
- Level: Ví dụ "Lv.10" (hiển thị dưới tên).
- Thanh kinh nghiệm (EXP Bar): Hiển thị tiến độ lên cấp tiếp theo (Ví dụ: 350/1000 EXP).

### 2.3. Khu Vực Thống Kê (Stats)
- Chia làm 3 hoặc 4 ô thông tin:
  - Điểm xếp hạng (Point): Tổng RP hiện tại.
  - Số trận đã đấu: Tổng số trận.
  - Tỉ lệ thắng (%): (Số trận thắng / Tổng số trận) * 100.
  - Chuỗi thắng (Streak): Số trận thắng liên tiếp hiện tại.

### 2.4. Khu Vực Lịch Sử Đấu (History)
- Tiêu đề: "Trận đấu gần đây".
- Danh sách (List item), mỗi dòng là 1 trận:
  - Kết quả: Thắng (Xanh) / Thua (Đỏ).
  - Đối thủ: Avatar và Tên đối thủ.
  - Thời gian: "2 phút trước", "Hôm qua".
  - Điểm cộng/trừ: "+25 RP", "-15 RP".
- Giới hạn: Hiển thị 10-20 trận gần nhất.

### 2.5. Khu Vực Chức Năng (Menu)
- Nút "Chỉnh sửa hồ sơ":
  - Icon: Bút chì.
  - Hành động: Mở popup đổi tên, avatar.
- Nút "Đăng xuất":
  - Kiểu hiển thị: Nổi bật (Danger style) hoặc Vô hiệu hóa.
  - Vị trí: Dưới cùng danh sách.

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Xem Thông Tin
1. Người dùng truy cập tab "Cá nhân".
2. Hệ thống hiển thị Skeleton Loading.
3. Tải xong: Hiển thị đầy đủ Avatar, Tên, Thống kê, Lịch sử.

### 3.2. Luồng Chỉnh Sửa Hồ Sơ
1. Người dùng nhấn "Chỉnh sửa hồ sơ".
2. Hiển thị Popup/Bottom Sheet chỉnh sửa.
3. Người dùng chọn Avatar mới từ thư viện hoặc nhập Tên hiển thị mới.
4. Nhấn "Lưu thay đổi".
   - Thành công: Popup đóng, thông tin cập nhật ngay.
   - Thất bại: Báo lỗi (Ví dụ: Tên vi phạm quy tắc).

### 3.3. Luồng Đăng Xuất
1. Người dùng nhấn nút "Đăng xuất".
2. Hiển thị Dialog xác nhận: "Bạn có chắc chắn muốn đăng xuất?".
3. Nhấn "Đồng ý": Xóa token, chuyển về Giao diện Đăng nhập.
4. Nhấn "Hủy": Đóng dialog.

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Dữ Liệu
- Dữ liệu thống kê phải được tính toán chính xác từ server.
- Lịch sử đấu sắp xếp theo thời gian từ mới nhất đến cũ nhất.

### 4.2. Quyền Hạn
- Người dùng chỉ sửa được Tên hiển thị (Display Name), không sửa được Tên đăng nhập (Username).
- Avatar có thể chọn từ danh sách có sẵn hoặc upload (tùy cấu hình).

## 5. RESPONSIVE

### 5.1. Desktop
- Layout 2 cột: Cột trái là Thông tin chính + Menu, Cột phải là Thống kê + Lịch sử đấu.

### 5.2. Mobile
- Layout 1 cột dọc cuộn.
