# 1.2. XEM CHI TIẾT NGƯỜI DÙNG

## 1. NGHIỆP VỤ

### Mục đích
Admin xem thông tin chi tiết và lịch sử hoạt động của một người dùng.

### Luồng xử lý
1. Admin click vào một người dùng trong danh sách
2. Hệ thống load thông tin chi tiết (Users, Matches, MatchDetails)
3. Hiển thị theo tabs: Thông tin / Thi đấu / Hoạt động
4. Lịch sử thi đấu: 10 trận gần nhất, có nút "Xem thêm"

### Quy tắc nghiệp vụ
- Không hiển thị mật khẩu, token
- Thống kê tính real-time từ database

### Dữ liệu hiển thị
- Tổng số trận, số trận thắng, tỷ lệ thắng, điểm hiện tại, xếp hạng

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Layout tổng thể

- **Loại trang:** Trang chi tiết (detail page).
- **Cấu trúc từ trên xuống:**
  1. **Header:** Breadcrumb (Quản lý người dùng > Chi tiết) + tiêu đề "Thông tin người dùng" + nút hành động (Khóa/Mở khóa, Reset mật khẩu, Xóa tài khoản — hiện/ẩn theo trạng thái và quyền).
  2. **Card tổng quan:** Avatar lớn, tên, email, ID; badge Trạng thái và Vai trò; các số liệu nhanh: Điểm hiện tại, Xếp hạng, Tổng trận, Tỷ lệ thắng (dạng ô nhỏ hoặc grid).
  3. **Tabs:** "Thông tin" | "Thi đấu" | "Hoạt động".
  4. **Nội dung tab:** Thay đổi theo tab đang chọn.
- Nền trang nhạt; card và nội dung trong vùng trắng, bo góc và shadow nhẹ.

---

### 2.2. Tab "Thông tin"

- Danh sách cặp nhãn–giá trị (read-only): Họ tên, Email, Số điện thoại, Ngày sinh, Giới tính, Ngày đăng ký, Lần đăng nhập cuối, Trạng thái, Vai trò. Desktop có thể 2 cột, mobile 1 cột.
- Không hiển thị mật khẩu, token hay thông tin nhạy cảm.

---

### 2.3. Tab "Thi đấu"

- Thống kê tóm tắt: Tổng trận, Thắng, Thua, Tỷ lệ thắng, Điểm cao nhất.
- **Lịch sử 10 trận gần nhất:** Desktop: bảng (STT, Thời gian, Đối thủ, Chủ đề, Kết quả, Điểm). Mobile: danh sách card/lista (mỗi item: thời gian, đối thủ, chủ đề, kết quả Thắng/Thua, điểm) — tránh bảng nhiều cột khó đọc. Click hàng/card → xem chi tiết trận (modal hoặc trang khác).
- Nút "Xem thêm" mở danh sách đầy đủ lịch sử.
- Empty state: icon + "Chưa có lịch sử thi đấu".

---

### 2.4. Tab "Hoạt động"

- Danh sách log gần đây (đăng nhập, thay đổi thông tin…): icon + mô tả ngắn + thời gian (tương đối hoặc ngày giờ). Mới nhất trước; khoảng 20 dòng, có "Xem thêm" nếu cần.
- Empty state: "Chưa có hoạt động".

---

### 2.5. Loading, lỗi, responsive

- **Loading:** Skeleton cho card tổng quan và nội dung tab.
- **Lỗi:** Banner cảnh báo + "Không thể tải thông tin." + nút "Thử lại".
- **Mobile:** Card tổng quan xếp dọc; nút hành động có thể thu gọn vào menu (⋮).
