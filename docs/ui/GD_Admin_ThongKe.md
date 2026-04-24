# GIAO DIỆN XEM THỐNG KÊ (ADMIN)

## 1. MỤC ĐÍCH

Giao diện cung cấp số liệu tổng quan hệ thống, xu hướng user và danh sách user đăng ký gần đây.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Header & Bộ lọc thời gian
- Tiêu đề: `Báo cáo & thống kê`
- Bộ lọc:
  - Chọn nhanh `7 ngày qua` hoặc `30 ngày qua`
  - Hoặc chọn khoảng ngày bằng `range picker`
- Nút:
  - `Export` (mở API xuất file theo filter hiện tại)
  - `Lên lịch` (đang là UI button, chưa có luồng xử lý)

### 2.2. Tabs hiển thị
- Có 3 tab UI: `Người dùng`, `Câu hỏi`, `Trận đấu`
- Hiện tại dữ liệu đang render theo bộ thống kê user/system chung (chưa có luồng riêng theo từng tab)

### 2.3. Khu vực KPI cards
- 4 thẻ chính:
  - `Người dùng mới`
  - `Tổng người dùng`
  - `Tổng đăng ký`
  - `Peak CCU`
- Mỗi thẻ hiển thị giá trị + phần trăm thay đổi tăng/giảm

### 2.4. Khu vực biểu đồ
- Biểu đồ area: `Xu hướng người dùng mới`
- Biểu đồ donut: `Tỷ lệ tài khoản` (`Active/Banned/Inactive`)

### 2.5. Bảng chi tiết user gần đây
- Cột: `ID`, `Tên hiển thị`, `Email`, `Ngày đăng ký`, `Trạng thái`, `Tổng số trận`
- Có phân trang, đổi page size

## 3. LUỒNG THAO TÁC

### 3.1. Xem dữ liệu
1. Khi vào trang, hệ thống tải:
   - Thống kê tổng quan (`getSystemAnalytics`)
   - Danh sách user gần đây (`getRecentUsersAnalytics`)
2. Admin thay đổi filter thời gian, dữ liệu sẽ dùng filter đó cho lần tải/export tiếp theo.

### 3.2. Export
1. Nhấn `Export`.
2. Hệ thống tạo query theo `lastDays` hoặc `startDate/endDate`.
3. Mở endpoint export trong tab mới để tải file.

## 4. QUY TẮC NGHIỆP VỤ
- Nếu chọn khoảng ngày thủ công thì ưu tiên `startDate/endDate`; nếu không sẽ dùng `lastDays`.
- Trạng thái user được map màu tag theo giá trị Active/Banned/Inactive.
- Khi API lỗi sẽ hiển thị thông báo lỗi ở frontend.

## 5. RESPONSIVE
- Layout sử dụng grid responsive (`xs/sm/lg`) cho KPI và chart, ưu tiên trải nghiệm desktop.
