# 1.4. XÓA TÀI KHOẢN

## 1. NGHIỆP VỤ

### Mục đích
Xóa (soft delete) tài khoản người dùng trong trường hợp đặc biệt.

### Luồng xử lý
1. Admin chọn user → "Xóa tài khoản"
2. Hệ thống kiểm tra: đang trong trận đấu? dữ liệu quan trọng?
3. Dialog cảnh báo: xác nhận, liệt kê ảnh hưởng, nhập lý do xóa (bắt buộc), nhập lại mật khẩu admin
4. Xác nhận → soft delete, ẩn khỏi danh sách, anonymize dữ liệu cá nhân, log
5. Thông báo thành công

### Quy tắc
- Chỉ super admin
- Không xóa admin khác, không xóa khi đang trong trận đấu
- Soft delete, có thể khôi phục trong 30 ngày

### Validation
- Lý do: 20–500 ký tự
- Mật khẩu admin đúng
- Không xóa nếu đang trong trận đấu

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Điểm vào

- **Nút "Xóa tài khoản":** Trong dropdown hành động (cuối danh sách) hoặc trang chi tiết. Chỉ hiện với super admin. Màu đỏ, icon thùng rác.

---

### 2.2. Dialog "Xóa tài khoản"

- **Header:** Icon cảnh báo nghiêm trọng + tiêu đề "Xóa tài khoản vĩnh viễn?" + nút đóng.
- **Nội dung:**
  - Cảnh báo: hành động không thể hoàn tác, tài khoản sẽ bị ẩn và dữ liệu cá nhân được ẩn danh.
  - Block thông tin user (avatar, tên, email).
  - **Điều kiện đặc biệt:** Nếu đang trong trận đấu → thông báo rõ và disable nút Xóa. Nếu có nhiều trận/điểm → chỉ thông báo.
  - **Lý do xóa** (bắt buộc): textarea, tối thiểu 20 ký tự, max 500, có đếm ký tự.
  - **Mật khẩu admin** (bắt buộc): input password để xác nhận. Sai mật khẩu: hiển thị lỗi dưới ô.
- **Footer:** "Hủy" (secondary), "Xóa tài khoản" (danger). Nút Xóa disable khi thiếu lý do đủ dài, thiếu mật khẩu, hoặc đang trong trận đấu.
- **Sau khi thành công:** Đóng dialog, toast "Đã xóa tài khoản.", chuyển về danh sách hoặc cập nhật view.
- **Mobile:** Form dài (lý do + mật khẩu) nên dùng **bottom sheet** hoặc trang full thay modal giữa màn; nút full width, vùng chạm rõ.
