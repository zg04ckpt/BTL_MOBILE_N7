# 2.1. THÊM CÂU HỎI MỚI

## 1. NGHIỆP VỤ

### Mục đích
Admin/Editor thêm câu hỏi mới vào ngân hàng câu hỏi.

### Luồng xử lý
1. Vào "Quản lý câu hỏi" → "Thêm câu hỏi mới"
2. Form nhiều tab: Thông tin cơ bản, Đáp án, Media (nếu Audio/Image), Metadata
3. Điền thông tin → "Lưu nháp" (Draft) hoặc "Gửi duyệt" (Pending)
4. Validate: nội dung 10–500 ký tự, ít nhất 1 đáp án đúng, 4 đáp án, điểm 6–10, thời gian 10–300s, file đúng định dạng
5. Lưu nháp: status Draft; Gửi duyệt: status Pending, thông báo Moderator, log
6. Thông báo thành công

### Quy tắc
- Câu hỏi mới phải duyệt mới dùng trong thi đấu
- Draft chỉ người tạo thấy; Pending tất cả Moderator/Admin thấy
- Tự động gán người tạo và ngày tạo

### Validation
- Nội dung: 10–500 ký tự; đáp án: 1–200 ký tự; ít nhất 1 đáp án đúng
- File audio: mp3, wav, max 5MB; ảnh: jpg, png, max 2MB
- Điểm: 6–10; thời gian: 10–300 giây

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Loại trang:** Form nhiều tab (Thông tin cơ bản | Đáp án | Media | Metadata).
- **Cấu trúc:** Header (tiêu đề "Thêm câu hỏi mới" + nút Hủy) → Tabs → Nội dung tab → Footer (Lưu nháp | Gửi duyệt). Form trong vùng nội dung trắng, nền trang nhạt.
- **Hủy:** Click Hủy → xác nhận "Dữ liệu chưa lưu sẽ mất" → quay lại danh sách câu hỏi.

---

### 2.2. Tab "Thông tin cơ bản"

- **Các trường:** Loại câu hỏi (Text / Audio / Image, mặc định Text); Nội dung câu hỏi (bắt buộc, 10–500 ký tự, có đếm); Chủ đề (dropdown, bắt buộc); Mức độ khó (Dễ / Trung bình / Khó); Điểm số (6–10); Thời gian làm bài giây (10–300); Loại trả lời (1 đáp án đúng / Chọn nhiều đáp án đúng).
- Có thể đánh dấu tab lỗi (chấm đỏ) nếu validate tab đó fail. Lỗi hiển thị dưới ô tương ứng.

---

### 2.3. Tab "Đáp án"

- Bốn ô đáp án A, B, C, D (bắt buộc, max 200 ký tự); mỗi ô có checkbox "Đáp án đúng" (chọn 1 hoặc nhiều tùy loại trả lời). Giải thích tùy chọn dưới mỗi đáp án. Có thể thêm ảnh/audio cho đáp án.
- Validation: ít nhất 1 đáp án đúng; 4 đáp án không trống.

---

### 2.4. Tab "Media"

- Chỉ hiện khi Loại câu hỏi = Audio hoặc Image. Audio: upload mp3/wav (tối đa 5MB), có player preview. Image: upload jpg/png (tối đa 2MB), có preview và nút xóa. Hiển thị lỗi định dạng/dung lượng khi không hợp lệ.

---

### 2.5. Tab "Metadata"

- Tags/Keywords (input hoặc tag); Ghi chú nội bộ (textarea, tùy chọn). Người tạo, ngày tạo hiển thị read-only sau khi lưu.

---

### 2.6. Footer và trạng thái

- **Lưu nháp (secondary):** Lưu Draft, toast thành công, ở lại trang.
- **Gửi duyệt (primary):** Validate toàn bộ; nếu pass → lưu Pending, toast, chuyển về danh sách hoặc chi tiết câu hỏi.
- Đang lưu: disable nút, spinner. Lỗi API: toast lỗi. Validation: cuộn tới trường lỗi đầu tiên.

---

### 2.7. Mobile

- **Tabs:** Scroll ngang hoặc chuyển thành stepper (Bước 1, 2, 3…) để tránh chật. Có thể đánh dấu tab lỗi (chấm đỏ).
- **Form:** 1 cột; các trường xếp dọc. Tab "Đáp án" có thể accordion (A, B, C, D mở từng ô).
- **Footer:** Nút "Lưu nháp" và "Gửi duyệt" sticky bottom hoặc luôn hiện dưới cùng; nút full width hoặc 2 nút cạnh nhau.
- **Upload Media:** Trên mobile dùng chọn file / chụp ảnh; preview thu nhỏ, dễ xóa.
