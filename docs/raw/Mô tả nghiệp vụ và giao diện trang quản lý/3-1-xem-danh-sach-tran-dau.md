# 3.1. XEM DANH SÁCH TRẬN ĐẤU

## 1. NGHIỆP VỤ

### Mục đích
Admin xem tất cả trận đấu đã diễn ra và đang diễn ra.

### Luồng xử lý
1. Admin truy cập "Quản lý trận đấu"
2. Hệ thống load danh sách trận đấu: mặc định trận đấu hôm nay; lọc theo ngày, người chơi, chủ đề, trạng thái; tìm kiếm theo ID trận đấu, tên người chơi
3. Hiển thị danh sách: ID trận đấu, tên 2 người chơi, chủ đề, thời gian bắt đầu/kết thúc, kết quả (người thắng, điểm số), trạng thái (Đang diễn ra / Đã kết thúc / Bị hủy)
4. Admin click vào một trận đấu để xem chi tiết

### Quy tắc nghiệp vụ
- Mặc định hiển thị trận đấu hôm nay
- Trận đấu đang diễn ra được highlight
- Trận đấu bị hủy được đánh dấu màu đỏ

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Loại trang:** Trang danh sách trận đấu. Cấu trúc: Header (sticky) "Quản lý trận đấu" + Export (tùy chọn) → Toolbar (tìm kiếm, lọc ngày/chủ đề/trạng thái, sắp xếp, Áp dụng/Làm mới) → Bảng hoặc danh sách card trận đấu → Phân trang. Nền trang nhạt; nội dung chính trong vùng trắng, bo góc và shadow nhẹ.

---

### 2.2. Thanh công cụ và bảng

- **Toolbar:** Ô tìm kiếm (ID trận, tên người chơi, debounce); bộ lọc ngày (Từ–Đến, mặc định hôm nay); dropdown Chủ đề, Trạng thái (Tất cả, Đang diễn ra, Đã kết thúc, Bị hủy), Sắp xếp; nút Áp dụng, Làm mới. Mobile: gom thành nút "Bộ lọc" mở drawer/modal.
- **Bảng:** Cột: ID | Người chơi 1 (avatar + tên) | Người chơi 2 | Chủ đề (badge) | Thời gian (bắt đầu–kết thúc) | Kết quả (tên thắng – tỉ số hoặc "Đang diễn ra") | Trạng thái (badge: Đang diễn ra nổi bật, Đã kết thúc, Bị hủy đỏ) | Hành động (Xem chi tiết). Click hàng (trừ cột hành động) → Chi tiết trận đấu (file 3-2).

---

### 2.3. Empty state, Loading, Lỗi, Phân trang, Responsive

- **Empty:** Không có trận thỏa bộ lọc → icon + "Không có trận đấu nào", mô tả ngắn, nút "Xóa bộ lọc".
- **Loading:** Skeleton hàng trong bảng hoặc spinner giữa vùng nội dung.
- **Lỗi:** Banner cảnh báo + "Không thể tải danh sách trận đấu." + nút "Thử lại".
- **Phân trang:** Text "Hiển thị X–Y trong Z trận đấu", nút Trước/Sau, số trang, chọn số dòng/trang (20, 50, 100).
- **Mobile:** Danh sách card (ID, 2 người chơi, chủ đề, trạng thái, kết quả); chạm card → chi tiết. Toolbar → nút Bộ lọc; phân trang đơn giản.
