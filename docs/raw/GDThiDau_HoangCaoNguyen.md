## Mô tả giao diện thi đấu 
- Giao diện này hiển thị các nội dung thi đấu cho một nhóm người chơi (1 phòng thi đấu)
- Số lượng câu hỏi giới hạn, thời gian thi đấu giới hạn
- Có thể quay lại làm câu trước đó
- Nội dung có thể scroll

### Luồng xử lý chính
- Đếm ngược 3s -> Hiển thị nội dung + Đáp án -> Lựa chọn đáp án -> Ấn câu tiếp theo
- Mở menu hành động -> Ấn nút Nộp bài -> Hiển thị xác nhận -> GD hiển thị kết quả
- Mở menu hành động -> Ấn nút Hướng dẫn -> Hiển thị hướng dẫn -> Ấn đóng
- Mở menu hành động -> Ấn nút Nhảy câu -> Mở ma trận câu -> Chọn câu -> Hiển thị nội dung câu

### Các thành phần UI
(* Bao gồm các thành phần giống như trong GD vượt ải đơn)

- Thông báo trạng thái của các người chơi khác:
    - Text: Thông báo
    - Button: Nút x
    - Vị trí: Chính giữa top

- Ma trận câu: 1 Bảng chứa danh sách các câu
    - Text: Ma trận câu hỏi
    - Text: Ấn vào ô câu hỏi để dịch chuyển
    - List câu hỏi: Grid 3xN mỗi ô bao gồm:
        - Text: Số thứ tự câu
        - Màu hiển thị đánh dấu trạng thái: Đã làm/Chưa làm
    - Kiểu hiển thị: BottomSheetDialog

