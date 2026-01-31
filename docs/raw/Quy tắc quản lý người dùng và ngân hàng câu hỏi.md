# QUY TẮC NGHIỆP VỤ: QUẢN LÝ NGƯỜI DÙNG VÀ NGÂN HÀNG CÂU HỎI

## 1. QUẢN LÝ NGƯỜI DÙNG

### 1.1. Quy tắc chung (danh sách, xem chi tiết)

| Nội dung | Quy tắc |
|----------|--------|
| **Hiển thị mặc định** | Danh sách mặc định: tất cả người dùng **active**; sắp xếp mặc định: **ngày đăng ký mới nhất**. |
| **Phân trang** | 20–50 người dùng/trang; mặc định 20/trang. |
| **Thông tin nhạy cảm** | Admin **không được xem** mật khẩu, token. Chỉ xem thông tin công khai. |
| **Thống kê** | Thống kê (điểm, xếp hạng, tỷ lệ thắng, lịch sử trận) tính **real-time** từ database. |

### 1.2. Validation – Danh sách & tìm kiếm

| Trường / Hành vi | Ràng buộc |
|------------------|-----------|
| Tìm kiếm (tên, email, ID, SĐT) | Tối thiểu **2 ký tự**; không phân biệt hoa thường. |
| Bộ lọc khoảng thời gian đăng ký | Không quá **1 năm**. |

### 1.3. Khóa / Mở khóa tài khoản

| Nội dung | Quy tắc |
|----------|--------|
| **Phân quyền** | Chỉ **super admin** mới được khóa tài khoản **admin khác**. Admin đang đăng nhập không phải super admin → ẩn/disable nút Khóa với đối tượng là admin. |
| **Hành vi khi bị khóa** | Người dùng bị khóa **không thể đăng nhập**, **không thể tham gia thi đấu**. |
| **Lý do khóa** | Bắt buộc; **10–500 ký tự**. |
| **Thời gian khóa** | Tạm thời: 1 / 7 / 30 ngày (số dương); hoặc Vĩnh viễn. |
| **Tự động mở khóa** | Khóa tạm thời: hệ thống **tự động mở khóa** khi hết thời gian (scheduled job). |

### 1.4. Xóa tài khoản

| Nội dung | Quy tắc |
|----------|--------|
| **Phân quyền** | Chỉ **super admin** có quyền xóa tài khoản. **Không được xóa** tài khoản admin khác. |
| **Điều kiện** | **Không được xóa** tài khoản **đang trong trận đấu**. Nếu đang trong trận → thông báo rõ và disable nút Xóa. |
| **Cách xóa** | **Soft delete**: đánh dấu `deleted_at`; không xóa vật lý. Ẩn tài khoản khỏi danh sách; anonymize dữ liệu cá nhân (GDPR). Dữ liệu thi đấu giữ lại nhưng ẩn danh (vd. "Người chơi đã xóa"). |
| **Lý do xóa** | Bắt buộc; **20–500 ký tự**. |
| **Xác nhận** | Bắt buộc nhập **mật khẩu admin** đúng để xác nhận. |

### 1.5. Reset mật khẩu

| Nội dung | Quy tắc |
|----------|--------|
| **Link reset qua email** | Token reset có hiệu lực **24 giờ**. |
| **Mật khẩu tạm thời** | Chỉ hiển thị **1 lần** cho admin; gửi qua email cho người dùng. Người dùng **phải đổi mật khẩu** khi đăng nhập lần đầu với mật khẩu tạm thời. |
| **Giới hạn** | Mỗi tài khoản reset tối đa **3 lần/ngày**. |
| **Điều kiện** | Tài khoản phải **active**; email hợp lệ. |

### 1.6. Xử lý báo cáo người dùng

| Nội dung | Quy tắc |
|----------|--------|
| **Thứ tự ưu tiên xử lý** | Gian lận > Spam > Hành vi không phù hợp; kèm theo số lượng báo cáo về cùng một người và thời gian báo cáo. |
| **Tự động** | **3 báo cáo** về cùng một người **trong 30 ngày** → tự động khóa tạm thời (vd. 7 ngày). **Bị khóa 3 lần** (tùy chính sách) → có thể áp dụng khóa vĩnh viễn. |
| **Thời hạn xử lý** | Admin nên xử lý báo cáo **trong 48 giờ**. |
| **Lý do xử lý** | Bắt buộc; **10–500 ký tự**. Phải chọn **đúng một** hành động: Cảnh báo / Khóa tạm thời / Khóa vĩnh viễn / Bỏ qua. |

---

## 2. QUẢN LÝ NGÂN HÀNG CÂU HỎI

### 2.1. Quy tắc chung – Trạng thái và quyền

| Nội dung | Quy tắc |
|----------|--------|
| **Trạng thái câu hỏi** | **Draft** (nháp), **Pending** (chờ duyệt), **Approved** (đã duyệt), **Rejected** (từ chối), **Needs Revision** (yêu cầu chỉnh sửa). |
| **Sử dụng trong thi đấu** | Chỉ câu hỏi **Approved** mới được dùng trong thi đấu. |
| **Visibility** | Draft: chỉ **người tạo** thấy. Pending: **Moderator/Admin** đều thấy. |
| **Gửi duyệt** | Editor có thể lưu nháp; **Moderator/Admin** mới có thể gửi duyệt (Pending). Câu hỏi mới tạo phải **duyệt** mới dùng trong thi đấu. |
| **Metadata** | Tự động gán **người tạo** và **ngày tạo**. |

### 2.2. Thêm câu hỏi mới

