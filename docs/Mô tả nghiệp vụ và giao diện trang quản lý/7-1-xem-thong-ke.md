# 7.1. XEM THỐNG KÊ (BÁO CÁO VÀ THỐNG KÊ)

## 1. NGHIỆP VỤ

### Mục đích
Admin xem các báo cáo và thống kê hệ thống: người dùng, câu hỏi, trận đấu, tài chính (nếu có); chọn khoảng thời gian; export Excel/PDF; lên lịch gửi báo cáo định kỳ.

### Luồng xử lý
1. Admin truy cập "Báo cáo & Thống kê".
2. Chọn loại thống kê: Người dùng / Câu hỏi / Trận đấu / Tài chính.
3. Chọn khoảng thời gian: Hôm nay, 7 ngày, 30 ngày, 3 tháng, 1 năm, Tùy chọn (từ ngày – đến ngày).
4. Hệ thống tính và hiển thị: Số liệu tổng quan, biểu đồ, bảng chi tiết.
5. Admin có thể: Export Excel/PDF, lưu báo cáo, lên lịch gửi báo cáo định kỳ.

### Quy tắc nghiệp vụ
- Dữ liệu tính real-time từ database; có thể cache để tăng tốc.
- Export báo cáo có thể mất vài phút nếu dữ liệu lớn.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Layout:** Trang dashboard báo cáo, scroll dọc. Cấu trúc: Header (tiêu đề + bộ lọc thời gian + Export / Lên lịch) → Tabs loại thống kê ("Người dùng" | "Câu hỏi" | "Trận đấu" | "Tài chính") → KPI cards → Biểu đồ → Bảng chi tiết. Nền trang nhạt, card trắng bo góc, shadow nhẹ.

---

### 2.2. Header

- Sticky, nền trắng, border dưới. Trái: "Báo cáo & Thống kê" (có thể icon biểu đồ). Phải: Bộ lọc thời gian (Hôm nay, 7/30 ngày, 3 tháng, 1 năm, Tùy chọn với 2 date picker) | Nút Export (Excel/PDF, có loading khi tạo file) | Nút "Lên lịch gửi báo cáo" (mở modal).

---

### 2.3. Bộ chọn loại thống kê

- Tabs ngang; tab chọn nổi bật. Tab "Tài chính" có thể ẩn nếu không dùng.

---

### 2.4. KPI cards

- Lưới card: desktop 4 cột, tablet 2, mobile 1–2. Mỗi card: label nhỏ (vd. "Tổng người dùng mới") + value số lớn đậm + (tùy chọn) so sánh với kỳ trước (↑ xanh / ↓ đỏ) + icon (tùy chọn).
- **Theo loại:** Người dùng (mới, đăng nhập, active, bị khóa) | Câu hỏi (mới, đã duyệt, chờ duyệt, theo chủ đề) | Trận đấu (tổng, đã kết thúc, đang diễn ra, số người chơi) | Tài chính (nếu có).

---

### 2.5. Biểu đồ

- 1–3 biểu đồ trong card, có tiêu đề (vd. "Người dùng mới theo ngày"). Gợi ý: Line (xu hướng thời gian), Bar (so sánh nhóm/chủ đề), Pie/Donut (tỷ lệ). Có legend; palette màu thống nhất. Mobile: thu nhỏ hoặc scroll ngang.

---

### 2.6. Bảng chi tiết

- Dưới biểu đồ, trong card. Tiêu đề "Chi tiết" / "Dữ liệu chi tiết". Bảng có header, hover hàng; nội dung cột theo loại thống kê (ngày, số liệu, v.v.). Phân trang nếu nhiều dòng. Có thể nút "Export bảng này".

---

### 2.7. Modal "Lên lịch gửi báo cáo"

- Form: Email nhận (bắt buộc, có thể nhiều) | Tần suất (Hàng ngày / Tuần / Tháng) | Loại báo cáo (checkbox: Người dùng, Câu hỏi, Trận đấu, Tài chính) | Thời gian gửi (time picker). Nút "Hủy", "Lên lịch". Thành công → toast "Đã lên lịch gửi báo cáo." → đóng modal.

---

### 2.8. Loading

- Đổi loại thống kê hoặc khoảng thời gian: skeleton hoặc spinner cho vùng KPIs + biểu đồ + bảng. Export: nút "Đang tạo file..." + spinner; xong → tải file + toast "Export thành công."

---

### 2.9. Lỗi

- Không tải được: banner đỏ nhạt + text + "Thử lại". Export lỗi: toast đỏ "Export thất bại. Vui lòng thử lại."

---

### 2.10. Ghi chú designer

- Ưu tiên load KPIs và 1 biểu đồ trước (lazy load phần còn lại). Số tăng dùng xanh, số giảm dùng đỏ. Breadcrumb "Trang chủ > Báo cáo & Thống kê". Mobile: KPI 1–2 cột, biểu đồ full width, bảng scroll ngang.
