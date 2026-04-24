# GIAO DIỆN QUẢN LÝ TRẬN ĐẤU (ADMIN)

## 1. MỤC ĐÍCH

Giao diện giúp admin theo dõi danh sách trận đấu và xem thông tin tổng quan cơ bản.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần thống kê nhanh
- Hiển thị 3 chỉ số dạng thẻ:
  - Trận đang diễn ra
  - Trận trong ngày
  - Người chơi đồng thời
- Giá trị đang hiển thị dạng tĩnh trên UI (chưa bind API realtime)

### 2.2. Danh sách trận đấu
- Dữ liệu lấy từ API `getAllMatches` theo phân trang
- Các cột:
  - `ID`
  - `Thời gian` (ghép `startTime - endTime` nếu có)
  - `Loại trận đấu`
  - `Số lượng người chơi`
  - `Trạng thái`
  - `Chủ đề`
  - `Hành động` (icon xem - hiện tại chỉ hiển thị, chưa mở màn chi tiết)

### 2.3. Phân trang
- Có phân trang và thay đổi số bản ghi/trang (`5/10/20/50`)
- Khi đổi trang/page size sẽ gọi lại API

## 3. LUỒNG THAO TÁC

### 3.1. Xem danh sách trận
1. Mở trang quản lý trận đấu.
2. Hệ thống tải trang đầu danh sách trận.
3. Admin chuyển trang để xem các bản ghi khác.

## 4. PHẠM VI HIỆN TẠI
- Chưa có bộ lọc nâng cao theo user/match ID trên UI này.
- Chưa có màn log chi tiết trận đấu.
- Chưa có tác vụ can thiệp trận (hủy/hoàn tác/force stop) từ frontend.

## 5. RESPONSIVE
- Giao diện ưu tiên desktop với bảng dữ liệu.
