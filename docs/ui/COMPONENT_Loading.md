# COMPONENT: LOADING (MÀN HÌNH CHỜ)

## 1. MỤC ĐÍCH

Đảm bảo người dùng nhận biết được hệ thống đang xử lý dữ liệu, tránh thao tác trùng lặp hoặc hiểu nhầm là ứng dụng bị treo.

## 2. CÁC DẠNG LOADING

### 2.1. Loading Toàn Màn Hình (Full Screen Loading)
- **Sử dụng khi:** 
  - Khởi động ứng dụng (Splash screen).
  - Chuyển cảnh giữa các màn hình lớn.
  - Xử lý tác vụ quan trọng chặn toàn bộ thao tác (ví dụ: Thanh toán, Đăng nhập).
- **Giao diện:**
  - Nền: Lớp overlay mờ (opacity 50-80%) che phủ toàn bộ nội dung.
  - Icon: Spinner xoay tròn hoặc Logo ứng dụng có hiệu ứng pulse/beat.
  - Text: "Đang tải dữ liệu...", "Vui lòng chờ...".

### 2.2. Loading Cục Bộ (Skeleton / Component Loading)
- **Sử dụng khi:**
  - Tải dữ liệu cho một phần cụ thể của màn hình (Danh sách, Card thông tin).
  - Giữ layout ổn định, không bị giật (layout shift).
- **Giao diện:**
  - Hình khối (Skeleton): Các khối màu trung tính mô phỏng cấu trúc nội dung (Avatar tròn, dòng text dài/ngắn).
  - Hiệu ứng: Animation sóng trượt (Shimmer effect) từ trái qua phải.

### 2.3. Loading Trên Nút (Button Loading State)
- **Sử dụng khi:**
  - Người dùng nhấn nút thực hiện hành động (Gửi form, Lưu cài đặt).
  - Ngăn chặn bấm nhiều lần (Double submission).
- **Giao diện:**
  - Trạng thái: Nút bị vô hiệu hóa (Disabled), mờ đi.
  - Icon: Spinner nhỏ xoay tròn thay thế hoặc nằm cạnh text của nút.
  - Text: Có thể đổi thành "Đang xử lý...".

## 3. QUY TẮC NGHIỆP VỤ

- **Thời gian phản hồi:**
  - Dưới 1s: Có thể không cần hiện loading hoặc chỉ hiện spinner nhỏ.
  - 1s - 5s: Bắt buộc hiện Loading để phản hồi trạng thái.
  - Quá 10s: Cần có phương án Timeout (Thông báo lỗi, nút Thử lại).
- **Hủy tác vụ:** Đối với các tác vụ không quan trọng (như tìm kiếm), cho phép người dùng hủy loading bằng cách nhấn nút "X" hoặc chạm ra ngoài (tùy ngữ cảnh).
- **Tính nhất quán:** Sử dụng cùng một bộ icon và hiệu ứng loading xuyên suốt ứng dụng.
