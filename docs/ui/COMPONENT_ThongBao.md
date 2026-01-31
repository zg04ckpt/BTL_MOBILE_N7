# COMPONENT: THÔNG BÁO (NOTIFICATIONS)

## 1. MỤC ĐÍCH

Cung cấp thông tin phản hồi cho người dùng về trạng thái của các hành động (thành công, thất bại, cảnh báo) hoặc yêu cầu xác nhận.

## 2. CÁC DẠNG THÔNG BÁO

### 2.1. Toast Message (Thông báo nhanh)
- **Mục đích:** Thông báo kết quả hành động nhẹ nhàng, không chặn thao tác.
- **Sử dụng khi:** Đăng nhập thành công, Lưu thay đổi, Copy text, Lỗi kết nối nhẹ.
- **Giao diện:**
  - Vị trí: Góc trên cùng (Top) hoặc dưới cùng (Bottom) màn hình, nổi lên trên nội dung.
  - Màu sắc:
    - Xanh lá (Success): Thành công.
    - Đỏ (Error): Thất bại/Lỗi.
    - Vàng (Warning): Cảnh báo.
    - Xanh dương/Xám (Info): Thông tin chung.
  - Icon: Tương ứng với loại thông báo (Check V, Dấu X, Dấu chấm than).
  - Nội dung: Text ngắn gọn (1-2 dòng).
- **Hành vi:**
  - Tự động biến mất sau 3-5 giây.
  - Có thể vuốt để tắt nhanh.

### 2.2. Alert Dialog (Hộp thoại cảnh báo)
- **Mục đích:** Thu hút sự chú ý tuyệt đối, yêu cầu người dùng đọc và phản hồi.
- **Sử dụng khi:** Xác nhận xóa, Lỗi hệ thống nghiêm trọng, Thông báo bắt buộc đọc.
- **Giao diện:**
  - Popup nằm giữa màn hình.
  - Nền sau bị làm mờ (Dimmed background) và chặn thao tác.
  - Tiêu đề: Ngắn gọn, rõ ràng (Ví dụ: "Xác nhận xóa").
  - Nội dung: Giải thích chi tiết hậu quả.
  - Nút bấm:
    - Nút chính (Hành động): "Xóa", "Đồng ý".
    - Nút phụ (Hủy): "Hủy bỏ", "Đóng".
- **Hành vi:** Không tự đóng, bắt buộc người dùng chọn một hành động.

### 2.3. Modal / Bottom Sheet (Bảng thông tin trượt)
- **Mục đích:** Hiển thị nội dung chi tiết hoặc form nhập liệu phụ mà không rời khỏi màn hình chính.
- **Sử dụng khi:** Chọn bộ lọc, Xem chi tiết nhanh, Menu tùy chọn.
- **Giao diện:**
  - Trượt từ dưới lên (Mobile) hoặc Popup lớn (Desktop).
  - Có nút đóng (X) hoặc vuốt xuống để đóng.

## 3. QUY TẮC NGHIỆP VỤ

- **Ưu tiên:** Alert Dialog > Toast Message.
- **Nội dung:**
  - Tránh dùng ngôn ngữ kỹ thuật (Code lỗi) với người dùng cuối.
  - Luôn gợi ý hướng giải quyết nếu là thông báo lỗi (Ví dụ: "Kiểm tra lại kết nối mạng").
- **Tần suất:** Tránh hiện quá nhiều Toast cùng lúc (Spam thông báo). Nếu có nhiều lỗi, nên gom lại hoặc hiển thị dạng list trong Dialog.
