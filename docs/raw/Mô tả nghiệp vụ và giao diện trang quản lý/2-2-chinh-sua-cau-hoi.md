# 2.2. CHỈNH SỬA CÂU HỎI

## 1. NGHIỆP VỤ

### Mục đích
Admin/Editor chỉnh sửa câu hỏi đã có.

### Luồng xử lý
1. Tìm câu hỏi → "Chỉnh sửa"
2. Kiểm tra: đang dùng trong trận đấu? quyền sửa?
3. Nếu đang dùng: cảnh báo "Tạo bản sao để chỉnh sửa?" → nếu chọn: tạo câu hỏi mới từ nội dung cũ
4. Nếu không: Form chỉnh sửa (giống form thêm mới, đã điền sẵn) + hiển thị lịch sử chỉnh sửa
5. Lưu → validate; nếu đã duyệt thì chuyển về Pending (duyệt lại); lưu version cũ vào QuestionVersions; log
6. Thông báo thành công

### Quy tắc
- Câu hỏi đã duyệt khi sửa cần duyệt lại
- Version control: lưu phiên bản cũ, có thể khôi phục
- Đang dùng trong trận đấu → tạo bản sao thay vì sửa trực tiếp

### Validation
- Giống thêm mới; thêm kiểm tra quyền chỉnh sửa

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Giống trang "Thêm câu hỏi mới"** (cùng 4 tab, cùng form). Tiêu đề "Chỉnh sửa câu hỏi" + ID hoặc trích nội dung; các ô đã điền sẵn từ API. Header: breadcrumb (Quản lý câu hỏi > Chi tiết #ID > Chỉnh sửa), nút Hủy và Lưu.

---

### 2.2. Cảnh báo khi câu hỏi đang được sử dụng

- Banner phía trên form: "Câu hỏi này đang được sử dụng trong trận đấu. Bạn có thể tạo bản sao để chỉnh sửa mà không ảnh hưởng trận đấu hiện tại."
- Nút "Tạo bản sao và chỉnh sửa" (primary) → tạo câu hỏi mới từ nội dung hiện tại, chuyển sang form thêm mới đã điền sẵn.
- Nút "Chỉnh sửa trực tiếp" (secondary) → mở form chỉnh sửa (sau lưu câu hỏi chuyển Pending).

---

### 2.3. Form chỉnh sửa và Lịch sử

- Form giống Thêm câu hỏi (4 tab). Footer chỉ có "Lưu" (primary); có hoặc không "Lưu nháp" tùy quyền/trạng thái. Sau khi Lưu: toast và cập nhật trạng thái; nếu trước đó Approved thì thông báo "chuyển sang chờ duyệt lại".
- **Lịch sử chỉnh sửa:** Tab "Lịch sử" hoặc khối collapsible: danh sách phiên bản (mới nhất trước), mỗi dòng có "Xem/So sánh" và "Khôi phục phiên bản này" (có xác nhận).

---

### 2.4. Loading và lỗi

- Đang load: skeleton form hoặc spinner. Lỗi 404: banner + "Không tìm thấy câu hỏi." + "Quay lại danh sách". Lỗi lưu: toast lỗi.
