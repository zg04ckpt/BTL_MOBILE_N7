# GIAO DIỆN CẤU HÌNH THI ĐẤU

## 1. MỤC ĐÍCH

Giao diện Cấu hình thi đấu cho phép người chơi lựa chọn các thiết lập trước khi bắt đầu thi đấu bao gồm kiểu thi đấu, số lượng người chơi và chủ đề câu hỏi.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header

- Tiêu đề: "Lựa chọn nội dung thi đấu"
- Nút Quay lại: Quay về Giao diện Trang chủ

### 2.2. Khu Vực Lựa Chọn Kiểu Thi Đấu

- Tiêu đề khu vực: "Kiểu thi đấu"
- Danh sách kiểu thi đấu:
  - Kiểu hiển thị: Lưới 2x1 (2 cột, 1 hàng)
  - Các lựa chọn:
    - Vượt ải đơn:
      - Text: "Đấu đơn vượt ải"
      - Mô tả ngắn: "Thi đấu một mình, không giới hạn thời gian"
    - Ghép trận:
      - Text: "Đấu đội"
      - Mô tả ngắn: "Thi đấu với người chơi khác"
- Trạng thái lựa chọn: Đổi màu nền đậm hơn và highlight viền khi được chọn

### 2.3. Khu Vực Lựa Chọn Số Lượng Người Chơi

- Tiêu đề khu vực: "Số lượng người chơi"
- Danh sách số lượng:
  - Kiểu hiển thị: Lưới 2x2 (2 cột, 2 hàng)
  - Các lựa chọn: 1, 2, 5, 10 người
  - Quy tắc:
    - Tự động khóa chọn "1" khi lựa chọn "Đấu đơn vượt ải"
    - Chỉ cho phép chọn 2, 5, 10 khi lựa chọn "Đấu đội"
- Trạng thái lựa chọn: Nổi bật nền và highlight viền khi được chọn (Selected state)
- Trạng thái vô hiệu hóa: Mờ đi (Disabled), không cho nhấn khi không hợp lệ

### 2.4. Khu Vực Lựa Chọn Chủ Đề

- Tiêu đề khu vực: "Lựa chọn chủ đề"
- Danh sách chủ đề:
  - Kiểu hiển thị: Lưới 2xN (2 cột, N hàng)
  - Các lựa chọn mặc định:
    - Hỗn hợp: Câu hỏi từ nhiều chủ đề khác nhau
    - Ngẫu nhiên: Một chủ đề được chọn ngẫu nhiên bởi hệ thống
  - Các chủ đề cụ thể: Khoa học, Lịch sử, Văn học, Địa lý, v.v.
- Trạng thái lựa chọn: Đổi màu nền đậm hơn và highlight viền khi được chọn
- Giao diện có thể cuộn: Cho phép hiển thị nhiều chủ đề

### 2.5. Phần Nút Xác Nhận

- Nút Xác nhận:
  - Text: "Xác nhận"
  - Kiểu: Primary button
  - Vị trí: Dưới cùng, kéo dài hết chiều ngang màn hình
  - Trạng thái: Chỉ active khi đã lựa chọn đầy đủ các thông tin

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Cấu Hình Và Bắt Đầu

1. Người dùng vào Giao diện Cấu hình thi đấu
2. Người dùng chọn Kiểu thi đấu:
   - Nếu chọn "Vượt ải đơn": Tự động đặt "Số lượng người chơi" = 1 và khóa
   - Nếu chọn "Ghép trận": Mở khóa lựa chọn 2, 5, 10 người
3. Người dùng chọn Số lượng người chơi (nếu là "Ghép trận")
4. Người dùng chọn Chủ đề câu hỏi
5. Người dùng nhấn nút "Xác nhận"
6. Hệ thống hiển thị Bottom Sheet Dialog xác nhận:
   - Tiêu đề: "Xác nhận trước khi bắt đầu"
   - Thông tin tổng hợp:
     - Kiểu thi đấu: Hiển thị kiểu đã chọn
     - Số lượng người: Hiển thị số lượng
     - Chủ đề: Hiển thị chủ đề đã chọn
     - Danh sách bonus: Ví dụ "+10 điểm xếp hạng", "X2 điểm xếp hạng"
     - Quy tắc chơi: Tóm tắt quy tắc ngắn gọn
   - Nút "Bắt đầu ngay": Xác nhận và bắt đầu
   - Nút "Đóng" hoặc tap ngoài để đóng dialog
7. Người dùng nhấn "Bắt đầu ngay":
   - Nếu chọn "Vượt ải đơn": Chuyển đến Giao diện Thi đấu vượt ải
   - Nếu chọn "Ghép trận": Chuyển đến Giao diện Ghép trận

### 3.2. Luồng Quay Lại

1. Người dùng nhấn nút "Quay lại"
2. Hệ thống quay về Giao diện Trang chủ

### 3.3. Luồng Thay Đổi Lựa Chọn

1. Người dùng có thể thay đổi bất kỳ lựa chọn nào bất cứ lúc nào
2. Hệ thống cập nhật trạng thái các lựa chọn tương ứng:
   - Khi đổi kiểu thi đấu: Cập nhật trạng thái khu vực Số lượng người chơi
   - Khi đổi chủ đề: Cập nhật thông tin bonus tương ứng

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Ràng Buộc Lựa Chọn

- Vượt ải đơn: Chỉ cho phép 1 người chơi
- Ghép trận: Chỉ cho phép 2, 5 hoặc 10 người chơi
- Bắt buộc chọn đầy đủ: Kiểu thi đấu, Số lượng người chơi, Chủ đề

### 4.2. Điểm Thưởng

- Chủ đề "Hỗn hợp" hoặc "Ngẫu nhiên": Nhận X2 điểm xếp hạng
- Các chủ đề cụ thể: Điểm xếp hạng bình thường
- Điểm thưởng có thể điều chỉnh theo chính sách

### 4.3. Validation

- Kiểm tra người dùng đã lựa chọn đầy đủ các thông tin trước khi cho phép nhấn "Xác nhận"
- Kiểm tra chủ đề có đủ số lượng câu hỏi (tối thiểu 10 câu đã duyệt)
