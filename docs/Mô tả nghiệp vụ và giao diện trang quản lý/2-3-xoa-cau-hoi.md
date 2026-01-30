# 2.3. XÓA CÂU HỎI

## 1. NGHIỆP VỤ

### Mục đích
Xóa câu hỏi khỏi ngân hàng (soft delete).

### Luồng xử lý
1. Chọn câu hỏi → "Xóa"
2. Kiểm tra: đang dùng trong trận đấu? số lần đã sử dụng?
3. Dialog xác nhận: thông tin câu hỏi, số lần sử dụng, cảnh báo nếu đang dùng, nhập lý do xóa
4. Xác nhận → soft delete (deleted_at), ẩn khỏi danh sách, log
5. Thông báo thành công

### Quy tắc
- Soft delete; có thể khôi phục trong 30 ngày
- Đã xóa không dùng trong trận mới; vẫn hiển thị trong lịch sử trận cũ
- Không xóa nếu đang trong trận đấu đang diễn ra

### Validation
- Lý do: 10–200 ký tự
- Không xóa nếu đang trong trận đấu đang diễn ra

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Điểm vào

- **Nút "Xóa câu hỏi":** Trong dropdown hành động (danh sách hoặc trang chi tiết câu hỏi). Màu đỏ, icon thùng rác.

---

### 2.2. Dialog "Xóa câu hỏi"

- **Header:** Icon cảnh báo + tiêu đề "Xóa câu hỏi" + nút đóng.
- **Nội dung:** Block thông tin câu hỏi (trích nội dung, chủ đề, mức độ khó); số lần đã sử dụng trong trận đấu. Nếu đang được sử dụng trong trận đấu → banner cảnh báo và disable nút Xóa, chỉ có "Đóng". Ô "Lý do xóa" (bắt buộc, 10–200 ký tự, có đếm).
- **Footer:** "Hủy" + "Xóa câu hỏi" (danger). Disable "Xóa" khi lý do < 10 ký tự hoặc đang trong trận đấu.
- **Sau khi xóa:** Đóng dialog, toast thành công, danh sách/trang cập nhật.
