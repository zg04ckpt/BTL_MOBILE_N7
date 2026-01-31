# 3.2. XEM CHI TIẾT TRẬN ĐẤU

## 1. NGHIỆP VỤ

### Mục đích
Admin xem chi tiết một trận đấu cụ thể: thông tin trận, hai người chơi, từng câu hỏi – đáp án, kết quả và timeline.

### Luồng xử lý
1. Admin click vào một trận đấu trong danh sách.
2. Hệ thống load: thông tin trận (ID, thời gian, chủ đề, số câu, trạng thái); thông tin người chơi 1 & 2 (tên, điểm, thời gian hoàn thành, số câu đúng/sai); danh sách câu hỏi với đáp án từng người, đúng/sai, thời gian trả lời; kết quả (người thắng, điểm chính xác + nhanh nhẹn).
3. Hiển thị dạng tabs: **Tổng quan**, **Chi tiết** (câu hỏi – đáp án), **Timeline**.

### Quy tắc nghiệp vụ
- Hiển thị đầy đủ để admin kiểm tra tính hợp lệ.
- Timeline giúp phát hiện gian lận (trả lời quá nhanh).

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Loại trang:** Chi tiết trận đấu (detail page).
- **Cấu trúc:** Header (Quay lại + tiêu đề "Chi tiết trận đấu" + ID trận + nút "Đánh dấu nghi vấn", "Xử lý lỗi") → Tabs (Tổng quan | Chi tiết | Timeline) → Nội dung theo tab. Nền trang nhạt; nội dung trong vùng trắng, bo góc, shadow nhẹ.

---

### 2.2. Tab "Tổng quan"

- **Thông tin trận đấu (card):** ID, Chủ đề, Số câu hỏi, Thời gian bắt đầu/kết thúc, Trạng thái (badge).
- **Hai người chơi (2 card cạnh nhau desktop / 1 cột mobile):** Avatar, Tên, Điểm số, Thời gian hoàn thành, Số câu đúng/tổng, Điểm chính xác / nhanh nhẹn (nếu tách). Người thắng nổi bật (viền hoặc icon trophy).
- **Kết quả tổng:** Một dòng nổi bật kiểu "Nguyễn A thắng 85 – 70" (hoặc Hòa).

---

### 2.3. Tab "Chi tiết"

- Danh sách từng câu hỏi: nội dung câu, đáp án đúng, với mỗi người chơi: đã chọn đáp án nào – Đúng/Sai – thời gian trả lời. Layout: accordion (mobile) hoặc bảng (desktop). Đúng/Sai dùng màu hoặc icon rõ ràng.

---

### 2.4. Tab "Timeline"

- Timeline dọc: mỗi mốc = thời điểm (từ lúc bắt đầu) – ai trả lời câu mấy – Đúng/Sai – thời gian trả lời. Node xanh/đỏ theo đúng/sai; có thể highlight cảnh báo khi thời gian trả lời quá nhanh. Tùy chọn lọc theo một người chơi.

---

### 2.5. Loading, Lỗi, Responsive

- **Loading:** Skeleton hoặc spinner giữa vùng nội dung.
- **Lỗi:** Banner cảnh báo + "Không thể tải chi tiết trận đấu." + nút "Thử lại", "Quay lại danh sách".
- **Mobile:** 1 cột; tab Chi tiết dùng accordion; timeline thu gọn text.
