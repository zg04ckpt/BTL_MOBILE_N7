# 1.6. XỬ LÝ BÁO CÁO NGƯỜI DÙNG

## 1. NGHIỆP VỤ

### Mục đích
Admin xử lý báo cáo về hành vi vi phạm của người dùng.

### Luồng xử lý
1. Admin vào trang "Báo cáo người dùng"
2. Danh sách báo cáo chờ xử lý (ưu tiên: độ nghiêm trọng, số lượng báo cáo, thời gian)
3. Click một báo cáo → xem chi tiết: người báo cáo, người bị báo cáo, lý do, bằng chứng, lịch sử
4. Quyết định: Cảnh báo / Khóa tạm thời / Khóa vĩnh viễn / Bỏ qua
5. Nhập lý do xử lý → cập nhật trạng thái, thực hiện hành động, gửi thông báo 2 bên, log
6. Thông báo thành công

### Quy tắc
- Ưu tiên: gian lận > spam > hành vi không phù hợp
- 3 báo cáo trong 30 ngày → tự động khóa tạm 7 ngày
- Xử lý trong 48 giờ

### Validation
- Lý do xử lý: bắt buộc, 10–500 ký tự
- Phải chọn 1 hành động

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang danh sách báo cáo

- **Header:** "Báo cáo người dùng", có thể kèm badge "Chờ xử lý: X".
- **Toolbar:** Lọc theo Trạng thái (Tất cả / Chờ xử lý / Đã xử lý), Loại báo cáo (Gian lận, Spam, Hành vi không phù hợp, Khác); Sắp xếp (Độ ưu tiên cao nhất, Mới nhất, Cũ nhất).
- **Bảng (hoặc card list):** Cột: Thời gian báo cáo, Người báo cáo (avatar + tên), Người bị báo cáo (avatar + tên), Loại báo cáo (badge màu: Gian lận đỏ, Spam cam, Hành vi không phù hợp vàng), Trạng thái (Chờ / Đã xử lý), Hành động (Xem). Hàng "Chờ xử lý" có thể nổi bật nhẹ. Click hàng hoặc "Xem" → mở chi tiết báo cáo (trang hoặc modal).

---

### 2.2. Trang / Modal chi tiết báo cáo

- **Layout:** Desktop: 2 cột — Trái: thông tin báo cáo; Phải: form xử lý. Mobile: 1 cột, form ở dưới.
- **Phần thông tin báo cáo:** Người báo cáo và người bị báo cáo (avatar, tên, email, link "Xem hồ sơ"); thời gian báo cáo; loại báo cáo (badge); lý do báo cáo; bằng chứng (ảnh thumbnail, file, chat log nếu có); lịch sử báo cáo trước (nếu có).
- **Phần xử lý:** Chọn hành động (bắt buộc): Cảnh báo, Khóa tạm thời (kèm chọn số ngày 1–30), Khóa vĩnh viễn, Bỏ qua. Ô "Lý do xử lý" (bắt buộc, 10–500 ký tự, có đếm). Nút "Gửi xử lý" (primary); disable khi chưa chọn hành động hoặc chưa nhập lý do đủ.
- **Sau khi gửi:** Đóng modal hoặc về danh sách, toast thành công, cập nhật trạng thái.

---

### 2.3. Empty state và loading

- Không có báo cáo: icon + "Không có báo cáo chờ xử lý."
- Loading: skeleton bảng hoặc spinner.
