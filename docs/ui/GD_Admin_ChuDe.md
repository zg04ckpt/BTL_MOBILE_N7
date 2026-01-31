# GIAO DIỆN QUẢN LÝ CHỦ ĐỀ (ADMIN)

## 1. MỤC ĐÍCH

Giao diện Quản lý chủ đề cho phép Admin thêm, sửa, xóa và sắp xếp các chủ đề câu hỏi trong hệ thống.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header & Công Cụ
- Tiêu đề trang: "Quản lý chủ đề"
- Nút "Thêm chủ đề": Primary button, icon dấu cộng.

### 2.2. Danh Sách Chủ Đề (Grid View hoặc List View)
- Hiển thị danh sách các card chủ đề. Mỗi card gồm:
  - Icon/Ảnh đại diện chủ đề.
  - Tên chủ đề (Ví dụ: Khoa học, Lịch sử).
  - Mô tả ngắn (1 dòng).
  - Số lượng câu hỏi hiện có.
  - Trạng thái: Active (Xanh) / Inactive (Xám).
  - Màu sắc nhận diện (Dot màu hoặc Viền).
  - Hành động: Sửa (Icon bút), Xóa (Icon thùng rác).
- Empty State: "Chưa có chủ đề nào. Bấm Thêm chủ đề để tạo mới."

### 2.3. Modal Thêm / Sửa Chủ Đề
- Tiêu đề: "Thêm chủ đề mới" hoặc "Chỉnh sửa chủ đề".
- Form nhập liệu:
  - Tên chủ đề: Input text (Bắt buộc, 3-50 ký tự, Unique).
  - Mô tả: Textarea (Tùy chọn).
  - Icon/Ảnh đại diện: Upload file (JPG/PNG, max 500KB), có Preview.
  - Màu sắc: Color picker hoặc Dropdown chọn màu.
  - Thứ tự hiển thị: Input number (Nhỏ hiển thị trước).
  - Trạng thái: Switch Active/Inactive (Chỉ hiện khi chỉnh sửa).
- Footer:
  - Nút "Hủy": Đóng modal.
  - Nút "Lưu": Validate và lưu dữ liệu.

### 2.4. Dialog Xóa Chủ Đề
- Tiêu đề: "Xóa chủ đề".
- Nội dung:
  - Cảnh báo: "Chủ đề [Tên] có [X] câu hỏi."
  - Lựa chọn (Nếu X > 0):
    - Radio 1: "Xóa tất cả câu hỏi thuộc chủ đề này" (Cảnh báo đỏ).
    - Radio 2: "Chuyển câu hỏi sang chủ đề khác" (Hiện Dropdown chọn chủ đề đích).
- Footer:
  - Nút "Hủy".
  - Nút "Xóa": Style Danger. Disable nếu chọn Radio 2 mà chưa chọn chủ đề đích.

## 3. LUỒNG THAO TÁC

### 3.1. Thêm Chủ Đề
1. Nhấn nút "Thêm chủ đề".
2. Điền đầy đủ thông tin: Tên, Mô tả, Chọn ảnh, Chọn màu.
3. Nhấn "Lưu".
4. Hệ thống kiểm tra trùng tên -> Lưu -> Thông báo "Thêm thành công" -> Cập nhật danh sách.

### 3.2. Sửa Chủ Đề
1. Nhấn icon "Sửa" trên card chủ đề.
2. Sửa thông tin (Ví dụ: Ẩn chủ đề bằng cách tắt Active).
3. Nhấn "Lưu".

### 3.3. Xóa Chủ Đề
1. Nhấn icon "Xóa".
2. Hệ thống kiểm tra số lượng câu hỏi trong chủ đề.
3. Nếu có câu hỏi: Admin chọn phương án (Xóa hết hoặc Chuyển).
4. Nhấn xác nhận "Xóa".
5. Hệ thống thực hiện xóa và cập nhật lại dữ liệu.

## 4. QUY TẮC NGHIỆP VỤ
- Tên chủ đề là duy nhất, không phân biệt hoa thường.
- Không xóa được chủ đề đang có trận đấu diễn ra (nếu hệ thống có check realtime).
- Chủ đề cần tối thiểu 10 câu hỏi đã duyệt (Approved) mới có thể được sử dụng trong cấu hình thi đấu.

## 5. RESPONSIVE
- **Desktop:** Hiển thị dạng Grid nhiều cột (3-4 cột).
- **Mobile:** Hiển thị dạng List dọc hoặc Grid 1-2 cột. Modal full màn hình.
