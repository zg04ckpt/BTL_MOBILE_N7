## Mô tả giao diện thi đấu vượt ải đơn
- Giao diện này hiển thị các nội dung thi đấu cho một người chơi duy nhất
- Số lượng câu hỏi không giới hạn, thời gian thi đấu không giới hạn
- Không thể quay lại làm câu trước đó
- Nội dung có thể scroll

### Luồng xử lý chính
- Đếm ngược 3s -> Hiển thị nội dung + Đáp án -> Lựa chọn đáp án -> Ấn câu tiếp theo
- Mở menu hành động -> Ấn nút Nộp bài -> Hiển thị xác nhận -> GD hiển thị kết quả
- Mở menu hành động -> Ấn nút Hướng dẫn -> Hiển thị hướng dẫn -> Ấn đóng

### Các thành phần UI
- Điếm ngược 3s: Lớp phủ mờ toàn màn hình + Bộ đếm ngược 3 -> 2 -> 1

- Thanh tiến trình (Cố định trên top, Ko bị thay đổi khi cuộn):
    - Text: Số lượng câu đã làm 
    - Text: Số lượng câu đúng
    - Text: Điểm (Tạm thời mỗi câu 10 điểm)
    - Text: Thời gian đã trôi qua: VD 01h 21p 32s
    - Button: Mở menu hành động

- Nội dung câu hỏi:
    - Text: Nội dung dạng String của câu hỏi
    - Image: Ảnh minh họa cho câu hỏi (Nếu có)
    - Video: Video minh họa cho câu hỏi (Nếu có)
    - Audio: Âm thanh minh họa cho câu hỏi (Nếu có)

- Nội dung đáp án:
    - Một lựa chọn: 4 Hàng với 4 RadioButton
    - Nhiều lựa chọn: N Hàng với N Checkbox

- Nút ấn tiếp theo:
    - Text: Câu tiếp theo

- Menu hành động:
    - Kích thước: Rộng 1/2 màn hình + Cao tối đa 80% màn hình 
    - Có thể cuộn
    - Mỗi chức năng một ô bao gồm:
        - Icon
        - Text: tên chức năng

- Hướng dẫn:
    - Text: Quy tắc trò chơi
    - Buttons: Đóng
    - Dạng hiển thị: BottomSheetDialog
