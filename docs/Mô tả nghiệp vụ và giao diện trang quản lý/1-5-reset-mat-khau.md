# 1.5. RESET MẬT KHẨU

## 1. NGHIỆP VỤ

### Mục đích
Admin reset mật khẩu cho người dùng (quên mật khẩu).

### Luồng xử lý
1. Admin chọn user → "Reset mật khẩu"
2. Dialog: xác nhận email, chọn "Gửi link reset qua email" HOẶC "Tạo mật khẩu tạm thời"
3. Nếu gửi link: tạo token 24h, gửi email, log
4. Nếu mật khẩu tạm: tạo mật khẩu ngẫu nhiên, hiển thị 1 lần cho admin, gửi email cho user, log; user phải đổi khi đăng nhập lần đầu
5. Thông báo thành công

### Quy tắc
- Reset token 24h; mật khẩu tạm chỉ hiển thị 1 lần
- Tối đa 3 lần reset/ngày/tài khoản

### Validation
- Email hợp lệ, tài khoản active

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Điểm vào

- **Nút "Reset mật khẩu":** Trong dropdown hành động hoặc trang chi tiết user. Outline, icon key/refresh.

---

### 2.2. Dialog "Reset mật khẩu"

- **Header:** Icon key + tiêu đề "Reset mật khẩu" + nút đóng.
- **Nội dung:**
  - Hiển thị email người nhận: "Gửi đến: user@example.com".
  - **Chọn phương thức (Radio):**
    - "Gửi link reset qua email" (mặc định): user nhận email có link đổi mật khẩu trong 24h.
    - "Tạo mật khẩu tạm thời": hệ thống tạo mật khẩu và hiển thị một lần trong dialog sau khi submit.
  - Ghi chú: tối đa 3 lần reset trong 24 giờ cho tài khoản này.
- **Footer:** "Hủy" + "Gửi" (hoặc "Tạo mật khẩu").

---

### 2.3. Sau khi gửi

- **Gửi link:** Đóng dialog (hoặc giữ), toast thành công.
- **Tạo mật khẩu tạm:** Trong dialog (hoặc bước 2) hiển thị mật khẩu một lần, có nút Sao chép và cảnh báo "chỉ hiển thị một lần"; nút "Đóng".

---

### 2.4. Lỗi và giới hạn

- Quá 3 lần/24h: toast cảnh báo.
- Lỗi gửi email: toast lỗi, gợi ý kiểm tra cấu hình.
