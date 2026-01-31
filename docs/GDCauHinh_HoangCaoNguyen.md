## Mô tả giao diện cấu hình
Giao diện này được xây dụng để người chơi lựa chọn nội dung thi đấu bao gồm:
- Kiểu thi đấu: Vượt ải đơn/Ghép trận
- Số lượng người thi đấu:
    - Cố định: 1/2/5/10 
    - Tự động khóa chọn 1 khi lựa chọn vượt ải đơn
    - Chỉ chọn 2/5/10 khi lựa chọn kiểu thi đấu ghép trận
- Chủ đề câu hỏi: 
    - Bao gồm N ô chủ đề + 1 ô chủ đề hỗn hợp + 1 ô chủ đề ngẫu nhiên

### Luồng xử lý chính
Chọn kiểu thi đấu -> Chọn số lượng người thi đấu -> Chọn chủ đề câu hỏi 
-> Ấn nút xác nhận -> Hiển thị thông báo xác nhận -> Ấn nút bắt đầu ngay

### Các thành phần UI
Giao diện cấu hình có thể scroll, lựa chọn màu sắc được thống nhất sau khi hoàn thành docs

- Tiêu đề chung: Lựa chọn nội dung thi đấu

- Khu vực lựa chọn kiểu thi đấu:
    - Tiêu đề: Kiểu thi đấu
    - Các kiểu thi đấu: Danh sách dạng lưới 2x1 [2 cột - 1 hàng]
    - Khi ấn vào ô: Đánh dấu bằng cách đổi màu ô (màu đậm hơn) và hightlight viền

- Khu vực lựa chọn số lượng người chơi:
    - Tiêu đề: Số lượng người chơi
    - Số lượng: Danh sách dạng lưới 2x2 [2 cột - 2 hàng]
    - Khi ấn vào ô: Đánh dấu bằng cách đổi màu ô (màu đậm hơn) và hightlight viền

- Khu vực lựa chọn chủ đề:
    - Tiêu đề: Lựa chọn chủ đề
    - Số lượng: Danh sách dạng lưới 2xN [2 cột - N hàng]
    - Khi ấn vào ô: Đánh dấu bằng cách đổi màu ô (màu đậm hơn) và hightlight viền
    - Ô đầu tiên: Hỗn hợp 
    - Ô thứ 2: Ngẫu nhiên
    - Các ô còn lại: Các chủ đề

- Button xác nhận: 
    - Nội dung: Xác nhận
    - Chiều rộng: kéo dài hết chiều ngang màn hình

- Thông báo xác nhận:
    - Kiểu: BottomSheetDialog
    - Nội dung bao gồm:
        - Tiêu đề: Xác nhận trước khi bắt đầu
        - Text: Kiểu thi đấu
        - Text: Số lượng người
        - Text: Chủ đề
        - Text: DS bonus (VD: X2 điểm xếp hạng, +10 điểm XH, ...) [Sửa đổi sau khi có quy tắc XH]
        - Text: Quy tắc chơi
    - Button:
        - Nội dung: Bắt đầu ngay
        - Chiều rộng: kéo dài hết chiều ngang BottomSheetDialog
