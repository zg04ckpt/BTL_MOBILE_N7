# GIAO DIỆN BẢNG XẾP HẠNG (LEADERBOARD)

## 1. MỤC ĐÍCH

Hiển thị thứ hạng người chơi dựa trên thành tích tích lũy để kích thích tính cạnh tranh và giữ chân người dùng.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header Filter (Bộ Lọc Thời Gian)
- Tabs chuyển đổi thời gian:
  - [Tháng] | [Năm] | [Tất cả]
  - Trạng thái: Tab đang chọn được highlight (đậm màu, có nền bo tròn).
  - Mặc định: Tab "Tất cả" được chọn khi vào trang.

### 2.2. Phần Top 3 (Podium)
- Khu vực vinh danh 3 người đứng đầu:
  - Vị trí Top 1: Ở giữa, cao nhất, avatar lớn nhất, khung viền vàng.
  - Vị trí Top 2: Bên trái, thấp hơn Top 1, khung viền bạc.
  - Vị trí Top 3: Bên phải, thấp hơn Top 1, khung viền đồng.
- Thông tin mỗi người: Avatar (tròn), Tên, Số điểm (RP).

### 2.3. Phần Danh Sách (List View)
- Bảng danh sách các thứ hạng tiếp theo (từ 4 đến 8):
  - Cột 1 (Rank): Số thứ tự (4, 5, 6, 7, 8).
  - Cột 2 (Info): Avatar (nhỏ, tròn) + Level badge + Tên.
  - Cột 3 (Point): Tổng điểm RP (Ranking Point).
- Cuộn dọc: Cho phép xem hết danh sách trong card.

### 2.4. Phần Sticky Bottom Bar (Hạng Của Tôi)
- Thanh cố định ở dưới cùng màn hình (phía trên nút về trang chủ):
  - Hiển thị thứ hạng của chính Người dùng hiện tại.
  - Thông tin: Rank, Avatar, Tên, Điểm RP.
  - Nếu chưa đăng nhập: Hiển thị "Đăng nhập để xem xếp hạng của bạn".
  - Dữ liệu lấy từ toàn bộ danh sách API (không giới hạn top 8).

### 2.5. Nút Về Trang Chủ
- Nút "Về trang chủ" ở dưới cùng.
- Hành động: Đóng Activity, quay về màn hình trước.

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Xem Bảng Xếp Hạng
1. Người dùng nhấn icon "Xếp hạng" tại Bottom Navigation của Trang chủ.
2. Hệ thống tải dữ liệu mặc định (Tất cả).
3. Hiển thị Top 3 nổi bật và danh sách rank 4-8 bên dưới.
4. Hiển thị hạng của người dùng ở thanh dưới cùng.

### 3.2. Luồng Lọc Dữ Liệu
1. Người dùng chọn tab "Tháng" hoặc "Năm".
2. Hệ thống tải lại dữ liệu tương ứng và cập nhật giao diện.

### 3.3. Luồng Xem Profile
1. Người dùng click vào một item người chơi bất kỳ trong danh sách.
2. Hệ thống hiện thông báo "Chức năng đang phát triển" (Toast).

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Cách Tính Điểm Xếp Hạng (RP)
- Thắng: +20 RP
- Thua: -10 RP
- Hòa: +5 RP
- Hệ thống điểm độc lập với điểm trong trận đấu (In-game score).

### 4.2. Thuật Toán Xếp Hạng
Ưu tiên giảm dần:
1. Tổng điểm RP cao nhất.
2. Tỷ lệ thắng (Win Rate) cao hơn (nếu RP bằng nhau).
3. Chuỗi thắng (Winning Streak) dài hơn.
4. Thời gian đạt điểm sớm hơn.
5. Nếu tất cả trùng nhau: Đồng hạng.

### 4.3. Hiển Thị
- Hiển thị Top 8 trên giao diện (Top 3 ở Podium, rank 4-8 ở danh sách) để tối ưu hiệu năng.
- Người dùng ngoài Top 8 vẫn thấy hạng cụ thể của mình ở Sticky Bottom Bar (VD: #102).
- Toàn bộ dữ liệu từ API được lưu riêng để tra cứu hạng người dùng hiện tại.
