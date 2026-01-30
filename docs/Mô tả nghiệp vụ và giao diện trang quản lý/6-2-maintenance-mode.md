# 6.2. BẬT/TẮT MAINTENANCE MODE

## 1. NGHIỆP VỤ

### Mục đích
Bật chế độ bảo trì khi cần bảo trì hệ thống: chặn đăng nhập mới, cho phép trận đang diễn ra hoàn thành, hiển thị thông báo cho người dùng.

### Luồng xử lý
1. Admin truy cập "Cấu hình hệ thống" → "Maintenance Mode" (hoặc menu riêng).
2. Bấm "Bật Maintenance Mode".
3. Nhập: Thông báo cho người dùng (bắt buộc); Thời gian bắt đầu (mặc định: ngay bây giờ); Thời gian kết thúc dự kiến.
4. Xác nhận.
5. Hệ thống bật maintenance mode: hiển thị thông báo cho tất cả người dùng, chặn đăng nhập mới, cho phép trận đang diễn ra hoàn thành, log hành động.
6. Sau khi bảo trì xong: Bấm "Tắt Maintenance Mode" → hệ thống trở lại bình thường, gửi thông báo người dùng.

### Quy tắc nghiệp vụ
- Maintenance mode chặn đăng nhập mới; trận đang diễn ra được phép hoàn thành.
- Admin vẫn có thể đăng nhập.
- Nên thông báo trước ít nhất 1 giờ.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Điểm vào

- Trong trang "Cấu hình hệ thống" (6-1): tab hoặc section "Maintenance Mode"; hoặc menu sidebar. Chỉ admin (super admin) thấy và thao tác.

---

### 2.2. Khi chưa bật Maintenance Mode

- Section "Chế độ bảo trì": Badge "Hệ thống đang hoạt động bình thường" (màu xanh, có thể kèm icon) + mô tả ngắn "Người dùng có thể đăng nhập và thi đấu bình thường." + Nút "Bật Maintenance Mode" (màu cảnh báo: cam/đỏ). Click → mở modal Bật Maintenance Mode.

---

### 2.3. Modal "Bật Maintenance Mode"

- Modal vừa (desktop); mobile có thể full-page. Header: tiêu đề "Bật chế độ bảo trì", icon bảo trì (tùy chọn), nút đóng.
- **Form:** Thông báo cho người dùng (textarea, bắt buộc, 10–500 ký tự, placeholder mẫu) | Thời gian bắt đầu (datetime, mặc định "Ngay bây giờ") | Thời gian kết thúc dự kiến (datetime, phải sau thời gian bắt đầu).
- **Cảnh báo trong modal:** Banner vàng + text "Khi bật, người dùng mới không thể đăng nhập. Trận đang diễn ra được phép hoàn thành. Nên thông báo trước ít nhất 1 giờ."
- **Footer:** "Hủy" (secondary), "Bật Maintenance Mode" (primary cam/đỏ). Khi gửi: disabled + spinner "Đang bật...". Có thể có dialog xác nhận trước khi gửi.

---

### 2.4. Khi đang bật Maintenance Mode

- Cùng section: Badge "Đang bảo trì" (vàng/đỏ nhạt) + Thời gian bắt đầu & dự kiến kết thúc + Trích đoạn thông báo đang hiển thị + Nút "Tắt Maintenance Mode" (primary xanh). Click "Tắt" → dialog xác nhận → gửi → toast "Đã tắt chế độ bảo trì." → UI cập nhật (badge "Hoạt động bình thường", hiện lại nút "Bật").

---

### 2.5. Dialog xác nhận Bật

- Modal nhỏ. Nội dung: "Bạn có chắc bật chế độ bảo trì? Người dùng mới sẽ không thể đăng nhập cho đến khi tắt." Nút "Hủy", "Bật" (primary cam/đỏ).

---

### 2.6. Dialog xác nhận Tắt

- Nội dung: "Bạn có chắc tắt chế độ bảo trì? Hệ thống sẽ hoạt động bình thường trở lại." Nút "Hủy", "Tắt" (primary xanh).

---

### 2.7. Giao diện người dùng (khi bảo trì bật)

- **Chưa đăng nhập / mở app:** Màn full-screen hoặc overlay: icon bảo trì + tiêu đề "Hệ thống đang bảo trì" + nội dung thông báo (đã nhập admin) + thời gian dự kiến hoàn thành + (tùy chọn) nút "Thử lại sau".
- **Đã đăng nhập, đang trong trận:** Cho hoàn thành trận; sau đó có thể hiện banner "Hệ thống đang bảo trì. Có thể xem kết quả nhưng không bắt đầu trận mới."

---

### 2.8. Loading & lỗi

- Bật/tắt: nút loading (spinner + disabled). Lỗi: toast đỏ "Thao tác thất bại. Vui lòng thử lại.", giữ nguyên trạng thái.

---

### 2.9. Ghi chú designer

- Màu bảo trì thống nhất (vàng/amber cảnh báo; đỏ nhạt nếu nhấn mạnh). Trạng thái "Đang bảo trì" rất rõ (badge nổi) để admin không quên tắt. Có thể thêm "Lần bật gần nhất: [ngày] bởi [admin]."
