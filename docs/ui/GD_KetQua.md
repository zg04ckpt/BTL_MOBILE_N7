# GIAO DIỆN KẾT QUẢ

## 1. MỤC ĐÍCH

Giao diện Kết quả hiển thị tổng kết sau khi người chơi hoàn thành bài thi (vượt ải hoặc ghép trận), bao gồm điểm số và chi tiết đáp án.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Tiêu đề: "Kết Quả"
- Vị trí: Chính giữa phía trên

### 2.2. Phần Điểm Bài Thi
- Số điểm đạt được:
  - Text: N điểm (Ví dụ: "85 Điểm")
  - Hình ảnh/Logo/Animation trang trí

### 2.3. Danh Sách Điểm Thưởng
- Danh sách các mục điểm, mỗi dòng gồm:
  - Text: "+N điểm"
  - Text: Hạng mục (Xếp hạng / EXP)
  - Text (Nếu có thay đổi): Hạng mới (34 -> 35), Level mới (101 -> 102)

### 2.4. Chức Năng Xem Đáp Án
- Nút hiển thị đáp án:
  - Text: "Hiển thị đáp án đúng"
  - Hành động: Mở rộng danh sách bên dưới

### 2.5. Danh Sách Câu Hỏi và Đáp Án
- Vị trí: Chèn thêm vào cuối trang giao diện
- Mỗi item câu hỏi bao gồm:
  - Text: STT + Nội dung câu hỏi
  - Text: Đáp án đúng
  - Text: Số điểm cộng
  - Text: Đáp án đã chọn sai (nếu có)

### 2.6. Phần Điều Hướng (Footer)
- Nút Về trang chủ:
  - Text: "Về trang chủ"
  - Vị trí cố định: Luôn ở bottom của màn hình
  - Hành động: Chuyển về Giao diện Trang chủ

## 3. LUỒNG THAO TÁC
1. Màn hình hiển thị Điểm bài thi và các điểm cộng.
2. Người dùng nhấn "Hiển thị đáp án đúng" -> Hiển thị danh sách chi tiết.
3. Người dùng nhấn "Về trang chủ" -> Quay về Trang chủ.
