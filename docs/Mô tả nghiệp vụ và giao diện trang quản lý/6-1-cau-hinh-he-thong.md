# 6.1. CẤU HÌNH HỆ THỐNG

## 1. NGHIỆP VỤ

### Mục đích
Admin cấu hình các tham số hệ thống: thi đấu (số câu, thời gian, điểm), ghép trận (timeout, tiêu chí), thông báo (push, template).

### Luồng xử lý
1. Admin truy cập "Cấu hình hệ thống".
2. Hiển thị các tab: **Thi đấu**, **Ghép trận**, **Thông báo**.
3. Admin chỉnh sửa từng tab và bấm "Lưu".
4. Hệ thống validate (số câu 5–20, thời gian 10–600s, điểm hợp lệ); lưu cấu hình; áp dụng ngay (không cần restart).
5. Hiển thị thông báo thành công.

### Quy tắc nghiệp vụ
- Thay đổi cấu hình chỉ ảnh hưởng trận đấu mới; trận đang diễn ra không đổi.
- Nên thông báo trước khi thay đổi lớn.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Layout:** Trang cấu hình nhiều tab (settings). Cấu trúc: Header ("Cấu hình hệ thống" + nút "Lưu") → Tabs ("Thi đấu" | "Ghép trận" | "Thông báo") → Form theo tab. Nền trang nhạt, nội dung trong card trắng bo góc.

---

### 2.2. Header

- Sticky, nền trắng, border dưới. Trái: tiêu đề "Cấu hình hệ thống" (có thể kèm icon cài đặt). Phải: nút primary "Lưu"; khi đang lưu: disabled + spinner.

---

### 2.3. Tab navigation

- 3 tab ngang; tab chọn nổi bật (chữ đậm, gạch dưới primary).

---

### 2.4. Tab "Thi đấu"

- Tiêu đề section "Tham số thi đấu" + helper "Cấu hình áp dụng cho các trận đấu mới."
- **Trường:** Số câu hỏi mỗi trận (number, 5–20, mặc định 10) | Thời gian làm bài mặc định (giây, 10–600) | Điểm chính xác (min–max hoặc điểm mỗi câu đúng) | Điểm nhanh nhẹn (bonus, ≥ 0) | Điều kiện nhận điểm nhanh nhẹn (checkbox/radio: hoàn thành tất cả câu, số câu đúng tối thiểu). Các trường xếp dọc, có validation và thông báo lỗi.

---

### 2.5. Tab "Ghép trận"

- Tiêu đề "Tham số ghép trận" + helper "Cấu hình phòng chờ và tiêu chí ghép trận."
- **Trường:** Thời gian chờ ghép trận (giây, 10–300) | Ghép theo điểm (switch, mặc định Bật) | Khoảng điểm chênh lệch ± (khi bật ghép theo điểm) | Ghép theo chủ đề yêu thích (switch) | Số người tối đa trong phòng chờ (2–100).

---

### 2.6. Tab "Thông báo"

- Tiêu đề "Cấu hình thông báo".
- **Trường:** Thông báo push (switch) | Nội dung thông báo mặc định (textarea, placeholder mẫu) | Template thông báo (danh sách + "Thêm template" nếu có).

---

### 2.7. Nút "Lưu"

- Ở header. Trạng thái: Default → Loading ("Đang lưu...") → Success toast hoặc Error toast. Sau lưu: cấu hình áp dụng ngay, không cần reload.

---

### 2.8. Cảnh báo

- Khi thay đổi số câu hoặc thời gian làm bài quá nhiều: banner vàng nhạt + text "Thay đổi này ảnh hưởng đến trải nghiệm người chơi. Nên thông báo trước."

---

### 2.9. Loading & lỗi

- Load cấu hình: skeleton form hoặc spinner. Lưu lỗi: toast đỏ, form giữ nguyên.

---

### 2.10. Ghi chú designer

- Mỗi tab có tiêu đề section và helper text ngắn. Có thể breadcrumb "Trang chủ > Cấu hình hệ thống". Tab Thông báo có thể mở rộng (email, SMS) tùy sản phẩm.

---

### 2.11. Mobile

- **Tabs:** 3 tab scroll ngang; tab chọn nổi bật.
- **Form:** 1 cột; các trường xếp dọc, khoảng cách rõ. Switch/input dễ bấm (vùng chạm đủ lớn).
- **Nút Lưu:** Giữ trong header (sticky) hoặc sticky bottom; full width trên mobile nếu đặt dưới.
