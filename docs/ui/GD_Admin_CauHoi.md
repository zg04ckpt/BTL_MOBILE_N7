# GIAO DIỆN QUẢN LÝ CÂU HỎI (ADMIN)

## 1. MỤC ĐÍCH

Giao diện Quản lý câu hỏi dùng để thêm mới, chỉnh sửa, duyệt và quản lý ngân hàng câu hỏi của hệ thống.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header & Công Cụ
- Tiêu đề: "Ngân hàng câu hỏi"
- Nút "Thêm câu hỏi mới": Primary button
- Nút "Import Excel": Hỗ trợ nhập hàng loạt
- Nút "Duyệt câu hỏi": Dành cho các câu hỏi do user đóng góp (nếu có)

### 2.2. Bộ Lọc
- Chủ đề: Khoa học, Xã hội, Thể thao...
- Độ khó: Dễ, Trung bình, Khó
- Trạng thái: Đang hoạt động, Chờ duyệt, Tạm ẩn
- Tìm kiếm: Theo nội dung câu hỏi

### 2.3. Danh Sách Câu Hỏi (Data Table)
- Cột Nội dung: Hiển thị trích đoạn câu hỏi
- Cột Chủ đề
- Cột Độ khó (Màu sắc phân biệt)
- Cột Đáp án đúng
- Cột Người tạo
- Cột Trạng thái
- Cột Hành động: Sửa, Xóa, Ẩn/Hiện

### 2.4. Form Thêm/Sửa Câu Hỏi
- Trường Nội dung câu hỏi (Rich text editor hoặc Text area)
- Trường Upload hình ảnh/âm thanh đính kèm
- Dropdown Chủ đề
- Dropdown Độ khó
- 4 Trường Đáp án (A, B, C, D) có Radio button để chọn đáp án đúng
- Trường Giải thích đáp án (Optional)
- Trạng thái: Checkbox "Kích hoạt ngay"

## 3. LUỒNG THAO TÁC

### 3.1. Thêm Câu Hỏi Mới
1. Admin nhấn "Thêm câu hỏi mới"
2. Hiển thị Form thêm mới
3. Nhập đầy đủ thông tin: Nội dung, 4 đáp án, chọn đáp án đúng, phân loại
4. Nhấn "Lưu"
5. Validate dữ liệu -> Lưu vào database -> Thông báo thành công

### 3.2. Import Câu Hỏi
1. Admin nhấn "Import Excel"
2. Hiển thị Popup upload file
3. Chọn file theo mẫu quy định (.xlsx, .csv)
4. Hệ thống preview dữ liệu
5. Nhấn "Import" -> Hệ thống xử lý hàng loạt và báo cáo kết quả (Số dòng thành công, số dòng lỗi)

### 3.3. Duyệt Câu Hỏi (Nếu có quy trình duyệt)
1. Vào tab/bộ lọc "Chờ duyệt"
2. Xem nội dung câu hỏi
3. Nhấn "Duyệt" (Public) hoặc "Từ chối" (Kèm lý do)

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Tính Toàn Vẹn
- Một câu hỏi bắt buộc phải có 4 đáp án
- Bắt buộc phải có đúng 1 đáp án đúng
- Không được xóa câu hỏi đã xuất hiện trong các trận đấu log (chỉ được chuyển sang trạng thái Ẩn)

### 4.2. Media
- Ảnh upload phải được tối ưu kích thước
- Hỗ trợ định dạng JPG, PNG

## 5. RESPONSIVE
- Desktop: Hiển thị dạng bảng chi tiết, Form chia 2 cột.
- Mobile: Hiển thị dạng thẻ (Card), Form 1 cột dọc.
