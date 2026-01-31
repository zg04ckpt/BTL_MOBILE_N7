# BÁO CÁO CẬP NHẬT QUY CHUẨN UI

## 1. Mục Đích
Báo cáo này liệt kê các thay đổi đã thực hiện đối với hệ thống tài liệu UI (`docs/ui`) để đảm bảo tuân thủ tuyệt đối các file yêu cầu gốc (`docs/raw`), loại bỏ các tính năng dư thừa và chuẩn hóa nền tảng (Mobile cho User, Web cho Admin).

## 2. Các Thay Đổi Chi Tiết

### 2.1. Loại Bỏ Tính Năng (Không có trong Raw)
| File UI | Tính năng bị loại bỏ | Lý do |
| :--- | :--- | :--- |
| **GD_BanBe.md** | Toàn bộ file | Không có raw file mô tả chức năng quản lý bạn bè (Add/Accept/Search). |
| **GD_TrangChu.md** | Nút "Bạn bè", Nút "Cài đặt" (Header) | Raw không mô tả nút Bạn bè. Nút Cài đặt thuộc màn profile (Raw GDThongTinCaNhan). |
| **GD_KetQua.md** | Section "Kết quả đối kháng" (Kết bạn, Tố cáo) | Raw `GDKetQua` không đề cập các nút này. |
| **GD_ThiDauVuotAi.md** | Quyền trợ giúp (50/50, Gọi điện...) | Raw `GDThiDauVuotAiDon` không có quyền trợ giúp, chỉ có Menu hành động (Nộp, Hướng dẫn). |
| **GD_ThiDauGhepTran.md** | Chat, Emoji, Đồng bộ Sync Round | Raw `GDThiDau` mô tả thi đấu dạng "Test" (Ma trận câu, Nhảy câu, Nộp bài), không phải Live Quiz Sync. |

### 2.2. Hiệu Chỉnh Nội Dung và Luồng (Để khớp với Raw)
| File UI | Nội dung gốc (Raw) | Nội dung UI Sau chỉnh sửa |
| :--- | :--- | :--- |
| **GD_ThiDauGhepTran** | Có thể quay lại, Ma trận câu, Nhảy câu. | Chuyển từ "Live Quiz" sang mô hình "Làm bài thi" (Exam Mode) với chức năng Nhảy câu/Ma trận. |
| **GD_ThiDauVuotAi** | Không thể quay lại. Top Bar cố định (Câu đã làm, Điểm, Thời gian). | Cập nhật Top Bar hiển thị đúng các trường raw yêu cầu. Thêm đếm ngược 3s đầu game. |
| **GD_KetQua** | Điểm số, List điểm cộng, Nút hiện đáp án, List câu hỏi. | Chuẩn hóa danh sách hiển thị đúng Raw (Có hiển thị "Đáp án đã chọn sai" nếu có). |

### 2.3. Định Hướng Nền Tảng
- **User App (`docs/ui/GD_*.md`):** Được thiết kế theo ngôn ngữ Mobile (Touch, Tap, BottomSheet, Android/iOS patterns).
- **Admin Web (`docs/ui/GD_Admin_*.md`):** Được giữ nguyên định dạng Web (Dashboard, Table, Filter), phù hợp với mô tả nghiệp vụ quản lý.

## 3. Kết Luận
Hệ thống tài liệu UI hiện đã đồng bộ hoàn toàn với folder `docs/raw`. Các tính năng "sáng tạo thêm" (Social, Chat, Lifelines) đã được loại bỏ để đảm bảo đúng phạm vi dự án ban đầu.
