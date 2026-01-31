# GIAO DIỆN THI ĐẤU GHÉP TRẬN (MULTIPLAYER)

## 1. MỤC ĐÍCH

Giao diện Thi đấu ghép trận cho phép nhiều người chơi cùng trả lời một bộ câu hỏi trong thời gian thực, so sánh điểm số trực tiếp sau mỗi câu.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Thông tin Vòng đấu: Ví dụ "Câu hỏi 3/10"
- Đồng hồ đếm ngược chung: Hiển thị thời gian còn lại cho câu hỏi hiện tại
- Nút Thoát: Rời khỏi trận đấu (bị xử thua)

### 2.2. Khu Vực Người Chơi (Bảng Xếp Hạng Mini)
- Hiển thị danh sách người chơi tham gia (2, 5, hoặc 10 người)
- Thông tin mỗi người:
  - Avatar, Tên
  - Điểm số hiện tại
  - Trạng thái trả lời (Icon):
    - Đang suy nghĩ (Thinking...)
    - Đã trả lời (Checkmark ẩn)
    - Đúng (Màu xanh - hiện sau khi hết giờ)
    - Sai (Màu đỏ - hiện sau khi hết giờ)
- Vị trí:
  - Desktop: Sidebar bên trái hoặc phải
  - Mobile: Thanh ngang trượt được ở trên cùng (Top bar)

### 2.3. Khu Vực Câu Hỏi & Trả Lời
- Tương tự Giao diện Vượt ải đơn:
  - Nội dung câu hỏi
  - Hình ảnh minh họa
  - 4 đáp án A, B, C, D

### 2.4. Phần Chat/Emoji (Tùy chọn)
- Các biểu tượng cảm xúc nhanh: Haha, Wow, Buồn, Tức giận
- Gửi tin nhắn nhanh (Preset messages): "Nhanh lên nào!", "Khó quá!", "May mắn nhé!"

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Bắt Đầu Câu Hỏi
1. Hiển thị thông báo "Câu hỏi số X" trong 2-3 giây
2. Hiển thị nội dung câu hỏi và đáp án
3. Bắt đầu đếm ngược thời gian (Đồng bộ giữa các người chơi)

### 3.2. Luồng Trong Khi Trả Lời
1. Người chơi chọn đáp án:
   - Khóa thao tác chọn lại
   - Hiển thị trạng thái "Đã trả lời" trên bảng xếp hạng mini của bản thân
   - Gửi tín hiệu lên server
2. Các đối thủ khác trả lời:
   - Cập nhật trạng thái "Đã trả lời" của đối thủ tương ứng
   - Không hiển thị họ chọn đúng hay sai ngay lập tức

### 3.3. Luồng Kết Thúc Câu Hỏi (Hết giờ)
1. Khi đồng hồ đếm ngược về 0 hoặc tất cả đã trả lời:
2. Server gửi kết quả về client
3. Hiển thị đáp án đúng/sai trên màn hình của người chơi
4. Cập nhật trạng thái Đúng/Sai cho tất cả người chơi trên bảng xếp hạng mini
5. Cập nhật và sắp xếp lại Bảng xếp hạng mini dựa trên tổng điểm mới
6. Chờ 3-5 giây trước khi sang câu tiếp theo

### 3.4. Luồng Kết Thúc Trận Đấu
1. Sau khi hoàn thành câu hỏi cuối cùng
2. Tổng kết điểm số cuối cùng
3. Chuyển sang Giao diện Kết quả trận đấu

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Đồng Bộ Realtime
- Sử dụng WebSocket để đảm bảo thời gian và trạng thái được đồng bộ
- Xử lý độ trễ mạng (Lag compensation)

### 4.2. Tính Điểm Đối Kháng
- Trả lời đúng: Điểm cơ bản + Điểm tốc độ
- Trả lời sai: 0 điểm (hoặc âm điểm nếu luật cho phép)
- Điểm thưởng Top 1 mỗi câu (nếu có): Người trả lời đúng nhanh nhất được bonus

### 4.3. Xử Lý Thoát Trận (AFK/Disconnect)
- Nếu người chơi thoát giữa chừng: Avatar mờ đi, trạng thái "Mất kết nối"
- Vẫn tính là tham gia, điểm số giữ nguyên tại thời điểm thoát
- Tự động đánh dấu là Sai cho các câu hỏi còn lại

## 5. RESPONSIVE

### 5.1. Desktop
- Layout 2 cột:
  - Cột 1 (20-30%): Danh sách người chơi xếp dọc
  - Cột 2 (70-80%): Khu vực câu hỏi và đáp án trung tâm

### 5.2. Mobile
- Khu vực người chơi thu gọn thành thanh ngang ở trên, chỉ hiện Avatar và trạng thái màu sắc
- Câu hỏi và đáp án chiếm phần còn lại
- Hiệu ứng chuyển động (Sắp xếp lại vị trí avatar khi điểm thay đổi)
