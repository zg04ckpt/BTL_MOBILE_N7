# GIAO DIỆN GHÉP TRẬN

## 1. MỤC ĐÍCH

Giao diện Ghép trận hiển thị trạng thái chờ ghép trận, tìm kiếm đối thủ phù hợp và hiển thị danh sách người chơi đã tham gia phòng.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Tiêu đề: "Đang ghép trận"
- Vị trí: Chính giữa phía trên

### 2.2. Phần Bộ Đếm Thời Gian
- Text hiển thị: Số giây đã trôi qua kể từ khi bắt đầu chờ
- Định dạng: "00:00:15" (phút:giây)
- Logo hoặc Animation động: Hiệu ứng quay tròn hoặc animation tìm kiếm
- Vị trí: Chính giữa màn hình, nổi bật

### 2.3. Phần Trạng Thái Ghép Trận
- Thông tin số lượng:
  - Text: "Đã ghép được X / Y người"
  - Ví dụ: "Đã ghép được 2 / 5 người"
  - Vị trí: Dưới bộ đếm thời gian
  
- Danh sách người chơi đã ghép:
  - Kiểu hiển thị: Danh sách dọc hoặc lưới ngang
  - Mỗi người chơi bao gồm:
    - Số thứ tự (STT)
    - Avatar người chơi
    - Tên hiển thị
    - Level người chơi
  - Người chơi chưa ghép: Hiển thị ô trống hoặc dấu chấm hỏi

### 2.4. Phần Nút Chức Năng
- Nút Hủy ghép trận:
  - Text: "Hủy ghép trận"
  - Kiểu: Secondary button hoặc Danger button
  - Vị trí: Dưới cùng màn hình

### 2.5. Dialog Xác Nhận Hủy
- Kiểu hiển thị: Bottom Sheet Dialog
- Nội dung:
  - Tiêu đề: "Xác nhận hủy ghép trận"
  - Text: "Bạn có chắc chắn muốn hủy ghép trận?"
  - Nút "Đóng": Đóng dialog, tiếp tục chờ
  - Nút "Xác nhận": Xác nhận hủy, quay lại Giao diện Cấu hình

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Chờ Ghép Trận
1. Người dùng vào Giao diện Ghép trận sau khi xác nhận cấu hình
2. Hệ thống bắt đầu tìm kiếm đối thủ:
   - Gửi request lên server với thông tin: ELO, chủ đề, số lượng người
   - Server đưa người dùng vào hàng đợi (Queue)
3. Bộ đếm thời gian bắt đầu chạy từ 0
4. Khi có người chơi mới được ghép:
   - Cập nhật số lượng "Đã ghép được"
   - Thêm thông tin người chơi vào danh sách
   - Hiển thị animation thêm người
5. Khi đủ số lượng người chơi:
   - Hệ thống tự động chuyển đến Giao diện Thi đấu ghép trận sau 3-5 giây
   - Hiển thị thông báo "Tìm thấy đủ đối thủ, chuẩn bị bắt đầu"

### 3.2. Luồng Hủy Ghép Trận
1. Người dùng nhấn nút "Hủy ghép trận"
2. Hệ thống hiển thị Dialog xác nhận
3. Người dùng chọn:
   - "Đóng": Đóng dialog, tiếp tục chờ ghép trận
   - "Xác nhận": 
     - Gửi request hủy đến server
     - Server xóa người dùng khỏi hàng đợi
     - Chuyển về Giao diện Cấu hình thi đấu

### 3.3. Luồng Timeout
1. Sau 30 giây vẫn chưa tìm thấy đủ đối thủ:
   - Hiển thị popup "Không tìm thấy đối thủ phù hợp"
   - Gợi ý: "Bạn có muốn thi đấu với Bot không?"
   - Nút "Thi đấu với Bot": Bắt đầu thi đấu với AI
   - Nút "Tiếp tục chờ": Mở rộng khoảng tìm kiếm
   - Nút "Hủy": Quay về Giao diện Cấu hình

### 3.4. Luồng Mất Kết Nối
1. Người dùng mất kết nối trong quá trình chờ:
   - Hiển thị thông báo "Mất kết nối mạng"
   - Nút "Thử lại": Kết nối lại và tiếp tục chờ
   - Nút "Hủy": Quay về Giao diện Cấu hình
2. Nếu kết nối lại thành công:
   - Kiểm tra trạng thái hàng đợi
   - Tiếp tục hiển thị danh sách người chơi

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Thuật Toán Ghép Cặp
- Dựa trên điểm ELO hoặc Rank Point: Tìm trong khoảng +/- 100 điểm
- Mở rộng tìm kiếm: Sau 10 giây, mở rộng khoảng chênh lệch điểm
- Chủ đề khớp:
  - "Hỗn hợp" ghép với "Hỗn hợp"
  - "Ngẫu nhiên" ghép với "Ngẫu nhiên"
  - Chủ đề cụ thể ghép với cùng chủ đề
- Timeout: Sau 30 giây chưa tìm thấy, gợi ý Bot

### 4.2. Trạng Thái Sẵn Sàng
- Khi ghép thành công: Tự động bắt đầu sau 5 giây đếm ngược
- Pre-loading: Tải xong 100% tài nguyên (ảnh, audio) trước khi bắt đầu
- Kiểm tra kết nối: Kiểm tra tất cả người chơi đều online và sẵn sàng

### 4.3. Xử Lý Lỗi
- Người chơi hủy: Cập nhật danh sách và số lượng, tiếp tục tìm
- Server lỗi: Hiển thị thông báo và cho phép thử lại
- Timeout: Hiển thị các lựa chọn thay thế


