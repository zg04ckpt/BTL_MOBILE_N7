# GIAO DIỆN QUẢN LÝ CHỦ ĐỀ (ADMIN)

## 1. MỤC ĐÍCH

Giao diện cho phép quản trị viên xem danh sách chủ đề, tạo mới, đổi tên và xóa chủ đề.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header & Công Cụ
- Tiêu đề trang: `Quản lý chủ đề`
- Ô nhập tìm kiếm theo tên chủ đề (UI có sẵn, hiện chưa áp dụng filter thực tế)
- Nút `Thêm chủ đề`

### 2.2. Danh Sách Chủ Đề (Card Grid)
- Hiển thị card chủ đề theo lưới
- Thông tin mỗi card:
  - Tên chủ đề
  - `questionCount` (số câu hỏi)
  - Trạng thái hiển thị dạng nhãn `Active` (đang là UI tĩnh)
- Hành động: `Sửa` và `Xóa`

### 2.3. Modal Thêm / Sửa Chủ Đề
- Có 1 input tên chủ đề (`name`)
- Cùng một modal cho cả tạo mới và cập nhật
- Khi bấm lưu:
  - `mode=create`: gọi API tạo chủ đề mới
  - `mode=update`: gọi API cập nhật tên chủ đề

### 2.4. Dialog Xóa Chủ Đề
- Xác nhận xóa đơn giản qua popup
- Nếu đồng ý thì gọi API xóa trực tiếp

## 3. LUỒNG THAO TÁC

### 3.1. Thêm chủ đề
1. Nhấn `Thêm chủ đề`.
2. Nhập tên.
3. Nhấn lưu để tạo mới.
4. Tải lại danh sách sau khi thành công.

### 3.2. Sửa chủ đề
1. Nhấn icon `Sửa`.
2. Cập nhật tên chủ đề.
3. Lưu và refresh danh sách.

### 3.3. Xóa chủ đề
1. Nhấn icon `Xóa`.
2. Xác nhận trong popup.
3. Hệ thống xóa và tải lại danh sách.

## 4. QUY TẮC/PHẠM VI HIỆN TẠI
- UI hiện tại chỉ thao tác trường `name` cho create/update.
- Chưa có các trường nâng cao như mô tả, màu, icon, thứ tự hiển thị.
- Chưa có luồng chuyển câu hỏi sang chủ đề khác khi xóa trong frontend hiện tại.

## 5. RESPONSIVE
- Layout đang dùng grid card, ưu tiên desktop.
