# GIAO DIỆN XEM THỐNG KÊ (ADMIN)

## 1. MỤC ĐÍCH

Giao diện Xem thống kê cung cấp cái nhìn tổng quan và chi tiết về tình hình hoạt động của hệ thống thông qua các báo cáo số liệu và biểu đồ trực quan.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header & Bộ Lọc
- Tiêu đề: "Báo cáo & Thống kê".
- Bộ lọc thời gian (Bên phải):
  - Dropdown chọn nhanh: Hôm nay, 7 ngày qua, 30 ngày qua, 3 tháng, 1 năm.
  - Tùy chọn: Date picker (Từ ngày - Đến ngày).
- Các nút chức năng:
  - Nút "Export": Xuất báo cáo ra Excel/PDF.
  - Nút "Lên lịch": Cài đặt gửi báo cáo định kỳ qua email.

### 2.2. Tabs Phân Loại Thống Kê
- Tab "Người dùng": Số liệu về đăng ký user, active user.
- Tab "Câu hỏi": Số lượng câu hỏi mới, tỷ lệ duyệt, thống kê theo chủ đề.
- Tab "Trận đấu": Tổng số trận, giờ cao điểm, tỷ lệ hoàn thành.
- Tab "Tài chính" (Nếu có): Doanh thu, nạp tiền.

### 2.3. Khu Vực Chỉ Số KPI (KPI Cards)
- Hiển thị lưới các thẻ chỉ số quan trọng (3-4 thẻ hàng ngang):
  - Label: Tên chỉ số (Ví dụ: Tổng người dùng mới).
  - Value: Số liệu thực tế (In đậm, to).
  - Trend: So sánh với kỳ trước (Mũi tên Xanh tăng / Đỏ giảm).
  - Icon minh họa mờ nền.

### 2.4. Khu Vực Biểu Đồ (Charts)
- Biểu đồ đường (Line Chart): Xu hướng thay đổi theo thời gian (Ví dụ: CCU theo giờ, New User theo ngày).
- Biểu đồ cột (Bar Chart): So sánh giữa các nhóm (Ví dụ: Số lượng câu hỏi theo Chủ đề).
- Biểu đồ tròn (Pie/Donut Chart): Tỷ lệ phần trăm (Ví dụ: Tỷ lệ các gói nạp, Tỷ lệ Trận Solo vs Multiplayer).
- Có Legend chú thích màu sắc.

### 2.5. Bảng Chi Tiết (Data Table)
- Hiển thị dữ liệu chi tiết tương ứng với biểu đồ bên trên.
- Các cột dữ liệu số liệu cụ thể.
- Phân trang nếu dữ liệu dài.

## 3. LUỒNG THAO TÁC

### 3.1. Xem Báo Cáo
1. Admin vào trang "Báo cáo & Thống kê".
2. Hệ thống mặc định hiển thị Tab "Người dùng" và thời gian "7 ngày qua".
3. Admin đổi Tab sang "Trận đấu" -> KPI cards và Biểu đồ cập nhật theo dữ liệu trận đấu.
4. Admin đổi thời gian sang "Hôm nay" -> Tất cả số liệu được tính toán lại và hiển thị.

### 3.2. Xuất Báo Cáo
1. Admin nhấn nút "Export".
2. Chọn định dạng (Excel hoặc PDF).
3. Hệ thống xử lý (Loading) và tải file xuống máy.

## 4. QUY TẮC NGHIỆP VỤ
- Dữ liệu hiển thị nên được tính toán gần như Real-time hoặc cache ngắn (5-10 phút).
- Các biểu đồ phải có Tooltip khi hover để xem con số chính xác.
- Export file với dữ liệu lớn cần có tiến trình xử lý nền (Background job) nếu mất quá nhiều thời gian.

## 5. RESPONSIVE
- **Desktop:** Layout Dashboard đầy đủ, 4 cột KPI, Biểu đồ lớn.
- **Tablet:** 2 cột KPI, Biểu đồ vừa.
- **Mobile:** 1 cột KPI, Biểu đồ thu nhỏ (có thể cuộn ngang), ẩn bớt các cột trong bảng chi tiết.
