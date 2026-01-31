# 2.4. DUYỆT CÂU HỎI

## 1. NGHIỆP VỤ

### Mục đích
Moderator/Admin duyệt câu hỏi đang chờ duyệt.

### Luồng xử lý
1. Vào "Câu hỏi chờ duyệt" → danh sách Pending
2. Click một câu hỏi → xem chi tiết: đầy đủ thông tin, preview như người chơi thấy, lịch sử chỉnh sửa
3. Quyết định: Duyệt / Từ chối / Yêu cầu chỉnh sửa
4. Duyệt: status Approved, thông báo người tạo, log
5. Từ chối: nhập lý do (10–500 ký tự), status Rejected, thông báo người tạo, log
6. Yêu cầu chỉnh sửa: nhập yêu cầu (10–500 ký tự), status Needs Revision, thông báo người tạo, log
7. Thông báo thành công

### Quy tắc
- Chỉ Approved mới dùng trong thi đấu
- Một câu hỏi chỉ 1 Moderator duyệt (tránh duplicate)
- Rejected có thể tạo lại; Needs Revision sau khi sửa → Pending

### Validation
- Lý do từ chối: 10–500 ký tự
- Yêu cầu chỉnh sửa: 10–500 ký tự

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang danh sách "Câu hỏi chờ duyệt"

- **Header:** "Câu hỏi chờ duyệt", badge số câu Pending.
- **Danh sách:** Bảng (desktop) hoặc card (mobile): ID, Nội dung (trích), Chủ đề, Mức độ khó, Người tạo, Ngày gửi, Hành động (Xem / Duyệt nhanh). Lọc/sắp xếp theo chủ đề, ngày gửi; mobile: gom lọc vào nút "Bộ lọc" mở drawer. Click hàng/card hoặc "Xem" → mở chi tiết duyệt (trang full hoặc modal; mobile ưu tiên trang full).

---

### 2.2. Trang / Modal chi tiết duyệt

- **Layout:** Desktop 2 cột. Trái: thông tin đầy đủ câu hỏi (nội dung, chủ đề, mức độ, điểm, thời gian, 4 đáp án, đáp án đúng, giải thích, media) + preview "Giao diện người chơi" (hiển thị như trong app) + lịch sử chỉnh sửa (nếu có). Phải: form quyết định duyệt.
- **Form quyết định:** Chọn "Duyệt" (xanh) | "Từ chối" (đỏ) | "Yêu cầu chỉnh sửa" (cam). Khi Từ chối hoặc Yêu cầu chỉnh sửa: hiện textarea lý do (bắt buộc, 10–500 ký tự). Nút "Gửi quyết định" (primary); disable khi thiếu lý do. Sau khi gửi: đóng modal/trang, toast tương ứng, danh sách Pending cập nhật.
- **Mobile:** 1 cột: thông tin câu hỏi + preview phía trên (scroll), form quyết định phía dưới hoặc sticky bottom. Ưu tiên full-screen trang thay modal giữa màn (tránh modal quá cao). Form quyết định: 3 nút Duyệt / Từ chối / Yêu cầu chỉnh sửa full width hoặc rõ ràng; textarea lý do khi cần.

---

### 2.3. Trạng thái "Đang được xem xét"

- Khi Moderator mở chi tiết có thể đánh dấu "đang xem" (tránh trùng). Trong danh sách, câu hỏi đó có badge "Đang xem xét" và có thể ẩn/disable "Xem" cho người khác (tùy nghiệp vụ).