| Trường / Hành vi | Ràng buộc |
|------------------|-----------|
| Nội dung câu hỏi | Bắt buộc; **10–500 ký tự**. |
| Đáp án (A, B, C, D) | Mỗi đáp án **1–200 ký tự**; bắt buộc **đủ 4 đáp án**; **ít nhất 1 đáp án đúng** (hoặc nhiều hơn nếu loại "chọn nhiều đáp án đúng"). |
| Điểm số | **6–10** (số nguyên). |
| Thời gian làm bài | **10–300 giây**. |
| File audio | mp3, wav; **tối đa 5MB**. |
| File ảnh | jpg, png; **tối đa 2MB**. |

### 2.3. Chỉnh sửa câu hỏi

| Nội dung | Quy tắc |
|----------|--------|
| **Đang trong trận đấu** | Nếu câu hỏi **đang được sử dụng** trong trận đấu đang diễn ra → không sửa trực tiếp; có thể **tạo bản sao** để chỉnh sửa. |
| **Sau khi sửa câu đã duyệt** | Câu hỏi đã Approved khi sửa → tự động chuyển về **Pending** (cần duyệt lại); gửi thông báo Moderator. |
| **Lịch sử** | Giữ **version control**: lưu phiên bản cũ (vd. QuestionVersions); có thể khôi phục phiên bản cũ. |
| **Validation** | Giống thêm câu hỏi mới; thêm kiểm tra **quyền chỉnh sửa**. |

### 2.4. Xóa câu hỏi

| Nội dung | Quy tắc |
|----------|--------|
| **Cách xóa** | **Soft delete**: đánh dấu `deleted_at`; ẩn khỏi danh sách active; vẫn lưu trong DB (thống kê, lịch sử). |
| **Sử dụng** | Câu đã xóa **không** dùng trong trận đấu **mới**; vẫn hiển thị trong **lịch sử trận đấu cũ**. |
| **Khôi phục** | Có thể khôi phục câu hỏi đã xóa **trong vòng 30 ngày**. |
| **Điều kiện** | **Không được xóa** nếu câu hỏi **đang trong trận đấu đang diễn ra**. |
| **Lý do xóa** | Bắt buộc; **10–200 ký tự**. |

### 2.5. Duyệt câu hỏi

| Nội dung | Quy tắc |
|----------|--------|
| **Người duyệt** | Chỉ **Moderator/Admin**. Một câu hỏi chỉ do **một** Moderator duyệt (tránh duplicate; có thể đánh dấu "Đang xem xét"). |
| **Kết quả** | **Duyệt** → Approved. **Từ chối** → Rejected (nhập lý do). **Yêu cầu chỉnh sửa** → Needs Revision (nhập yêu cầu); sau khi người tạo sửa → chuyển lại Pending. |
| **Lý do** | Từ chối: bắt buộc **10–500 ký tự**. Yêu cầu chỉnh sửa: bắt buộc **10–500 ký tự**. |

### 2.6. Import câu hỏi (Excel/CSV)

| Nội dung | Quy tắc |
|----------|--------|
| **File** | Định dạng xlsx, csv; **tối đa 10MB**; **tối đa 1000 câu/lần** import. |
| **Cấu trúc** | Mỗi dòng = 1 câu hỏi; đủ cột bắt buộc (Nội dung, Chủ đề, Mức độ khó, Điểm, Thời gian, Đáp án A–D, Đáp án đúng, v.v.). |
| **Nội dung** | 10–500 ký tự. Đáp án đúng: **A, B, C, D** (hoặc nhiều, cách nhau dấu phẩy). |
| **Sau import** | Câu hỏi import mặc định trạng thái **Pending**. Dòng lỗi **bỏ qua**, không dừng import; báo cáo chi tiết thành công/lỗi. |

### 2.7. Quản lý chủ đề

| Nội dung | Quy tắc |
|----------|--------|
| **Thêm/Sửa chủ đề** | Tên: bắt buộc, **3–50 ký tự**, **unique**. Icon/ảnh: jpg, png, **tối đa 500KB**. |
| **Xóa chủ đề** | **Không** xóa chủ đề nếu có câu hỏi **đang được sử dụng** trong trận đấu. Khi xóa: nếu chủ đề có câu hỏi → bắt buộc chọn **Xóa tất cả câu hỏi thuộc chủ đề** HOẶC **Chuyển câu hỏi sang chủ đề khác** (chọn chủ đề đích). |
| **Thi đấu** | Chủ đề phải có **tối thiểu 10 câu hỏi đã duyệt (Approved)** mới được dùng trong thi đấu (có thể cảnh báo/gợi ý bổ sung câu hỏi). |

---

## 3. TÓM TẮT PHÂN QUYỀN

| Vai trò | Quản lý người dùng | Ngân hàng câu hỏi |
|---------|--------------------|--------------------|
| **Super Admin** | Xem, khóa/mở khóa, xóa tài khoản, reset mật khẩu, xử lý báo cáo. | Toàn quyền (thêm, sửa, xóa, duyệt, import, chủ đề). |
| **Admin** | Xem, khóa/mở khóa (trừ admin khác), reset mật khẩu, xử lý báo cáo. Không xóa tài khoản. | Toàn quyền câu hỏi và chủ đề. |
| **Moderator** | Xem, xử lý báo cáo; có thể khóa/mở khóa (trừ admin). | Duyệt câu hỏi (Pending); xem, sửa; không nhất thiết xóa/import tùy cấu hình. |
| **Editor** | Chỉ xem (nếu được cấp). | Thêm, sửa câu hỏi; lưu nháp, gửi duyệt; không duyệt. |
| **Viewer** | Chỉ xem. | Chỉ xem. |
