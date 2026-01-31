# 5.1. TẠO SỰ KIỆN

## 1. NGHIỆP VỤ

### Mục đích
Admin tạo sự kiện thi đấu đặc biệt (tên, mô tả, thời gian, quy tắc, phần thưởng).

### Luồng xử lý
1. Admin truy cập "Quản lý sự kiện" → "Tạo sự kiện mới".
2. Nhập: Thông tin cơ bản (tên, mô tả, banner); Thời gian (bắt đầu, kết thúc); Quy tắc (chủ đề, mức độ khó, số câu, thời gian làm bài); Phần thưởng (điểm bonus, huy hiệu, quà).
3. Bấm "Lưu".
4. Hệ thống validate (tên 5–100 ký tự, thời gian kết thúc > bắt đầu, bắt đầu ≥ hiện tại); lưu với trạng thái "Sắp diễn ra".
5. Tùy chọn gửi thông báo cho tất cả người dùng.
6. Hiển thị thông báo thành công.

### Quy tắc nghiệp vụ
- Sự kiện phải được tạo trước ít nhất 24 giờ.
- Có thể chỉnh sửa sự kiện trước khi bắt đầu.
- Sau khi bắt đầu không chỉnh sửa quy tắc.
- Tự động chuyển trạng thái: Sắp diễn ra → Đang diễn ra → Đã kết thúc.

### Validation
- Tên: bắt buộc, 5–100 ký tự.
- Thời gian kết thúc > Thời gian bắt đầu; Thời gian bắt đầu ≥ Hiện tại.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Loại trang:** Form tạo sự kiện (create form), có thể scroll. Header: "Tạo sự kiện mới" + nút Quay lại/Hủy + nút "Lưu" (primary). Form chia sections: Thông tin cơ bản, Thời gian, Quy tắc đặc biệt, Phần thưởng; mỗi section là card/khối có tiêu đề. Nền trang nhạt; form trong vùng trắng, bo góc.

---

### 2.2. Section "Thông tin cơ bản"

- **Tên sự kiện** (bắt buộc, 5–100 ký tự). **Mô tả** (textarea, tùy chọn). **Banner sự kiện** (upload ảnh, kéo thả hoặc chọn file; tỷ lệ khuyến nghị; có preview và nút xóa). Lỗi validation hiển thị dưới ô.

---

### 2.3. Section "Thời gian"

- **Thời gian bắt đầu** (bắt buộc, DateTime picker; ≥ hiện tại; có thể cảnh báo "nên tạo trước 24 giờ"). **Thời gian kết thúc** (bắt buộc, DateTime picker; > thời gian bắt đầu). Lỗi hiển thị dưới ô.

---

### 2.4. Section "Quy tắc đặc biệt"

- **Chủ đề** (multi-select hoặc checkbox, có thể "Tất cả chủ đề"). **Mức độ khó** (Dễ / Trung bình / Khó / Tất cả). **Số câu hỏi mỗi trận** (số, min–max theo quy tắc, mặc định 10). **Thời gian làm bài** (giây hoặc phút).

---

### 2.5. Section "Phần thưởng" và Tùy chọn

- **Điểm bonus** (số, tùy chọn). **Huy hiệu đặc biệt** (upload icon hoặc chọn thư viện). **Quà tặng / mô tả** (textarea, tùy chọn). Cuối form: Checkbox "Gửi thông báo cho tất cả người dùng khi tạo sự kiện" (mặc định unchecked).

---

### 2.6. Nút Lưu, Loading, Lỗi

- Nút "Lưu" ở header hoặc sticky footer (mobile). Khi submit: validate tên 5–100 ký tự, thời gian hợp lệ; nếu lỗi scroll tới trường lỗi đầu tiên. Khi đang lưu: disabled + spinner. Sau thành công: toast, chuyển về danh sách hoặc chi tiết sự kiện. Lỗi: toast lỗi, form giữ nguyên.
