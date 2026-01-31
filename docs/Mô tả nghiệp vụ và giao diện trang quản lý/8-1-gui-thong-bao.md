# 8.1. GỬI THÔNG BÁO

## 1. NGHIỆP VỤ

### Mục đích
Admin gửi thông báo cho người dùng: đơn lẻ (1 người) hoặc hàng loạt (nhiều người/theo nhóm); qua Push notification, Email, hoặc cả hai; gửi ngay hoặc lên lịch.

### Luồng gửi đơn lẻ
1. Admin truy cập "Quản lý thông báo" → "Gửi thông báo".
2. Chọn "Gửi cho 1 người dùng".
3. Chọn người dùng từ danh sách (tìm kiếm).
4. Nhập tiêu đề (bắt buộc), nội dung (bắt buộc); có thể chọn template.
5. Chọn phương thức: Push / Email / Cả hai.
6. Bấm "Gửi" → hệ thống gửi → thông báo thành công.

### Luồng gửi hàng loạt
1. Chọn "Gửi cho nhiều người dùng".
2. Chọn đối tượng: Tất cả / Theo nhóm (điểm, chủ đề yêu thích, v.v.) / Tùy chọn (chọn từng người).
3. Nhập nội dung (có thể dùng biến: {name}, {score}, v.v.).
4. Chọn phương thức; chọn thời gian: Gửi ngay / Lên lịch (chọn ngày giờ).
5. Bấm "Gửi" hoặc "Lên lịch" → xử lý tương ứng → thông báo thành công.

### Quy tắc nghiệp vụ
- Gửi hàng loạt có thể mất vài phút; có thể theo dõi trạng thái (đã gửi, đang gửi, lỗi).
- Người dùng có thể tắt nhận thông báo (trừ thông báo quan trọng).

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Layout:** Trang form gửi thông báo (+ tùy chọn lịch sử gửi / lịch đã lên). Cấu trúc: Header → Chọn đối tượng (1 người / nhiều người) → Form chọn người hoặc nhóm → Form nội dung → Nút "Gửi" hoặc "Lên lịch". Nền trang nhạt, form trong card trắng bo góc.

---

### 2.2. Header

- Sticky, nền trắng, border dưới. Trái: "Gửi thông báo" (có thể icon chuông/email). Phải (tùy chọn): link "Lịch sử gửi" / "Thông báo đã lên lịch".

---

### 2.3. Chọn đối tượng (bước 1)

- 2 option dạng card/radio: "Gửi cho 1 người dùng" (icon user + mô tả ngắn) | "Gửi cho nhiều người dùng" (icon nhóm + mô tả). Khi chọn: card có viền/bg primary. Click cả card để chọn.

---

### 2.4. Form chọn người (gửi 1 người)

- Tiêu đề "Chọn người dùng". Ô tìm kiếm autocomplete: placeholder "Tìm theo tên, email, ID..."; gợi ý dropdown (avatar + tên + email), chọn 1. Sau chọn: chip (avatar + tên + nút X bỏ chọn). Bắt buộc chọn 1 người; thiếu thì báo lỗi "Vui lòng chọn người nhận."

---

### 2.5. Form chọn đối tượng (gửi nhiều người)

- Tiêu đề "Chọn đối tượng nhận". Radio/Dropdown: Tất cả người dùng | Theo nhóm (điểm, chủ đề yêu thích, đăng ký 30 ngày...) | Tùy chọn (chọn từng người, list + checkbox hoặc multi-select). Có thể hiển thị "Số người nhận ước tính: ~X".

---

### 2.6. Form nội dung thông báo

- Tiêu đề section "Nội dung thông báo".
- **Trường:** Template (dropdown, tùy chọn; chọn template thì điền sẵn tiêu đề + nội dung) | Tiêu đề (bắt buộc, max ký tự) | Nội dung (textarea, bắt buộc, helper biến {name}, {score}...) | Phương thức (checkbox: Push, Email, Cả hai; ít nhất 1) | Thời gian gửi (chỉ gửi nhiều người: "Gửi ngay" / "Lên lịch" với datetime picker; lên lịch > hiện tại). Có validation và thông báo lỗi.

---

### 2.7. Nút hành động

- Cuối form, hàng căn phải: "Hủy" (secondary), "Gửi" hoặc "Lên lịch" (primary). Khi chọn lên lịch thì nút là "Lên lịch". Khi gửi: disabled + spinner "Đang gửi..." / "Đang lên lịch...". Disable khi thiếu đối tượng, tiêu đề, nội dung hoặc phương thức; lên lịch thì thời gian hợp lệ.

---

### 2.8. Dialog xác nhận (gửi hàng loạt)

- Modal. Nội dung: "Bạn sẽ gửi thông báo cho [X] người [ngay / vào lúc dd/MM HH:mm]. Tiếp tục?" Nút "Hủy", "Gửi" / "Lên lịch" (primary).

---

### 2.9. Sau khi gửi thành công

- Gửi ngay: toast "Gửi thông báo thành công." (hoặc "Đã lên lịch..."); có thể reset form hoặc chuyển tab "Lịch sử gửi".
- Gửi hàng loạt (async): toast "Đã bắt đầu gửi. Theo dõi tiến độ trong Lịch sử gửi." + link "Xem tiến độ". Trang Chi tiết gửi: Đã gửi X/Tổng Y, Đang gửi..., Lỗi + "Thử lại" từng batch.

---

### 2.10. Loading & lỗi

- Gửi: nút spinner, không đóng form đến khi API trả về. Lỗi: toast đỏ "Gửi thất bại. Vui lòng thử lại.", form giữ nguyên. Gửi hàng loạt: trang Chi tiết gửi có danh sách lỗi + số thành công/thất bại.

---

### 2.11. Ghi chú designer

- Chia section rõ (Đối tượng → Nội dung → Phương thức & thời gian); có thể stepper cho "Gửi nhiều người". Breadcrumb "Trang chủ > Quản lý thông báo > Gửi thông báo". Mobile: form 1 cột, nút "Gửi" full width hoặc sticky footer.
