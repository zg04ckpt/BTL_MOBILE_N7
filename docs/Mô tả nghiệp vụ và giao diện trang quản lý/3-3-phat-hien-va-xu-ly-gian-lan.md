# 3.3. PHÁT HIỆN VÀ XỬ LÝ GIAN LẬN

## 1. NGHIỆP VỤ

### Mục đích
Phát hiện (tự động) và xử lý (thủ công) các trận đấu có dấu hiệu gian lận.

### Luồng phát hiện (tự động)
- Sau mỗi trận đấu, hệ thống phân tích: thời gian trả lời TB < 2s/câu → Cảnh báo; điểm 100% và thời gian < 30s → Cảnh báo; trả lời đúng 100% câu khó → Cảnh báo; IP 2 người chơi giống nhau → Cảnh báo.
- Nếu có cảnh báo: đánh dấu trận "Cần kiểm tra", gửi thông báo admin, hiển thị trong "Trận đấu nghi vấn".

### Luồng xử lý (thủ công)
1. Admin xem danh sách "Trận đấu nghi vấn".
2. Admin xem chi tiết trận (timeline, IP, thiết bị, lịch sử người chơi).
3. Admin quyết định: **Hợp lệ** (bỏ cờ) hoặc **Gian lận** (xử lý).
4. Nếu Gian lận: Chọn hành động (Hủy kết quả / Trừ điểm / Khóa tài khoản), nhập lý do, xác nhận. Hệ thống thực hiện và log.

### Quy tắc nghiệp vụ
- Tự động phát hiện theo tiêu chí đã định nghĩa.
- Admin phải xem xét kỹ trước khi kết luận gian lận.
- Người chơi có quyền khiếu nại.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang danh sách "Trận đấu nghi vấn"

- **Vị trí:** Menu "Quản lý trận đấu" > "Trận đấu nghi vấn" (hoặc tab/submenu).
- **Layout:** Giống trang danh sách trận đấu (3-1): tiêu đề "Trận đấu nghi vấn" (có thể kèm badge số lượng); chỉ hiển thị trận có cờ "Cần kiểm tra"; thêm cột **Lý do cảnh báo** (ví dụ thời gian trả lời TB quá nhanh, IP trùng, điểm cao bất thường); cột hành động "Xem & xử lý" mở chi tiết trận + panel xử lý. Có thể lọc theo loại cảnh báo, theo ngày.

---

### 2.2. Trang chi tiết trận + Panel "Xử lý gian lận"

- **Bố cục:** Trái (hoặc trên mobile): Chi tiết trận đấu như file 3-2. Phải (hoặc dưới mobile): **Panel "Xử lý gian lận"** (card, sticky trên desktop).
- **Panel:** Tiêu đề "Xử lý gian lận". Block **Lý do cảnh báo** (chỉ đọc, nền vàng nhạt). Hai nút: **"Hợp lệ"** (secondary xanh) → bỏ cờ, xác nhận qua dialog; **"Gian lận"** (primary đỏ) → hiện form: chọn hành động (Hủy kết quả / Trừ điểm / Khóa tài khoản), chọn người chơi gian lận (nếu áp dụng), textarea **Lý do** (bắt buộc, 20–500 ký tự), nút **"Xác nhận xử lý"**. Trước khi gửi: dialog xác nhận cuối. Sau thành công: toast, cập nhật trạng thái trận.
- **Dialog "Hợp lệ":** Modal nhỏ, text xác nhận bỏ đánh dấu nghi vấn, nút Hủy / Xác nhận.
- **Dialog "Xử lý gian lận":** Modal vừa, tóm tắt hành động và lý do, cảnh báo không thể hoàn tác, nút Hủy / Xác nhận xử lý.
- **Loading:** Nút chuyển spinner + disabled đến khi API trả về. **Lỗi:** Toast lỗi; giữ form để sửa và gửi lại.
