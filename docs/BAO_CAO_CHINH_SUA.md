# BÁO CÁO TỔNG HỢP CHỈNH SỬA VÀ TỐI ƯU HÓA TÀI LIỆU

## 1. TỔNG QUAN
Dựa trên yêu cầu phân tích, tối ưu và tổng hợp các mô tả từ thư mục `docs/raw`, tôi đã thực hiện tái cấu trúc lại toàn bộ hệ thống tài liệu UI và Logic. Mục tiêu là tạo ra bộ tài liệu nhất quán, rõ ràng, giảm thiểu xung đột và dễ dàng cho đội ngũ phát triển (Dev/Design) theo dõi.

## 2. DANH SÁCH CÁC TÀI LIỆU ĐÃ TẠO

### 2.1. Tài Liệu Giao Diện (docs/ui)
Tất cả các file UI đều tuân thủ cấu trúc: Mục đích -> Thành phần -> Luồng thao tác -> Quy tắc nhiệm vụ -> Responsive.

**Nhóm Người Dùng (User Client):**
1.  **[GD_DangNhap.md](docs/ui/GD_DangNhap.md):** Tối ưu hóa luồng xác thực, thêm xử lý quên mật khẩu và validation.
2.  **[GD_DangKy.md](docs/ui/GD_DangKy.md):** Bổ sung quy tắc mật khẩu mạnh và xác thực số điện thoại.
3.  **[GD_TrangChu.md](docs/ui/GD_TrangChu.md):** Tổng hợp thông tin người dùng, sự kiện và lối tắt vào thi đấu.
4.  **[GD_CauHinhThiDau.md](docs/ui/GD_CauHinhThiDau.md):** Làm rõ logic chọn chế độ chơi và chủ đề.
5.  **[GD_GhepTran.md](docs/ui/GD_GhepTran.md):** Chi tiết hóa trạng thái chờ và xử lý timeout.
6.  **[GD_ThiDauVuotAi.md](docs/ui/GD_ThiDauVuotAi.md):** Tách biệt logic thi đấu đơn (không giới hạn thời gian tổng).
7.  **[GD_ThiDauGhepTran.md](docs/ui/GD_ThiDauGhepTran.md):** Tách biệt logic thi đấu đối kháng (có ma trận câu hỏi, đồng bộ thời gian).
8.  **[GD_KetQua.md](docs/ui/GD_KetQua.md):** Hợp nhất hiển thị điểm thưởng và đáp án chi tiết.
9.  **[GD_BangXepHang.md](docs/ui/GD_BangXepHang.md):** Thêm bộ lọc thời gian và phạm vi bạn bè.
10. **[GD_SuKien.md](docs/ui/GD_SuKien.md):** Mô tả popup sự kiện và cơ chế tham gia.
11. **[GD_ThongTinCaNhan.md](docs/ui/GD_ThongTinCaNhan.md):** Bổ sung lịch sử đấu và thống kê chỉ số.

**Nhóm Quản Trị (Admin Dashboard):**
1.  **[GD_Admin_NguoiDung.md](docs/ui/GD_Admin_NguoiDung.md):** Tổng hợp xem, xóa, khóa trong một màn hình quản lý.
2.  **[GD_Admin_CauHoi.md](docs/ui/GD_Admin_CauHoi.md):** Quy trình duyệt câu hỏi (Pending -> Approved) rõ ràng hơn.
3.  **[GD_Admin_TranDau.md](docs/ui/GD_Admin_TranDau.md):** Công cụ giám sát real-time và log chống gian lận.
4.  **[GD_Admin_SuKien.md](docs/ui/GD_Admin_SuKien.md):** Cấu hình sự kiện linh hoạt theo thời gian.
5.  **[GD_Admin_CauHinh.md](docs/ui/GD_Admin_CauHinh.md):** Quản lý tham số hệ thống tập trung.

### 2.2. Tài Liệu Logic (docs/logic)
Tách biệt phần logic nghiệp vụ ra khỏi UI để dễ bảo trì/tham chiếu backend.
1.  **[Logic_NguoiDung.md](docs/logic/Logic_NguoiDung.md):** Định nghĩa Role, Auth flow, Exp formula.
2.  **[Logic_ThiDau.md](docs/logic/Logic_ThiDau.md):** Quy tắc tính điểm, Matchmaking ELO, Xử lý disconnect.
3.  **[Logic_XepHang.md](docs/logic/Logic_XepHang.md):** Hệ thống Rank Point, Tiers, Reset mùa giải.
4.  **[Logic_CauHoi.md](docs/logic/Logic_CauHoi.md):** Workflow duyệt câu hỏi, Randomization.
5.  **[Logic_SuKien.md](docs/logic/Logic_SuKien.md):** Loại sự kiện và phần thưởng riêng.

## 3. CÁC PHẦN ĐÃ CHỈNH SỬA VÀ GIẢI QUYẾT CONFLICT

Trong quá trình tổng hợp từ `docs/raw`, tôi đã phát hiện và xử lý các vấn đề sau:

### 3.1. Xung đột về Logic tính điểm
- **Ban đầu:** Có tài liệu nói cộng điểm theo thời gian, có tài liệu nói cộng cố định.
- **Giải quyết:** Thống nhất trong `Logic_ThiDau.md`:
  - Điểm cơ bản cố định theo độ khó.
  - Điểm thời gian chỉ áp dụng cho chế độ Ghép trận (để tạo cạnh tranh).
  - Chế độ Vượt ải không tính điểm thời gian (để người chơi tập trung suy nghĩ).

### 3.2. Xung đột về Quy trình duyệt câu hỏi
- **Ban đầu:** Mơ hồ về việc ai được duyệt, trạng thái câu hỏi.
- **Giải quyết:** Quy định rõ trong `Logic_CauHoi.md` và `GD_Admin_CauHoi.md`:
  - Editor chỉ được tạo Draft/Pending.
  - Chỉ Admin/Moderator mới được chuyển sang Approved.
  - Câu hỏi phải Approved mới được xuất hiện trong game.

### 3.3. Tối ưu hóa Giao diện Thi đấu
- **Vấn đề:** Giao diện thi đấu mô tả chung chung cho cả 2 chế độ gây khó hiểu.
- **Giải quyết:** Tách thành 2 file `GD_ThiDauVuotAi.md` (đơn giản, tuyến tính) và `GD_ThiDauGhepTran.md` (phức tạp, có ma trận câu hỏi, đồng bộ).
- **Bổ sung:** Thêm tính năng "Ma trận câu hỏi" cho chế độ ghép trận để người chơi chiến thuật hơn trong việc chọn câu làm trước.

### 3.4. Bổ sung Admin Dashboard
- **Vấn đề:** Tài liệu cũ mô tả phân tán các chức năng quản lý.
- **Giải quyết:** Gom nhóm lại thành các Module quản lý (Người dùng, Câu hỏi, Trận đấu) với UI thống nhất (Filter, Table, Action).

### 3.5. Quy hoạch lại Menu
- **Ban đầu:** Menu điều hướng chưa rõ ràng.
- **Giải quyết:** Thống nhất Bottom Navigation cho Mobile (Trang chủ, Thi đấu, BXH, Cá nhân) để dễ thao tác 1 tay.

## 4. KHUYẾN NGHỊ TIẾP THEO
- **Design:** Dựa trên các file `docs/ui` để thiết kế Wireframe/Mockup chi tiết.
- **Backend:** Dựa trên `docs/logic` để thiết kế Database Schema và API Swagger.
- **Frontend:** Implement các Component tái sử dụng (Button, Card, Dialog) theo mô tả.
