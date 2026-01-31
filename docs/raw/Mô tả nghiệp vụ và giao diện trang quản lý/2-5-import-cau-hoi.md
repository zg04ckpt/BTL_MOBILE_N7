# 2.5. IMPORT CÂU HỎI TỪ EXCEL/CSV

## 1. NGHIỆP VỤ

### Mục đích
Import hàng loạt câu hỏi từ file Excel/CSV.

### Luồng xử lý
1. Vào "Import câu hỏi"
2. "Download template" → điền dữ liệu (mỗi dòng 1 câu, đủ cột)
3. Upload file → validate: định dạng, cấu trúc, từng dòng
4. Preview: danh sách câu hỏi sẽ import, dòng lỗi đánh dấu đỏ, hiển thị lỗi cụ thể
5. Chọn: "Import tất cả" (bỏ qua dòng lỗi) / "Import chỉ dòng hợp lệ" / "Hủy" sửa file
6. Import → tạo câu hỏi Pending, log, báo cáo (tổng dòng, thành công, lỗi, chi tiết lỗi)
7. Hiển thị báo cáo kết quả

### Quy tắc
- File tối đa 1000 câu/lần, max 10MB
- Import mặc định Pending
- Dòng lỗi bỏ qua, không làm gián đoạn

### Validation
- File: xlsx, csv; mỗi dòng đủ cột; nội dung 10–500 ký tự; đáp án đúng A/B/C/D hoặc A,B,C

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang "Import câu hỏi" (theo bước)

- **Header:** "Import câu hỏi", mô tả ngắn: tải template, điền dữ liệu, upload file.
- **Bước 1 – Tải template:** Nút "Download template" tải file .xlsx/.csv mẫu (đúng cột: Nội dung, Chủ đề, Mức độ khó, Điểm số, Thời gian, Đáp án A–D, Đáp án đúng, Giải thích). Ghi chú: mỗi dòng = 1 câu hỏi; đáp án đúng A,B,C,D hoặc A,B,C nếu nhiều.
- **Bước 2 – Upload:** Vùng kéo thả hoặc chọn file (.xlsx, .csv, max 10MB). Sau khi chọn: hiển thị tên file, kích thước, nút "Xóa" để chọn lại. Nút "Kiểm tra file" (hoặc tự động sau upload) → chuyển bước 3.

---

### 2.2. Bước 3 – Preview và quyết định import

- **Kết quả kiểm tra:** Tóm tắt "Tổng X dòng. Hợp lệ Y. Lỗi Z." Bảng preview: STT, Nội dung (trích), Chủ đề, Mức độ khó, Điểm, Thời gian, Đáp án đúng, Trạng thái (Hợp lệ/Lỗi), Chi tiết lỗi. Hàng lỗi nổi bật (nền hoặc viền đỏ), cột Lỗi hiển thị text lỗi.
- **Nút:** "Import" (primary, import dòng hợp lệ), "Hủy và chọn file khác" (secondary). Khi không có dòng hợp lệ: disable Import, thông báo "Vui lòng sửa file và upload lại."

---

### 2.3. Bước 4 – Kết quả sau import

- **Tóm tắt:** "Đã import thành công X câu hỏi."; nếu có lỗi: "Lỗi Y dòng" + chi tiết lỗi (collapsible). Nút "Import thêm" (về bước 2), "Xem danh sách câu hỏi". Loading khi đang import: overlay hoặc spinner + "Đang import...".
