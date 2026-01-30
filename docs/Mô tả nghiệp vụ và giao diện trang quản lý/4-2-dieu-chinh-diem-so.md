# 4.2. ĐIỀU CHỈNH ĐIỂM SỐ

## 1. NGHIỆP VỤ

### Mục đích
Admin điều chỉnh điểm số của người dùng trong trường hợp đặc biệt (sửa lỗi, khiếu nại, v.v.).

### Luồng xử lý
1. Admin tìm người dùng cần điều chỉnh điểm (từ trang người dùng hoặc xếp hạng).
2. Bấm "Điều chỉnh điểm".
3. Hiển thị form: Điểm hiện tại (chỉ đọc); Nhập điểm mới HOẶC Nhập số điểm cộng/trừ; Lý do điều chỉnh (bắt buộc).
4. Admin điền và xác nhận.
5. Hệ thống cập nhật điểm, cập nhật bảng xếp hạng, log hành động; có thể gửi thông báo người dùng.
6. Hiển thị thông báo thành công.

### Quy tắc nghiệp vụ
- Chỉ super admin mới có quyền điều chỉnh điểm.
- Phải có lý do rõ ràng; mọi thay đổi đều được log.
- Người dùng có thể khiếu nại nếu không đồng ý.

### Validation
- Lý do: bắt buộc, 20–500 ký tự.
- Điểm mới: ≥ 0.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Điểm vào

- **Vị trí:** Trang Chi tiết người dùng (1-2) hoặc Bảng xếp hạng: nút "Điều chỉnh điểm" trong menu hành động hoặc card thông tin điểm. Chỉ hiện khi đăng nhập là super admin.

---

### 2.2. Modal / Drawer "Điều chỉnh điểm"

- **Loại:** Modal (desktop) hoặc Bottom sheet / Drawer (mobile). Header: tiêu đề "Điều chỉnh điểm" + nút đóng. Block thông tin người dùng (avatar, tên, email hoặc ID – chỉ đọc).
- **Form:** **Điểm hiện tại** (chỉ đọc, số lớn). **Cách điều chỉnh** (Radio): "Nhập điểm mới" → hiện input Điểm mới (số ≥ 0); "Cộng/trừ điểm" → hiện input số (có thể âm), có helper "Điểm sau điều chỉnh: …" cập nhật theo giá trị; nếu điểm sau < 0 thì cảnh báo và có thể disable Xác nhận. **Lý do điều chỉnh** (bắt buộc, 20–500 ký tự, textarea).
- **Footer:** Nút "Hủy" (secondary), "Xác nhận" (primary). Xác nhận disable khi thiếu dữ liệu hoặc validation lỗi. Khi gửi: spinner + disabled. Trước khi gửi: dialog xác nhận (tóm tắt hành động và lý do). Sau thành công: đóng modal, toast thành công, cập nhật điểm trên trang. Lỗi API: toast lỗi, giữ form. Validation: highlight trường lỗi, message dưới ô.
- **Mobile:** Modal có thể thay bằng **bottom sheet** hoặc drawer; form 1 cột, nút full width dưới cùng.
