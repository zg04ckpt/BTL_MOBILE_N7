# GIAO DIỆN THI ĐẤU GHÉP TRẬN

## 1. MỤC ĐÍCH

Giao diện hiển thị nội dung thi đấu cho một nhóm người chơi (1 phòng thi đấu). Số lượng câu hỏi giới hạn, thời gian thi đấu giới hạn.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Các Thành Phần Cơ Bản
(Bao gồm các thành phần giống Giao diện Vượt ải đơn):
- Đếm ngược bắt đầu (3s).
- Thanh tiến trình (Thông tin câu làm, câu đúng, điểm, thời gian).
- Khu vực Câu hỏi & Đáp án.
- Menu hành động.

### 2.2. Thông Báo Trạng Thái Đối Thủ
- Vị trí: Chính giữa top.
- Nội dung: Text thông báo trạng thái hoặc tiến độ của người chơi khác.
- Button: Nút "x" để đóng/ẩn thông báo.

### 2.3. Ma Trận Câu Hỏi (Chức năng Nhảy câu)
- Vị trí: Trong Menu hành động hoặc BottomSheetDialog.
- Tiêu đề: "Ma trận câu hỏi".
- Hướng dẫn: "Ấn vào ô câu hỏi để dịch chuyển".
- Danh sách câu hỏi: Grid 3xN (Lưới 3 cột).
- Mỗi ô câu hỏi:
  - Text: Số thứ tự câu.
  - Màu sắc: Đánh dấu trạng thái (Đã làm / Chưa làm).

## 3. CÁC QUY TẮC RIÊNG
- **Quay lại câu cũ:** Cho phép quay lại làm câu trước đó (Khác với Vượt ải đơn).
- **Nhảy câu:** Người chơi có thể chọn làm câu bất kỳ thông qua Ma trận câu.

## 4. LUỒNG THAO TÁC
1. Đếm ngược 3s -> Vào màn hình thi đấu.
2. Hiển thị Câu hỏi 1.
3. Người dùng chọn đáp án -> Ấn "Câu tiếp theo".
4. Muốn làm câu khác: Mở Menu hành động -> Chọn "Nhảy câu" -> Hiện Ma trận -> Chọn câu số X -> Hiển thị câu số X.
5. Muốn nộp bài: Mở Menu hành động -> Ấn "Nộp bài" -> Xác nhận -> Chuyển sang Giao diện Kết quả.
