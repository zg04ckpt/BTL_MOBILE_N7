# 3.4. XỬ LÝ LỖI KỸ THUẬT

## 1. NGHIỆP VỤ

### Mục đích
Xử lý các trận đấu bị lỗi kỹ thuật (người chơi báo lỗi hoặc hệ thống phát hiện).

### Luồng xử lý
1. Người chơi báo lỗi hoặc hệ thống đánh dấu trạng thái "Error" / "Incomplete".
2. Admin xem trận đấu bị lỗi: trạng thái, lý do lỗi (nếu có).
3. Admin quyết định: **Hủy trận đấu** / **Tạo lại trận đấu** / **Hoàn điểm** (nếu người chơi đã trả phí).
4. Hệ thống thực hiện tương ứng và log.

### Quy tắc nghiệp vụ
- Lỗi kỹ thuật nên được xử lý trong vòng 24 giờ.
- Người chơi không bị mất điểm do lỗi hệ thống.
- Có thể tạo lại trận nếu cả hai người chơi đồng ý.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Danh sách trận đấu lỗi

- **Vị trí:** Menu "Quản lý trận đấu" > "Trận đấu lỗi" (hoặc bộ lọc trạng thái Lỗi / Chưa hoàn thành trong danh sách trận đấu).
- **Layout:** Giống trang danh sách trận đấu (3-1): tiêu đề "Trận đấu lỗi" (có thể kèm badge); chỉ hiển thị trận Error / Incomplete; cột Trạng thái (badge Lỗi / Chưa hoàn thành); cột Lý do lỗi (nếu có); cột hành động "Xem & xử lý" mở chi tiết + panel xử lý lỗi.

---

### 2.2. Trang chi tiết trận + Panel "Xử lý lỗi kỹ thuật"

- **Bố cục:** Trái (hoặc trên mobile): Chi tiết trận đấu (3-2). Phải (hoặc dưới): **Panel "Xử lý lỗi kỹ thuật"** (card, sticky desktop).
- **Panel:** Tiêu đề "Xử lý lỗi kỹ thuật". Block thông tin lỗi (Trạng thái, Lý do – chỉ đọc). Chọn cách xử lý: **Hủy trận đấu** / **Tạo lại trận đấu** / **Hoàn điểm** (chỉ hiện nếu trận có phí). Textarea **Lý do xử lý** (bắt buộc, 10–300 ký tự). Nút **"Xác nhận"** (primary). Trước khi gửi: dialog xác nhận (tóm tắt hành động). Sau thành công: toast, cập nhật trạng thái.
- **Đặc biệt:** "Tạo lại" → có thể thông báo + nút "Xem trận mới". "Hoàn điểm" → chỉ enable khi có dữ liệu phí; hiển thị số điểm sẽ hoàn.
- **Loading:** Nút Xác nhận spinner + disabled. **Lỗi:** Toast lỗi; giữ form để sửa và gửi lại.
