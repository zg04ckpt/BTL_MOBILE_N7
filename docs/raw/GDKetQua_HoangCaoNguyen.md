## Mô tả giao diện thi đấu 
- Giao diện này hiển thị kết quả cho vượt ải đơn/thi đấu
- Có thể scroll

### Luồng xử lý chính
- Hiển thị điểm bài thi, điểm cộng xếp hạng, điểm cộng EXP -> Ấn hiển thị đáp án đúng
-> Hiển thị danh sách câu và đáp án đúng (Đáp án sai) -> Ấn nút quay về trang chủ

### Các thành phần UI
- Tiêu đề: Kết quả

- Điểm bài thi:
    - Text: N điểm 
    - Hình/Logo/Animation trang trí (Tùy biến)

- Danh sách điểm, mỗi dòng gồm:
    - Text: +N điểm
    - Text: Hạng mục (Xếp hạng/EXP)
    - Text (Nếu có thay đổi): Hạng mới (34 -> 35), Level mới (101 -> 102)

- Nút hiển thị đáp án:
    - Text: Hiển thị đáp án đúng

- Danh sách câu chứa đáp án:
    - Kiểu hiển thị: Chèn thêm vào cuối trang GD
    - Mỗi câu gồm:
        - Text: STT + Câu hỏi
        - Text: Đáp án đúng
        - Text: Số điểm cộng
        - Text (Nếu có): Đáp án đã chọn sai

- Nút quay lại trang chủ:
    - Text: Về trang chủ
    - Vị trí cố định: Luôn ở bottom của màn hình
