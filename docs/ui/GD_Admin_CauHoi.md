# GIAO DIỆN QUẢN LÝ CÂU HỎI (ADMIN)

## 1. MỤC ĐÍCH

Giao diện dùng để tìm kiếm, lọc, cập nhật, xóa câu hỏi và import hàng loạt bằng Excel.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header & Công Cụ
- Tiêu đề: `Quản lý câu hỏi`
- Nút `Thêm câu hỏi bằng Excel`

### 2.2. Bộ Lọc
- Tìm kiếm theo nội dung câu hỏi
- Lọc theo chủ đề (`topic`)
- Lọc theo cấp độ (`Dễ/Trung bình/Khó`)
- Lọc theo trạng thái (`Đang chỉnh sửa/Đang đợi duyệt/Đã duyệt/Đã từ chối`)

### 2.3. Danh Sách Câu Hỏi (Data Table)
- Hỗ trợ chọn nhiều dòng (`row selection`) để xóa hàng loạt
- Các cột: `ID`, `Nội dung`, `Chủ đề`, `Level`, `Kiểu câu hỏi`, `Trạng thái`, `Hành động`
- Hành động từng dòng: `Sửa`, `Xóa`
- Có phân trang và đổi số bản ghi/trang

### 2.4. Modal Cập Nhật Câu Hỏi
- Nội dung câu hỏi (`textarea`)
- Chủ đề, Level, Kiểu câu hỏi, Trạng thái
- Danh sách `Câu trả lời đúng` (tags)
- Danh sách `Các câu trả lời` (tags)
- Upload media: ảnh, audio, video (có preview và nút xóa media hiện tại)

### 2.5. Modal Import Excel
- Upload 1 file `.xlsx/.xls`
- Parse dữ liệu trực tiếp ở client, map sang payload tạo nhiều câu hỏi
- Cấu trúc cột đang dùng:
  - A: Nội dung câu hỏi
  - B: `TopicId`
  - C: `Level`
  - D: `Type`
  - E: `CorrectAnswers` (ngăn cách `;`)
  - F: `StringAnswers` (ngăn cách `;`)

## 3. LUỒNG THAO TÁC

### 3.1. Tìm kiếm/lọc danh sách
1. Nhập từ khóa hoặc chọn bộ lọc.
2. Hệ thống gọi API lấy danh sách theo điều kiện.
3. Bảng cập nhật theo kết quả.

### 3.2. Import câu hỏi
1. Admin nhấn `Thêm câu hỏi bằng Excel`.
2. Hiển thị Popup upload file
3. Chọn file `.xlsx/.xls`.
4. Hệ thống parse file, bỏ qua dòng không hợp lệ (thiếu nội dung hoặc thiếu topicId).
5. Gọi API tạo bulk và hiển thị thông báo kết quả.

### 3.3. Cập nhật/xóa câu hỏi
1. Chọn `Sửa` để mở modal cập nhật.
2. Validate dữ liệu bắt buộc rồi lưu.
3. Chọn `Xóa` để xóa 1 câu hỏi, hoặc chọn nhiều dòng để xóa hàng loạt.

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Validation phía UI đang có
- Bắt buộc: nội dung câu hỏi, chủ đề, kiểu, cấp độ
- Bắt buộc có ít nhất 1 đáp án đúng và 1 đáp án trong danh sách câu trả lời
- Trước khi xóa có popup xác nhận

### 4.2. Phạm vi hiện tại
- Chưa có màn riêng để `thêm thủ công` từng câu hỏi (chỉ có sửa câu hỏi hiện có và import Excel).
- Chưa có flow duyệt/từ chối tách riêng theo vai trò.

## 5. RESPONSIVE
- Thiết kế hiện tại tối ưu cho desktop (bảng dữ liệu rộng + modal chỉnh sửa).
