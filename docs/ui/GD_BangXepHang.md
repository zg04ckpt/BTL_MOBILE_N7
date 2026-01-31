# GIAO DIỆN BẢNG XẾP HẠNG (LEADERBOARD)

## 1. MỤC ĐÍCH

Hiển thị thứ hạng người chơi dựa trên thành tích tích lũy để kích thích tính cạnh tranh và giữ chân người dùng.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header Filter (Bộ Lọc)
- Tabs chuyển đổi thời gian:
  - [Tuần] | [Tháng] | [Tổng]
  - Trạng thái: Tab đang chọn được highlight (đậm màu, gạch chân).
- Tabs chuyển đổi phạm vi:
  - [Toàn Server] | [Bạn bè]
  - Trạng thái: Switch hoặc Tabs con.

### 2.2. Phần Top 3 (Podium)
- Khu vực vinh danh 3 người đứng đầu:
  - Vị trí Top 1: Ở giữa, cao nhất, avatar lớn nhất, khung viền vàng, hiệu ứng hào quang.
  - Vị trí Top 2: Bên trái, thấp hơn Top 1, khung viền bạc.
  - Vị trí Top 3: Bên phải, thấp hơn Top 1, khung viền đồng.
- Thông tin mỗi người: Avatar, Tên, Số điểm.

### 2.3. Phần Danh Sách (List View)
- Bảng danh sách các thứ hạng tiếp theo (từ 4 đến 25):
  - Cột 1 (Rank): Số thứ tự (4, 5, 6...).
  - Cột 2 (Info): Avatar (nhỏ) + Tên + Level.
  - Cột 3 (Sub-stat): Tỷ lệ thắng (Win rate) hoặc Chuỗi thắng (Streak).
  - Cột 4 (Point): Tổng điểm RP (Ranking Point).
- Cuộn dọc: Cho phép xem hết danh sách Top 25.

### 2.4. Phần Sticky Bottom Bar (Hạng Của Tôi)
- Thanh cố định ở dưới cùng màn hình:
  - Hiển thị thứ hạng của chính Người dùng (My Rank).
  - Các thông tin tương tự như một dòng trong danh sách: Rank, Info, Sub-stat, Point.
  - Mục đích: Giúp người dùng biết vị trí của mình ngay cả khi không nằm trong Top hiển thị.

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Xem Bảng Xếp Hạng
1. Người dùng nhấn icon "Leaderboard" hoặc tab "Xếp hạng" tại Trang chủ.
2. Hệ thống tải dữ liệu mặc định (Ví dụ: Tuần - Toàn Server).
3. Hiển thị Top 3 nổi bật và danh sách bên dưới.
4. Hiển thị hạng của người dùng ở thanh dưới cùng.

### 3.2. Luồng Lọc Dữ Liệu
1. Người dùng chọn tab "Tháng" hoặc "Tổng".
2. Hệ thống tải lại dữ liệu tương ứng.
3. Người dùng chọn tab "Bạn bè".
4. Hệ thống hiển thị bảng xếp hạng chỉ gồm bạn bè của người dùng.

### 3.3. Luồng Xem Profile
1. Người dùng click vào một item người chơi bất kỳ trong danh sách.
2. Hệ thống mở Popup "User Profile" hiển thị thông tin chi tiết người đó.

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
- Chỉ hiển thị Top 25 trên danh sách chính để tối ưu hiệu năng.
- Người dùng ngoài Top 25 vẫn thấy hạng cụ thể của mình ở Sticky Bottom Bar (VD: #10,502).


