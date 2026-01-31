# 2.6. QUẢN LÝ CHỦ ĐỀ

## 1. NGHIỆP VỤ

### Mục đích
Admin quản lý các chủ đề câu hỏi (thêm, sửa, xóa).

### Luồng thêm chủ đề
1. "Quản lý chủ đề" → "Thêm chủ đề"
2. Nhập: Tên (bắt buộc, unique), Mô tả, Icon/ảnh đại diện, Màu sắc, Thứ tự hiển thị
3. Lưu → validate (tên 3–50 ký tự, không trùng; icon jpg/png max 500KB)
4. Thông báo thành công

### Luồng xóa chủ đề
1. Chọn chủ đề cần xóa
2. Kiểm tra: có câu hỏi không? có đang dùng không?
3. Nếu có câu hỏi: cảnh báo + chọn "Xóa tất cả câu hỏi" HOẶC "Chuyển câu hỏi sang chủ đề khác"
4. Nếu chuyển: chọn chủ đề đích → chuyển tất cả câu hỏi
5. Xóa chủ đề → thông báo thành công

### Quy tắc
- Không xóa chủ đề nếu có câu hỏi đang dùng trong trận đấu
- Chủ đề cần tối thiểu 10 câu đã duyệt mới dùng trong thi đấu

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang danh sách chủ đề

- **Header:** "Quản lý chủ đề", nút "Thêm chủ đề" (primary).
- **Nội dung:** Grid hoặc bảng: mỗi chủ đề có icon/ảnh đại diện, Tên, Mô tả (1 dòng), Số câu hỏi, Trạng thái (Active/Inactive), Thứ tự, Hành động (Sửa, Xóa). Màu chủ đề hiển thị (dot hoặc viền). Có thể kéo thả đổi thứ tự hoặc sửa số thứ tự. Empty state: "Chưa có chủ đề. Bấm Thêm chủ đề để tạo."

---

### 2.2. Form thêm / sửa chủ đề

- **Modal hoặc trang riêng.** Tiêu đề "Thêm chủ đề" / "Chỉnh sửa chủ đề".
- **Các trường:** Tên chủ đề (bắt buộc, 3–50 ký tự, lỗi trùng "Tên đã tồn tại"); Mô tả (tùy chọn); Icon/ảnh đại diện (upload jpg/png, max 500KB, có preview); Màu sắc (color picker hoặc dropdown); Thứ tự hiển thị (số, nhỏ = trước); Trạng thái Active/Inactive (khi sửa).
- **Footer:** "Hủy" + "Lưu" (primary). Sau khi lưu: đóng form, toast, cập nhật danh sách.

---

### 2.3. Dialog xóa chủ đề

- **Header:** Icon cảnh báo + tiêu đề "Xóa chủ đề".
- **Nội dung:** Hiển thị tên chủ đề và "Chủ đề [Tên] có X câu hỏi." Nếu X > 0: chọn "Xóa tất cả câu hỏi thuộc chủ đề" hoặc "Chuyển câu hỏi sang chủ đề khác" (kèm dropdown chọn chủ đề đích). Nếu X = 0: chỉ xác nhận xóa.
- **Footer:** "Hủy" + "Xóa" (danger). Disable "Xóa" khi chọn Chuyển mà chưa chọn chủ đề đích. Sau khi xóa: đóng dialog, toast, cập nhật danh sách.
