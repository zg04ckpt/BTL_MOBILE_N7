# GIAO DIỆN TRANG CHỦ

## 1. MỤC ĐÍCH

Giao diện Trang chủ là điểm truy cập chính của người chơi sau khi đăng nhập, cung cấp các chức năng chính và thông tin tổng quan về tài khoản.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header (Card Thông Tin Người Dùng)
- Avatar người dùng: Hiển thị ở góc trái trên cùng (hình tròn, có viền).
- Tên hiển thị: Bên phải avatar, font đậm.
- Level hiện tại: Hiển thị bên dưới tên (VD: "Lv. 3").
- Thanh kinh nghiệm (EXP Bar):
  - Hai nhãn cấp độ ở hai đầu (VD: "Lv. 3" → "Lv. 4").
  - Phần trăm tiến độ hiển thị ở giữa thanh.

### 2.2. Phần Thông Tin Nhanh (4 Stat Cards)
- Điểm xếp hạng hiện tại:
  - Icon: Trophy vàng
  - Nhãn: "Điểm xếp hạng"
  - Giá trị: Số điểm RP

- Thứ hạng hiện tại:
  - Icon: Medal tím
  - Nhãn: "Thứ hạng"
  - Giá trị: Vị trí (#N)

- Số trận đã đấu:
  - Icon: Crossed swords xanh dương
  - Nhãn: "Số trận"
  - Giá trị: Tổng số trận đã chơi

- Tỷ lệ thắng:
  - Icon: Mũi tên xu hướng xanh lá
  - Nhãn: "Tỷ lệ thắng"
  - Giá trị: Phần trăm thắng (2 chữ số thập phân, VD: "65.32%")

### 2.3. Nút Bắt Đầu Thi Đấu
- Card lớn nổi bật màu đỏ/cam (color5).
- Icon crossed swords + text "BẮT ĐẦU THI ĐẤU" + text phụ "Tìm đối thủ ngay bây giờ".
- Hành động: Chuyển đến Giao diện Cấu hình thi đấu (MatchConfigActivity).

### 2.4. Phần Sự Kiện (Banner List)
- Tiêu đề: "Sự kiện đang diễn ra" + nút "Xem tất cả >>".
- Danh sách sự kiện: ViewPager2 dạng carousel trượt ngang.
- Mỗi item: Ảnh banner sự kiện + tên.
- Nút "Xem tất cả >>": Chuyển đến EventActivity.
- Nhấn vào banner: Chuyển đến EventActivity.

### 2.5. Phần Menu Dưới (Bottom Navigation)
- Trang chủ (active khi ở màn hình này)
- Thi đấu (icon crossed swords) → MatchConfigActivity
- Xếp hạng (icon trophy) → RankingActivity
- Cá nhân (icon profile) → ProfileActivity

## 3. LUỒNG THAO TÁC

### 3.1. Kiểm Tra Phiên Đăng Nhập
1. Khi vào Activity (onCreate/onResume), kiểm tra userId > 0 và accessToken hợp lệ.
2. Nếu không hợp lệ: Chuyển ngay đến LoginActivity, clear task stack.

### 3.2. Luồng Tải Dữ Liệu
1. Gọi API lấy thông tin profile (getUserStats): cập nhật avatar, tên, level, EXP, các stat.
2. Gọi API lấy danh sách sự kiện đang diễn ra (getListEvents): cập nhật carousel.
3. userId được lưu vào SharedPreferences sau khi lấy profile thành công.

### 3.3. Luồng Bắt Đầu Thi Đấu
1. Người dùng nhấn card "BẮT ĐẦU THI ĐẤU" hoặc tab "Thi đấu" ở Bottom Nav.
2. Hệ thống chuyển đến MatchConfigActivity.

### 3.4. Luồng Chuyển Tab
1. Người dùng nhấn các tab ở Bottom Navigation.
2. Hệ thống chuyển màn hình tương ứng (start Activity mới).
