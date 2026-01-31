# GIAO DIỆN BẠN BÈ

## 1. MỤC ĐÍCH

Cho phép người dùng quản lý danh sách bạn bè, tìm kiếm người chơi khác và gửi/nhận lời mời kết bạn để tăng tính tương tác xã hội trong game.

## 2. CÁC THÀNH PHẦN GIAO DIỆN

### 2.1. Phần Header
- Tiêu đề: "Bạn Bè"
- Tabs điều hướng:
  - Tab 1: Danh sách bạn bè (Mặc định)
  - Tab 2: LLời mời kết bạn (Kèm badge số lượng lời mời chưa xử lý)
  - Tab 3: Tìm kiếm

### 2.2. Tab Danh Sách Bạn Bè
- Ô tìm kiếm nhanh: Lọc người dùng trong danh sách bạn bè hiện có.
- Danh sách bạn bè (List View):
  - Avatar & Tên người dùng
  - Trạng thái: Online (Chấm xanh) / Offline (x phút trước) / Đang trong trận
  - Level / Rank hiện tại.
  - Nút hành động (Menu ...):
    - Xem hồ sơ
    - Mời thi đấu (Nếu đang Online)
    - Chat nhanh (Nếu có tính năng chat)
    - Hủy kết bạn (Unfriend)
- Trạng thái trống: "Bạn chưa có người bạn nào. Hãy tìm kiếm thêm bạn mới!"

### 2.3. Tab Lời Mời Kết Bạn
- Danh sách lời mời đã nhận:
  - Thông tin người gửi: Avatar, Tên, Level.
  - Thời gian gửi: "2 giờ trước".
  - Nút "Chấp nhận": Primary button.
  - Nút "Từ chối": Secondary button.
- Danh sách lời mời đã gửi (Optional): Xem lại các lời mời mình đã gửi đi.

### 2.4. Tab Tìm Kiếm Bạn Bè
- Form tìm kiếm:
  - Input: Nhập ID người dùng, Tên nhân vật, hoặc Email/SĐT (tùy chính sách bảo mật).
  - Nút "Tìm kiếm".
- Kết quả tìm kiếm:
  - Hiển thị danh sách user khớp với từ khóa.
  - Thông tin tóm tắt: Avatar, Tên, Level.
  - Nút hành động:
    - "Kết bạn" (Nếu chưa là bạn và chưa gửi lời mời).
    - "Đã gửi " (Disabled - Nếu đã gửi lời mời).
    - "Bạn bè" (Disabled - Nếu đã là bạn).

## 3. LUỒNG THAO TÁC

### 3.1. Gửi Lời Mời Kết Bạn (Chủ động)
1. Vào tab "Tìm kiếm".
2. Nhập tên hoặc ID đối phương.
3. Nhấn "Tìm kiếm".
4. Nhấn nút "Kết bạn" trên kết quả mong muốn.
5. Hệ thống gửi thông báo/yêu cầu đến đối phương. Nút chuyển sang "Đã gửi".

### 3.2. Chấp Nhận Lời Mời
1. Nhận thông báo (Notification) hoặc thấy badge đỏ ở tab Lời mời.
2. Vào tab "Lời mời kết bạn".
3. Nhấn "Chấp nhận" ở dòng người muốn kết bạn.
4. Hệ thống cập nhật:
   - Thêm người đó vào Danh sách bạn bè.
   - Xóa khỏi danh sách lời mời.
   - Gửi thông báo cho đối phương "A đã chấp nhận lời mời kết bạn".

### 3.3. Hủy Kết Bạn / Từ Chối
- **Từ chối:** Nhấn nút "Từ chối" tại tab Lời mời -> Yêu cầu bị xóa.
- **Hủy kết bạn:** Tại tab Danh sách bạn bè -> Chọn Menu -> "Hủy kết bạn" -> Xác nhận Dialog -> Xóa quan hệ bạn bè.

### 3.4. Kết Bạn Sau Trận Đấu
1. (Thực hiện tại Giao diện Kết quả trận đấu)
2. Người dùng nhấn icon "Thêm bạn" bên cạnh tên đối thủ.
3. Hệ thống gửi lời mời kết bạn tương tự như quy trình Tìm kiếm -> Gửi lời mời.

## 4. QUY TẮC NGHIỆP VỤ

- **Giới hạn bạn bè:** Tối đa 50 hoặc 100 bạn (tùy cấu hình).
- **Trạng thái Online:** Cập nhật realtime hoặc heart-beat mỗi 1-2 phút.
- **Spam:** Giới hạn số lượng lời mời gửi đi trong ngày để tránh spam.
- **Block:** Nếu user A chặn user B, không thể tìm thấy hoặc gửi lời mời cho nhau.
