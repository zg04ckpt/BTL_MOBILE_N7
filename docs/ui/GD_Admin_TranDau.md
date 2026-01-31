# GIAO DIỆN QUẢN LÝ TRẬN ĐẤU (ADMIN)

## 1. MỤC ĐÍCH

Giao diện Quản lý trận đấu giúp Admin theo dõi lịch sử các trận đấu diễn ra, phát hiện bất thường và xử lý các khiếu nại liên quan đến kết quả.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Thống Kê Nhanh
- Số trận đang diễn ra (Realtime)
- Tổng số trận trong ngày
- CCU (Concurrent Users): Số người chơi đồng thời

### 2.2. Danh Sách Trận Đấu
- Cột Mã trận đấu
- Cột Thời gian bắt đầu / Kết thúc
- Cột Loại trận (Solo / PvP / Tournament)
- Cột Người tham gia (List avt/tên)
- Cột Trạng thái (Đang diễn ra / Kết thúc / Lỗi)
- Cột Người thắng (Nếu đã kết thúc)
- Hành động: Xem chi tiết log

### 2.3. Chi Tiết Trận Đấu (Log View)
- Thông tin chung: ID, Room ID, Mode, Server xử lý
- Timeline:
  - 10:00:01 - Bắt đầu
  - 10:00:05 - User A trả lời câu 1 (Đúng - 2.5s)
  - 10:00:06 - User B trả lời câu 1 (Sai - 3.0s)
  - ...
- Kết quả cuối cùng

## 3. LUỒNG THAO TÁC

### 3.1. Xem Lịch Sử
1. Admin tìm kiếm theo ID người dùng hoặc ID trận đấu
2. Danh sách lọc ra các trận đấu liên quan
3. Nhấn vào một dòng để xem chi tiết diễn biến

### 3.2. Xử Lý Trận Đấu Lỗi
1. Admin nhận báo cáo trận đấu bị kẹt hoặc lỗi điểm
2. Tìm trận đấu theo ID
3. Thực hiện hành động: "Hủy trận đấu" hoặc "Hoàn tác kết quả" (nếu có tính năng này)

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Lưu Trữ
- Log chi tiết trận đấu được lưu trữ trong thời gian nhất định (ví dụ 30 ngày) để tiết kiệm dung lượng
- Các thông tin tổng hợp (Kết quả, điểm số) được lưu vĩnh viễn

### 4.2. Bảo Mật
- Admin không được can thiệp vào trận đấu đang diễn ra trừ trường hợp khẩn cấp (Force Stop)

## 5. RESPONSIVE
- Chủ yếu tối ưu cho Desktop để xem log và bảng dữ liệu rộng.
