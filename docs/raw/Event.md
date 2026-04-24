## 3. Chức Năng và Giao diện: Sự Kiện (Events)

### Mục tiêu (Goal)
Tạo nội dung tươi mới, giới hạn thời gian để tăng tương tác người dùng (Engagement) và cung cấp các vật phẩm độc quyền.

### 3.1. Phạm Vi (Scope)
Hệ thống hỗ trợ 3 loại hình sự kiện chính:
1.  **Lucky Spin:** Vòng quay may mắn hàng ngày.
2.  **Milestone Challenge:** Vượt qua các ải câu hỏi khó để nhận quà.

[//]: # (3.  **Tournament Tasks:** Thực hiện các nhiệm vụ tích lũy &#40;số trận thắng, số câu đúng...&#41; để nhận thưởng.)

### 3.2. Quy Tắc Nghiệp Vụ (Business Logic)
*   **Tính chất:** 
    *   Giới hạn thời gian (Countdown timer).
    *   Sử dụng hệ thống phần thưởng riêng (Currency, Frame, Badge) không ảnh hưởng trực tiếp đến Rank Point chính để tránh lạm phát ELO.
*   **Điều kiện tham gia:**
    *   Giới hạn lượt chơi theo ngày (đối với Lucky Spin).
    *   Mở khóa theo cấp độ hoặc theo tiến độ ải trước (đối với Milestone).
*   **Cơ chế thưởng (Rewards):**
    *   Xác nhận qua API để đảm bảo an toàn.
    *   Người dùng nhận thưởng thủ công sau khi đạt yêu cầu.

### 3.3. Thành Phần Giao Diện (UI Components)
*   **Event List:** Trang quản lý tập trung các sự kiện đang diễn ra.
*   **Lucky Wheel:** Vòng quay vật lý với hiệu ứng quay 5 giây.
*   **Vertical Progress Bar:** Hiển thị lộ trình vượt ải trong Milestone Challenge.
*   **Reward Dialog:** Popup thông báo nhận thưởng sinh động với âm thanh.

### 3.4. Luồng Hoạt Động (Activity Flow)
1.  **Khám phá:** User nhấn Banner sự kiện tại màn hình Home.
2.  **Lựa chọn:** Hệ thống hiển thị danh sách các sự kiện đang Active.
3.  **Tương tác:** 
    *   *Spin:* Quay và nhận quà ngẫu nhiên.
    *   *Challenge:* Tham gia trận đấu đặc biệt với bộ câu hỏi định sẵn.
    *   *Task:* Chơi game như bình thường và quay lại nhận quà khi đủ tiến độ.
4.  **Kết thúc:** Nhận quà và theo dõi countdown thời gian kết thúc sự kiện.
