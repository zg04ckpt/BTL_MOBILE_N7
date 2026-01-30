# 1.3. KHÓA / MỞ KHÓA TÀI KHOẢN

## 1. NGHIỆP VỤ

### Mục đích
Admin khóa tài khoản vi phạm hoặc mở khóa khi cần.

### Luồng khóa
1. Admin chọn người dùng → click "Khóa tài khoản"
2. Dialog xác nhận: thông tin user, nhập lý do khóa (bắt buộc), chọn thời gian (Tạm thời 1/7/30 ngày hoặc Vĩnh viễn)
3. Xác nhận → cập nhật status, lưu UserBans, gửi thông báo, log
4. Nếu tạm thời: tạo job tự động mở khóa
5. Thông báo thành công

### Luồng mở khóa
1. Chọn user bị khóa → "Mở khóa"
2. Dialog xác nhận
3. Cập nhật status active, unban_reason, log
4. Thông báo thành công

### Quy tắc
- Không khóa admin khác (trừ super admin)
- Lý do khóa: 10–500 ký tự
- Tự động mở khóa khi hết thời gian (khóa tạm thời)

### Validation
- Lý do khóa: bắt buộc, 10–500 ký tự
- Thời gian khóa: số dương (nếu tạm thời)

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Điểm vào (Khóa tài khoản)

- **Vị trí:** Trong dropdown hành động danh sách người dùng hoặc header trang chi tiết. Chỉ hiện khi trạng thái = Active. Ẩn/disable nếu admin đang đăng nhập không phải super admin và đối tượng là admin khác.
- **Nút:** "Khóa tài khoản", outline màu cảnh báo, có icon khóa.

---

### 2.2. Dialog "Khóa tài khoản"

- **Header:** Icon cảnh báo + tiêu đề "Khóa tài khoản" + nút đóng (X).
- **Nội dung:**
  - Đoạn mô tả: người dùng sẽ không thể đăng nhập và tham gia thi đấu.
  - Block thông tin người dùng (avatar, tên, email).
  - **Lý do khóa** (bắt buộc): textarea, placeholder gợi ý tối thiểu 10 ký tự, max 500, có đếm ký tự. Lỗi validation hiển thị dưới ô.
  - **Thời gian khóa:** Radio "Tạm thời" (kèm dropdown: 1 / 7 / 30 ngày) hoặc "Vĩnh viễn". Mặc định: Tạm thời + 7 ngày.
  - Checkbox (tùy chọn): "Gửi thông báo qua email", mặc định chọn.
- **Footer:** "Hủy" (secondary), "Khóa tài khoản" (primary màu cảnh báo). Nút Khóa disable khi lý do < 10 ký tự.
- **Sau khi thành công:** Đóng dialog, toast thành công, cập nhật trạng thái trên danh sách/chi tiết.

---

### 2.3. Điểm vào và Dialog "Mở khóa"

- **Nút "Mở khóa":** Trong dropdown hoặc header trang chi tiết, chỉ hiện khi trạng thái = Banned. Nút outline màu xanh, icon mở khóa.
- **Dialog "Mở khóa":** Tiêu đề "Mở khóa tài khoản", mô tả ngắn, block thông tin user, ô "Lý do mở khóa" (tùy chọn). Footer: "Hủy" + "Mở khóa" (primary xanh). Sau khi thành công: đóng dialog, toast, cập nhật trạng thái.

---

### 2.4. Validation và lỗi

- Lý do khóa < 10 ký tự: hiển thị lỗi dưới textarea.
- Lỗi API: toast đỏ "Không thể khóa tài khoản. Vui lòng thử lại."

---

### 2.5. Mobile

- Dialog Khóa/Mở khóa: trên mobile có thể dùng **bottom sheet** thay modal giữa màn (form dài, dễ cuộn). Nút Hủy / Khóa (hoặc Mở khóa) đặt cố định dưới; vùng chạm đủ lớn.
