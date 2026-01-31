# 5.2. QUẢN LÝ SỰ KIỆN

## 1. NGHIỆP VỤ

### Mục đích
Admin xem danh sách sự kiện (Sắp diễn ra / Đang diễn ra / Đã kết thúc), chỉnh sửa/hủy sự kiện sắp diễn ra, xem thống kê real-time sự kiện đang diễn ra, xem bảng xếp hạng và trao giải khi đã kết thúc.

### Luồng xử lý
- **Sắp diễn ra:** Xem, chỉnh sửa, hủy; xem số người đăng ký.
- **Đang diễn ra:** Xem thống kê (số người tham gia, số trận, top điểm); không chỉnh sửa quy tắc.
- **Đã kết thúc:** Xem bảng xếp hạng cuối, trao giải, export báo cáo.

### Quy tắc nghiệp vụ
- Sự kiện đang diễn ra không thể hủy.
- Sau khi kết thúc, tự động tính và trao giải; bảng xếp hạng sự kiện lưu vĩnh viễn.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Layout:** Trang danh sách + tabs theo trạng thái sự kiện.
- **Cấu trúc:** Header ("Quản lý sự kiện" + nút "Tạo sự kiện mới") → Tabs ("Sắp diễn ra" | "Đang diễn ra" | "Đã kết thúc") → Danh sách card/bảng sự kiện. Nền trang nhạt, nội dung trong vùng trắng bo góc.

---

### 2.2. Header

- Sticky, nền trắng, có border dưới. Trái: tiêu đề "Quản lý sự kiện". Phải: nút primary "Tạo sự kiện mới" → mở trang Tạo sự kiện (5-1).

---

### 2.3. Tabs trạng thái

- 3 tab ngang; tab chọn nổi bật (chữ đậm, gạch dưới/gạch dưới màu primary). Có thể kèm badge số lượng (vd. "Sắp diễn ra (3)").

---

### 2.4. Danh sách sự kiện (card)

- **Layout:** Lưới card: desktop 2–3 cột, tablet 2 cột, mobile 1 cột.
- **Mỗi card:** Banner/ảnh phía trên (hoặc placeholder nếu không ảnh) → Tên sự kiện (đậm) → Thời gian (icon đồng hồ + Bắt đầu/Kết thúc hoặc "Còn X ngày" / "Đang diễn ra" / "Đã kết thúc") → Badge trạng thái (Sắp diễn ra / Đang diễn ra / Đã kết thúc, màu phân biệt) → Thông tin bổ sung (số người đăng ký, số trận, top/người thắng tùy trạng thái) → Footer: nút hành động.
- **Hành động theo trạng thái:** Sắp diễn ra: "Chỉnh sửa", "Hủy sự kiện"; Đang diễn ra: "Xem thống kê"; Đã kết thúc: "Xem bảng xếp hạng", "Export báo cáo". Click card → Chi tiết sự kiện.
- **Hover (desktop):** Card có shadow nhẹ, cursor pointer.

---

### 2.5. Chi tiết sự kiện

- Header: Nút quay lại + Tên sự kiện + Badge trạng thái. Tabs: "Tổng quan" | "Thống kê" | "Bảng xếp hạng" (đã kết thúc) | "Cài đặt" (chỉ sắp diễn ra).
- **Tổng quan:** Banner, tên, mô tả, thời gian, chủ đề, số câu, thời gian làm bài, phần thưởng; sắp diễn ra có "Chỉnh sửa", "Hủy sự kiện", số người đăng ký.
- **Thống kê:** Số người tham gia, số trận; top 10 người chơi; có thể có biểu đồ đơn giản.
- **Bảng xếp hạng:** Bảng Hạng | Avatar + Tên | Điểm | Số trận thắng; nút "Trao giải", "Export báo cáo".
- **Cài đặt:** Form chỉnh sửa giống Tạo sự kiện; "Lưu thay đổi", "Hủy sự kiện" (có xác nhận).

---

### 2.6. Dialog "Hủy sự kiện"

- Modal nhỏ. Nội dung: xác nhận hủy sự kiện [Tên], người đăng ký sẽ được thông báo. Nút "Không", "Hủy sự kiện" (primary đỏ). Thành công → toast → quay danh sách.

---

### 2.7. Empty state

- Không có sự kiện trong tab: icon + text "Chưa có sự kiện [trạng thái]."; tab "Sắp diễn ra" có nút "Tạo sự kiện mới".

---

### 2.8. Loading & lỗi

- Loading: skeleton card hoặc spinner. Lỗi: banner đỏ nhạt + text + nút "Thử lại".

---

### 2.9. Responsive

- Desktop: 3 cột card. Tablet: 2 cột; tabs có thể scroll. Mobile: 1 cột; tabs scroll hoặc dropdown; nút "Tạo sự kiện" full width hoặc FAB.

---

### 2.10. Ghi chú designer

- "Đang diễn ra" nên nổi bật (badge "Live", viền/màu xanh). Chi tiết sự kiện có breadcrumb: "Quản lý sự kiện > [Tên sự kiện]".
