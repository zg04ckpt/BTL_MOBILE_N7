# GIAO DIỆN KẾT QUẢ

## 1. MỤC ĐÍCH

Giao diện Kết quả hiển thị tổng kết sau khi người chơi hoàn thành bài thi (vượt ải hoặc ghép trận), bao gồm điểm số, thưởng phạt và chi tiết đáp án.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Tiêu Đề
- Tiêu đề: "Kết Quả"
- Vị trí: Chính giữa phía trên

### 2.2. Phần Điểm Bài Thi
- Số điểm đạt được:
  - Text: N điểm (Ví dụ: "85 Điểm")
  - Kiểu hiển thị: Chữ lớn, nổi bật, có thể kèm hiệu ứng animation tăng số
- Hình ảnh/Logo trang trí: Tùy biến theo cấp độ đạt được (Vàng, Bạc, Đồng)

### 2.3. Phần Chi Tiết Điểm Thưởng
- Danh sách các mục điểm, mỗi dòng gồm:
  - Điểm cộng/trừ: "+N điểm" hoặc "-N điểm"
  - Hạng mục:
    - Xếp hạng (Rank Point)
    - Kinh nghiệm (EXP)
    - Thưởng nhanh nhẹn (nếu có)
- Hiển thị thay đổi (nếu có):
  - Hạng mới: Ví dụ "34 -> 35" (kèm mũi tên chỉ lên/xuống)
  - Level mới: Ví dụ "101 -> 102" (kèm hiệu ứng lên cấp)

### 2.4. Phần Chức Năng Xem Đáp Án
- Nút hiển thị đáp án:
  - Text: "Hiển thị đáp án đúng"
  - Kiểu: Button mở rộng/thu gọn (Accordion) hoặc chuyển sang màn hình chi tiết
  - Hành động: Hiển thị danh sách câu hỏi và đáp án chi tiết

### 2.5. Danh Sách Câu Hỏi Và Đáp Án
- Kiểu hiển thị: List cuộn, chèn vào dưới nút xem đáp án hoặc màn hình riêng
- Mỗi câu hỏi bao gồm:
  - Số thứ tự và Nội dung câu hỏi
  - Đáp án đúng: Hiển thị màu xanh lá
  - Đáp án đã chọn sai (nếu có): Hiển thị màu đỏ
  - Số điểm cộng: "+10" (nếu đúng), "+0" (nếu sai)

### 2.7. Kết Quả Đối Kháng (Chỉ dành cho chế độ Ghép trận)
- Danh sách xếp hạng trận đấu:
  - Hiển thị danh sách người chơi tham gia trận đấu, sắp xếp theo điểm số từ cao xuống thấp.
  - Thông tin mỗi hàng:
    - Hạng (1, 2, 3...)
    - Avatar, Tên người chơi
    - Tổng điểm
  - Nút chức năng cho từng người chơi (ngoại trừ bản thân):
    - Nút "Kết bạn" (Icon dấu +): Hiển thị nếu 2 người chưa là bạn bè. Nhấn để gửi lời mời kết bạn nhanh.
    - Nút "Đã gửi" (Icon đồng hồ): Hiển thị nếu đã gửi lời mời.
    - Nút "Bạn bè" (Icon mặt cười): Hiển thị nếu đã là bạn bè.
    - Nút "Tố cáo" (Icon cờ): Báo cáo hành vi gian lận hoặc ngôn từ không phù hợp.

### 2.8. Phần Điều Hướng (Footer)
- Nút Về trang chủ:
  - Text: "Về trang chủ"
  - Kiểu: Primary button
  - Vị trí: Cố định ở dưới cùng màn hình (Sticky Bottom)
  - Hành động: Chuyển về Giao diện Trang chủ

## 3. LUỒNG THAO TÁC

### 3.1. Luồng Hiển Thị Kết Quả
1. Sau khi kết thúc bài thi (nộp bài hoặc hết giờ), hệ thống chuyển đến Giao diện Kết quả.
2. Hiển thị điểm số bài thi với animation.
3. Hiển thị danh sách điểm thưởng (Xếp hạng, EXP) chạy lần lượt.
4. Nếu có lên cấp hoặc thăng hạng: Hiển thị hiệu ứng chúc mừng.

### 3.2. Luồng Xem Đáp Án
1. Người dùng nhấn nút "Hiển thị đáp án đúng".
2. Hệ thống mở rộng khu vực danh sách câu hỏi bên dưới.
3. Người dùng cuộn xem từng câu:
   - Thấy rõ câu nào đúng, câu nào sai.
   - Thấy đáp án đúng của các câu sai.

### 3.3. Luồng Quay Về Trang Chủ
1. Người dùng nhấn nút "Về trang chủ".
2. Hệ thống chuyển cảnh về Giao diện Trang chủ.

## 4. QUY TẮC NGHIỆP VỤ

### 4.1. Hiển Thị Điểm
- Điểm xếp hạng (RP):
  - Thắng: +20 RP
  - Thua: -10 RP
  - Hòa: +5 RP
  - Vượt ải đơn: Không tính RP (hoặc tính theo cơ chế riêng)
- Điểm kinh nghiệm (EXP):
  - Thắng: 100% EXP cơ bản + Bonus
  - Thua: 20% EXP cơ bản

### 4.2. Trạng Thái
- Nếu thi đấu ghép trận: Có thể cần chờ một chút để đồng bộ kết quả từ server nếu trận đấu chưa kết thúc hoàn toàn với người chơi khác (tuy nhiên thường hiển thị kết quả cá nhân ngay).

## 5. RESPONSIVE

### 5.1. Desktop
- Bố cục dạng thẻ (Card) ở giữa màn hình.
- Danh sách đáp án có thể hiển thị dạng lưới hoặc list rộng.

### 5.2. Mobile
- Bố cục toàn màn hình.
- Nút "Về trang chủ" luôn nổi ở đáy để dễ thao tác.
