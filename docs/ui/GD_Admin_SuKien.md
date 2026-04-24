# GIAO DIỆN QUẢN LÝ SỰ KIỆN (ADMIN)

## 1. MỤC ĐÍCH

Giao diện dùng để xem và cập nhật các sự kiện đã tồn tại trong hệ thống.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Danh Sách Sự Kiện
- Hiển thị dạng card 2 cột
- Mỗi card gồm:
  - Ảnh đại diện theo loại event
  - Tên sự kiện, mô tả ngắn
  - Tag thời gian (`timeType`, thời gian còn lại)
  - Tag số lượng cấu phần (`Mức` với Quiz Milestone, `Vật phẩm` với Lucky Spin)
- Có nút `Chỉnh sửa` cho từng sự kiện

### 2.2. Modal Cập Nhật Sự Kiện
- Thông tin chung:
  - Tên, mô tả, ngày bắt đầu/kết thúc
  - Trạng thái khóa sự kiện (`isLocked`)
- Cấu hình theo từng loại:
  - `LuckySpin`:
    - `maxSpinTimePerDay`
    - Danh sách `spinItems` (phần thưởng, giá trị, tỉ lệ)
    - Hỗ trợ thêm/xóa item, random tỉ lệ
  - `QuizMilestoneChallenge`:
    - Danh sách `thresholds`
    - Mỗi ngưỡng gồm điểm exp, danh sách reward, danh sách câu hỏi thử thách
    - Hỗ trợ thêm/xóa ngưỡng, reward, câu hỏi

## 3. LUỒNG THAO TÁC

### 3.1. Tải dữ liệu
1. Khi mở trang, hệ thống tải:
   - Danh sách sự kiện
   - Danh sách reward
   - Danh sách câu hỏi (để chọn cho challenge)
2. Ẩn loại sự kiện `TournamentRewards` khỏi danh sách hiển thị.

### 3.2. Cập nhật sự kiện
1. Nhấn `Chỉnh sửa` tại card sự kiện.
2. Chỉnh thông tin chung và cấu hình theo loại.
3. Validate dữ liệu trước khi gửi.
4. Convert cấu hình event sang JSON (`eventConfigJsonData`) và gọi API cập nhật.

## 4. QUY TẮC NGHIỆP VỤ (VALIDATION ĐANG IMPLEMENT)
- Bắt buộc có tên, mô tả, ngày bắt đầu hợp lệ.
- Nếu có ngày kết thúc thì phải sau ngày bắt đầu.
- Với `LuckySpin`:
  - `maxSpinTimePerDay >= 1`
  - Có ít nhất 1 vật phẩm
  - Tỉ lệ mỗi item > 0 và tổng tỉ lệ phải bằng `100%`
- Với `QuizMilestoneChallenge`:
  - Có ít nhất 1 ngưỡng
  - Mỗi ngưỡng phải có exp >= 1, tối thiểu 1 reward, tối thiểu 1 câu hỏi

## 5. PHẠM VI HIỆN TẠI
- Chưa có chức năng tạo sự kiện mới từ UI.
- Chưa có chức năng kết thúc sự kiện sớm bằng action riêng.
