# GIAO DIỆN THI ĐẤU VƯỢT ẢI (SOLO)

## 1. MỤC ĐÍCH

Giao diện Thi đấu vượt ải cho phép người chơi trả lời lần lượt các câu hỏi để tích lũy điểm và qua màn, không chịu áp lực từ đối thủ trực tiếp.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Thông tin người chơi: Avatar nhỏ, Tên hiển thị
- Thanh tiến độ: Hiển thị số câu đã hoàn thành trên tổng số câu (Ví dụ: 3/10)
- Điểm số hiện tại: Hiển thị số điểm tích lũy được trong lượt chơi này
- Nút Tạm dừng (Pause): Cho phép tạm dừng game (tùy cấu hình) hoặc Thoát

### 2.2. Phần Bộ Đếm Thời Gian
- Dạng hiển thị: Thanh thời gian (Progress bar) đếm ngược hoặc Số đếm giây
- Hiệu ứng: Chuyển màu từ Xanh -> Vàng -> Đỏ khi sắp hết giờ
- Âm thanh: Tiếng tích tắc gấp gáp khi còn 5 giây cuối

### 2.3. Khu Vực Câu Hỏi
- Nội dung câu hỏi: Text hiển thị rõ ràng, font chữ lớn
- Hình ảnh/Video minh họa (Nếu có): Hiển thị dưới nội dung câu hỏi, có thể phóng to

### 2.4. Khu Vực Câu Trả Lời
- Danh sách 4 đáp án: A, B, C, D
- Layout: 
  - Desktop: 2 hàng 2 cột
  - Mobile: Danh sách dọc
- Trạng thái chưa chọn: Nền màu mặc định (Trắng hoặc Xám nhạt)
- Trạng thái đã chọn:
  - Khi người dùng nhấn chọn: Đổi màu nền sang màu vàng/cam (Wait state)
- Trạng thái kết quả (Sau khi chốt):
  - Đáp án đúng: Nền màu xanh lá
  - Đáp án sai (Người dùng chọn): Nền màu đỏ
  - Đáp án sai (Người dùng không chọn): Mờ đi hoặc giữ nguyên

### 2.5. Phần Trợ Giúp (Quyền Trợ Giúp)
- 50/50: Loại bỏ 2 phương án sai
- Gọi điện thoại cho người thân (Giả lập): Gợi ý đáp án có xác suất đúng cao
- Hỏi ý kiến khán giả: Biểu đồ phần trăm lựa chọn
- Đổi câu hỏi: Chuyển sang câu hỏi khác
- Hiển thị: Các icon ở dưới cùng màn hình hoặc bên cạnh câu hỏi

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Trả Lời
1. Hệ thống hiển thị câu hỏi và 4 đáp án
2. Thời gian đếm ngược bắt đầu
3. Người dùng chọn 1 đáp án:
   - Nếu chưa hết giờ: Ghi nhận đáp án, khóa các đáp án còn lại
   - Nếu hết giờ: Coi như trả lời sai hoặc bỏ qua
4. Hệ thống hiển thị kết quả ngay lập tức:
   - Đúng: Hiệu ứng chúc mừng, cộng điểm, chuyển sang câu tiếp theo sau 2 giây
   - Sai: Hiệu ứng báo sai, hiển thị đáp án đúng, kết thúc lượt chơi (hoặc trừ mạng tùy mode)

### 3.2. Luồng Sử Dụng Trợ Giúp
1. Người dùng nhấn vào icon trợ giúp (khi chưa trả lời)
2. Hệ thống thực hiện logic trợ giúp:
   - 50/50: Ẩn đi 2 đáp án sai
   - Đổi câu hỏi: Thay thế nội dung câu hỏi mới
3. Disable quyền trợ giúp đã sử dụng

### 3.3. Luồng Kết Thúc
1. Hoàn thành tất cả câu hỏi:
   - Chuyển đến Giao diện Kết quả
   - Hiển thị tổng điểm và phần thưởng
2. Trả lời sai (Chế độ Survival):
   - Chuyển đến Giao diện Kết quả
   - Hiển thị điểm dừng cuộc chơi

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Cách Tính Điểm
- Trả lời đúng: +10 điểm (cơ bản)
- Bonus thời gian: Trả lời càng nhanh điểm càng cao (Ví dụ: + (Thời gian còn lại / Tổng thời gian) * 5)
- Combo: Trả lời đúng liên tiếp X câu -> Nhân hệ số điểm (x1.2, x1.5)

### 4.2. Thời Gian
- Mỗi câu hỏi có 15-30 giây suy nghĩ (tùy cấu hình)
- Hết giờ: Tự động xử lý là "Không trả lời" (tương đương Sai)

### 4.3. Xử Lý Mất Mạng
- Chế độ tích điểm: Sai không bị dừng, tiếp tục câu sau
- Chế độ Survival: Sai 1 câu là dừng cuộc chơi ngay lập tức

## 5. RESPONSIVE

### 5.1. Desktop
- Layout trung tâm
- Câu hỏi và đáp án nằm gọn trong khung nhìn
- Các quyền trợ giúp nằm bên phải hoặc dưới

### 5.2. Mobile
- Tận dụng toàn bộ màn hình
- Câu hỏi ở trên, đáp án ở dưới chiếm phần lớn diện tích để dễ bấm
- Các nút trợ giúp nhỏ gọn ở dưới cùng
