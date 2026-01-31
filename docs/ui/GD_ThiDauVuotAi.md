# GIAO DIỆN THI ĐẤU VƯỢT ẢI ĐƠN

## 1. MỤC ĐÍCH

Giao diện hiển thị nội dung thi đấu cho một người chơi duy nhất. Số lượng câu hỏi không giới hạn, thời gian thi đấu không giới hạn.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Đếm Ngược Bắt Đầu
- Lớp phủ mờ toàn màn hình.
- Bộ đếm ngược: 3 -> 2 -> 1.

### 2.2. Thanh Tiến Trình (Top Bar)
- Vị trí: Cố định trên top, không bị thay đổi khi cuộn.
- Thông tin:
  - Số lượng câu đã làm (Ví dụ: 5)
  - Số lượng câu đúng (Ví dụ: 4)
  - Điểm (Ví dụ: 40)
  - Thời gian đã trôi qua (Ví dụ: 01h 21p 32s)
- Button: Mở Menu hành động.

### 2.3. Khu Vực Câu Hỏi
- Nội dung Text: Nội dung câu hỏi.
- Multimedia (Nếu có): Image, Video, Audio minh họa.

### 2.4. Khu Vực Đáp Án
- Loại 1 lựa chọn: 4 Hàng với 4 RadioButton.
- Loại nhiều lựa chọn: N Hàng với N Checkbox.

### 2.5. Nút Điều Hướng
- Nút "Câu tiếp theo":
  - Text: "Câu tiếp theo"
  - Hành động: Chuyển sang câu mới (Không thể quay lại câu cũ).

### 2.6. Menu Hành Động
- Kích thước: Rộng 1/2 màn hình + Cao tối đa 80%.
- Kiểu: Popup hoặc Drawer có thể cuộn.
- Các chức năng:
  - Nút Nộp bài: Kết thúc phần thi sớm.
  - Nút Hướng dẫn: Hiển thị quy tắc trò chơi.

### 2.7. Popup Hướng Dẫn
- Tiêu đề: Quy tắc trò chơi.
- Button: Đóng.
- Kiểu: BottomSheetDialog.

## 3. LUỒNG THAO TÁC
1. Đếm ngược 3s -> Vào màn hình chính.
2. Hiển thị Câu hỏi + Đáp án.
3. Người dùng chọn đáp án -> Ấn "Câu tiếp theo".
4. Nếu muốn dừng: Mở Menu hành động -> Ấn "Nộp bài" -> Xác nhận -> Chuyển sang Giao diện Kết quả.
5. Nếu muốn xem luật: Mở Menu hành động -> Ấn "Hướng dẫn" -> Đóng.
