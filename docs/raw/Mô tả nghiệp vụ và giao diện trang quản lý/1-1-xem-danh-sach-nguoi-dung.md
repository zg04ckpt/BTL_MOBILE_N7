# 1.1. XEM DANH SÁCH NGƯỜI DÙNG

## 1. NGHIỆP VỤ

### Mục đích
Admin có thể xem, tìm kiếm, lọc và sắp xếp danh sách người dùng.

### Luồng xử lý
1. Admin truy cập trang "Quản lý người dùng"
2. Hệ thống load danh sách người dùng từ database
3. Áp dụng bộ lọc (nếu có): trạng thái, vai trò, khoảng thời gian đăng ký
4. Áp dụng tìm kiếm (nếu có): tên, email, ID, số điện thoại
5. Sắp xếp theo tiêu chí được chọn
6. Phân trang: 20–50 người dùng/trang
7. Hiển thị kết quả

### Quy tắc nghiệp vụ
- Mặc định hiển thị tất cả người dùng active
- Phân trang mặc định: 20 người dùng/trang
- Sắp xếp mặc định: ngày đăng ký mới nhất
- Admin chỉ xem thông tin công khai, không xem mật khẩu

### Validation
- Tìm kiếm: tối thiểu 2 ký tự
- Khoảng thời gian: không quá 1 năm

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Loại trang:** Trang danh sách (list page).
- **Cấu trúc từ trên xuống:**
  1. **Header (sticky):** Tiêu đề "Quản lý người dùng" + nút Export (tùy chọn).
  2. **Thanh công cụ:** Ô tìm kiếm + bộ lọc (Trạng thái, Vai trò, Ngày đăng ký) + Sắp xếp + nút Áp dụng / Làm mới.
  3. **Nội dung chính:** Bảng danh sách người dùng (desktop) hoặc danh sách card (mobile).
  4. **Phân trang:** Dưới bảng, có thông tin "Hiển thị X–Y trong Z người dùng", nút Trước/Sau, chọn số dòng/trang (20, 30, 50).
- Nền trang nhạt; nội dung chính trong vùng trắng, bo góc, shadow nhẹ (designer tự chọn).

---

### 2.2. Thanh công cụ

- **Ô tìm kiếm:** Placeholder "Tìm theo tên, email, ID, số điện thoại...". Có icon kính lúp. Gửi request tìm kiếm sau khi người dùng ngừng gõ (debounce).
- **Dropdown Trạng thái:** Mặc định "Tất cả"; option: Hoạt động, Không hoạt động, Bị khóa.
- **Dropdown Vai trò:** Tất cả, User, Editor, Moderator, Admin.
- **Bộ lọc ngày đăng ký:** Hai ô chọn ngày (Từ ngày – Đến ngày).
- **Dropdown Sắp xếp:** Ngày đăng ký mới/cũ nhất, Tên A–Z / Z–A, Điểm cao/thấp nhất.
- **Nút:** "Áp dụng bộ lọc" (primary), "Làm mới" (secondary).
- **Mobile:** Thu gọn thành nút "Bộ lọc" mở drawer/modal chứa toàn bộ tìm kiếm và lọc.

---

### 2.3. Bảng danh sách người dùng

- **Header bảng:** Các cột: Checkbox (chọn tất cả/từng dòng) | Người dùng (avatar + tên + email) | ID | Vai trò (badge) | Trạng thái (badge: Active xanh, Inactive xám, Banned đỏ) | Điểm | Số trận | Ngày đăng ký | Hành động.
- **Hàng dữ liệu:** Hover nhẹ; click vào hàng (trừ checkbox và cột hành động) → chuyển sang trang Chi tiết người dùng (file 1-2).
- **Cột Hành động:** Icon menu (3 chấm dọc); click mở dropdown: Xem chi tiết, Khóa tài khoản (khi active), Mở khóa (khi banned), Reset mật khẩu, Xóa tài khoản (màu đỏ, ẩn nếu không đủ quyền).

---

### 2.4. Empty state, Loading, Lỗi

- **Không có dữ liệu thỏa bộ lọc:** Icon + tiêu đề "Không tìm thấy người dùng", mô tả ngắn, nút "Xóa bộ lọc".
- **Đang tải:** Skeleton hàng trong bảng hoặc spinner giữa vùng nội dung.
- **Lỗi load:** Banner cảnh báo phía trên bảng + nút "Thử lại".

---

### 2.5. Responsive

- **Desktop:** Bảng đủ các cột.
- **Tablet:** Ẩn bớt cột hoặc bảng scroll ngang.
- **Mobile:** Chuyển sang danh sách card (avatar, tên, email, trạng thái, vai trò); chạm card → Chi tiết. Toolbar thành nút "Bộ lọc" mở modal; phân trang đơn giản (Trước/Sau + Trang X/Y).
