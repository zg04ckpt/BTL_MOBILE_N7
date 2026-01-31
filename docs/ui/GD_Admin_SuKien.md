# GIAO DIỆN QUẢN LÝ SỰ KIỆN (ADMIN)

## 1. MỤC ĐÍCH

Giao diện Quản lý sự kiện cho phép tạo và cấu hình các sự kiện đặc biệt, giải đấu hoặc các chiến dịch khuyến mãi trong game.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Danh Sách Sự Kiện
- Hiển thị các sự kiện dưới dạng Cards hoặc List
- Thông tin: Banner, Tên sự kiện, Thời gian (Bắt đầu - Kết thúc), Trạng thái (Sắp diễn ra, Đang chạy, Đã kết thúc)
- Toggle bật/tắt nhanh sự kiện

### 2.2. Form Tạo/Sửa Sự Kiện
- Tab Thông tin chung:
  - Tên sự kiện
  - Mô tả
  - Banner
  - Thời gian hiệu lực
- Tab Cấu hình luật chơi:
  - Bộ câu hỏi áp dụng (Chọn theo Chủ đề hoặc Tag)
  - Số lượng câu hỏi mỗi lượt
  - Thời gian trả lời
- Tab Phần thưởng:
  - Cấu hình các mốc điểm và phần thưởng tương ứng
  - Quà top bảng xếp hạng sự kiện

## 3. LUỒNG THAO TÁC

### 3.1. Tạo Sự Kiện Mới
1. Nhấn "Tạo sự kiện"
2. Điền thông tin, upload banner
3. Thiết lập thời gian chạy (Ví dụ: Từ 01/01 đến 07/01)
4. Cấu hình phần thưởng
5. Nhấn "Lưu lại" -> Trạng thái chuyển thành "Lên lịch"

### 3.2. Kết Thúc Sự Kiện Sớm
1. Chọn sự kiện đang chạy
2. Nhấn "Kết thúc ngay"
3. Hệ thống ngừng cho phép user tham gia, tiến hành tổng kết trao giải (nếu có)

## 4. QUY TẮC NGHIỆP VỤ
- Không được sửa đổi cấu hình luật chơi của sự kiện đang diễn ra để đảm bảo công bằng.
- Banner phải đúng kích thước quy định để hiển thị đẹp trên App.
