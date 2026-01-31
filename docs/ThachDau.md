## 2. Chức Năng và Giao Diện Chờ,Thách Đấu (Matchmaking)

###  Mục tiêu (Goal)
Tìm kiếm đối thủ tương đồng trình độ trong thời gian thực và chuẩn bị tài nguyên trước khi vào trận.


### 2.1. Phạm Vi (Scope)
* Thuật toán tìm kiếm đối thủ (Matchmaking Algorithm).
* Lựa chọn chủ đề (Topic Selection).
* Phòng chờ (Lobby/Waiting Room).


###  2.2. Quy Tắc Nghiệp Vụ (Business Logic)
* **Logic ghép cặp (Matching Logic):**
    * Dựa trên **ELO/Rank**: Tìm trong khoảng `+/- 100` điểm.
    * **Expand Search:** Sau 10s không tìm thấy -> Mở rộng khoảng chênh lệch điểm.
    * **Topic Rule:**
        * Chọn "Tự do": Ghép với user chọn "Tự do".
        * Chọn "Chủ đề cụ thể" (VD: Sử): Ghép với user chọn "Sử".
    * **Bot Fallback:** Sau 30s (Timeout) không tìm thấy -> Gợi ý đấu với Bot (AI có ELO tương đương).
* **Trạng thái sẵn sàng (Ready State):**
    * Khi ghép thành công -> Auto-start sau 5s đếm ngược.
    * **Pre-loading Assets:** Bắt buộc tải xong 100% tài nguyên (Ảnh/Audio) mới bắt đầu game để đảm bảo công bằng.



###  2.3. Thành Phần Giao Diện (UI Components)

**Màn hình 1: Thiết lập & Tìm kiếm (Searching Screen)**

* **Topic Selector:** Dropdown/Carousel chọn chủ đề (Khoa học, Lịch sử, Tự do...).
* **Play Button:** Nút kích thước lớn, Call-to-action chính.
* **Searching State:**
    * Left: Avatar User.
    * Right: Radar Animation / Hidden Avatar.
    * Text: "Đang tìm đối thủ..." + Timer đếm ngược.
    * Button: "Hủy tìm kiếm" (Cancel).

**Màn hình 2: Đã tìm thấy (Match Found - VS Screen)**
* **Animation:** Hiệu ứng "VS" chuyển cảnh kịch tính.
* **Info Cards:** Hiển thị rõ Avatar, Tên, Level của 2 bên.
* **Loading Bar:** "Đang tải câu hỏi..." (Hiển thị % load của cả mình và đối thủ).

### 2.4. Luồng Hoạt Động (Activity Flow)
1.  User chọn Chủ đề -> Nhấn **"Tìm trận"**.
2.  Client gửi sự kiện `socket.emit('find_match', { topic, elo })`.
3.  Server đưa User vào **Queue**.
    * **Case A (Thành công):** Server trả về `match_found` -> Client chuyển sang màn hình VS -> Pre-load Assets -> Vào Game.
    * **Case B (Hủy):** User nhấn Hủy -> `socket.emit('leave_queue')` -> Về Home.
    * **Case C (Timeout):** Sau 30s -> Hiển thị Popup "Đấu với Bot?".

---
