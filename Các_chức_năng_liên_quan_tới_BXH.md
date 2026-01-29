#  Tài Liệu Đặc Tả Chức Năng Game (Game Feature Specifications)

Tài liệu này mô tả chi tiết phạm vi (Scope), quy tắc nghiệp vụ (Business Rules), giao diện (UI) và luồng hoạt động (Activity Flow) cho 3 chức năng cốt lõi: **Bảng Xếp Hạng**.

---

## 1. Chức Năng: Bảng Xếp Hạng (Leaderboard)

###  Mục tiêu (Goal)
Hiển thị thứ hạng người chơi dựa trên thành tích tích lũy để kích thích tính cạnh tranh (Competitiveness) và giữ chân người dùng (Retention).

### 1.1. Phạm Vi & Dữ Liệu (Scope & Data)
* **Phân loại theo thời gian:**
    * Tuần (Weekly)
    * Tháng (Monthly)
    * Tổng (All-time)
* **Phân loại theo quan hệ:**
    * Toàn Server (Global)
    * Bạn bè (Friends)
* **Cơ chế làm mới (Reset Mechanism):**
    * Reset vào `00:00` ngày đầu tuần/tháng.
    * *Hoặc:* Chia theo "Mùa giải" (Season) để reset định kỳ, tạo cơ hội cho người chơi mới (Newbies).

###  1.2. Quy Tắc Nghiệp Vụ (Business Logic)
**Cách tính điểm xếp hạng (Ranking Point - RP):**
* **Hệ thống điểm:** Độc lập với điểm số trong trận đấu (In-game score).
* **Quy tắc cộng/trừ:**
    * Thắng (Win): `+20 RP`
    * Thua (Loss): `-10 RP`
    * Hòa (Draw): `+5 RP`

**Thuật toán xếp hạng (Sorting Algorithm):**
Ưu tiên theo thứ tự giảm dần (Descending Order):
1.  **Priority 1:** Tổng điểm RP cao nhất.
2.  **Priority 2:** Tỷ lệ thắng (Win Rate) cao hơn (nếu RP bằng nhau).
3.  **Priority 3:** Chuỗi thắng (Winning Streak) dài hơn.
4.  **Priority 4:** Thời gian đạt điểm sớm hơn (Time stamp).
5.  **Fallback:** Nếu tất cả trùng nhau -> Xếp đồng hạng.

**Hiển thị:**
* Chỉ hiển thị **Top 25** trên UI.
* Người chơi ngoài Top 25: Hiển thị thứ hạng cụ thể (VD: #10,502) ở thanh cố định.

###  1.3. Thành Phần Giao Diện (UI Components)
* **Header Filter:** Tab chuyển đổi [Tuần | Tháng | Tổng] và [Global | Bạn bè].
* **Top 3 Podium:** Khu vực vinh danh Top 1, 2, 3 (Avatar lớn + Khung viền đặc biệt + Hiệu ứng Glow).
* **List View (Danh sách):**
    | Cột | Nội dung | Chi tiết |
    | :--- | :--- | :--- |
    | 1 | Rank | Số thứ tự (4, 5, 6...) |
    | 2 | Info | Avatar + Tên + Level |
    | 3 | Sub-stat | Win rate / Streak |
    | 4 | Point | Tổng RP |
* **Sticky Bottom Bar:** Thanh cố định hiển thị thứ hạng của chính User (My Rank).

###  1.4. Luồng Hoạt Động (Activity Flow)
1.  User nhấn icon **"Leaderboard"** tại Home.
2.  Hệ thống gọi API `GET /api/leaderboard?type=weekly&scope=global`.
3.  Client hiển thị danh sách Top 25 và highlight vị trí User ở Bottom Bar.
4.  User đổi Tab -> Client gọi API reload dữ liệu tương ứng.
5.  User click vào item người chơi bất kỳ -> Mở Popup **"User Profile"**.

---

## 2. Chức Năng: Giao Diện Chờ & Thách Đấu (Matchmaking)

###  Mục tiêu (Goal)
Tìm kiếm đối thủ tương đồng trình độ trong thời gian thực và chuẩn bị tài nguyên trước khi vào trận.


-----
# ***Có thể bỏ qua nghiệp vụ đoạn này vì chỉ quan tâm đến giao diện***
----

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


-----
# ***Giao diện tại đây***
---

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

## 3. Chức Năng và Giao diện :  Sự Kiện (Events)

###  Mục tiêu (Goal)
Tạo nội dung tươi mới, giới hạn thời gian để tăng tương tác người dùng (Engagement).

-----
# ***Có thể bỏ qua nghiệp vụ đoạn này vì chỉ quan tâm đến giao diện***
----

###  3.1. Phạm Vi (Scope)
* Sự kiện giới hạn thời gian (Limited-time events).
* Chế độ chơi đặc biệt: Marathon, Survival (Sudden Death), Chủ đề Trending.

###  3.2. Quy Tắc Nghiệp Vụ (Business Logic)
* **Tính chất:**
    * Không tính vào Rank Point chính (tránh lạm phát ELO).
    * Sử dụng hệ thống phần thưởng riêng (Currency, Frame, Badge).
* **Điều kiện (Constraints):**
    * Vé tham dự (Ticket) hoặc Giới hạn lượt chơi (VD: 3 lượt free/ngày).
* **Cơ chế thưởng (Rewards):**
    * **Milestone:** Đạt mốc 5 câu/10 câu -> Nhận quà ngay.
    * **Event Leaderboard:** Top 10 người cao điểm nhất sự kiện nhận Huy hiệu độc quyền. Hoặc được đặc quyền hoặc gợi ý gì đó khi tham gia trận đấu.

-----
# ***Giao diện tại đây***
---

###  3.3. Thành Phần Giao Diện (UI Components)
* **Home Banner List:** Slide trượt ngang các sự kiện đang Active.
* **Event Detail Popup:**
    * Banner & Title.
    * Rule Description & Countdown Timer.
    * Reward List (Danh sách quà).
    * Button "Tham gia ngay" (Kèm hiển thị số vé/lượt còn lại).
* **In-game Progress:** Thanh tiến trình hiển thị mốc quà theo thời gian thực.

### 3.4. Luồng Hoạt Động (Activity Flow)
1.  User nhấn Banner sự kiện ở Home.
2.  Hệ thống hiển thị Popup chi tiết.
3.  User nhấn **"Tham gia"**:
    * Kiểm tra số dư Vé/Lượt chơi.
    * Nếu đủ -> Trừ vé -> Start Game (Bộ câu hỏi riêng của Event).
    * Nếu thiếu -> Gợi ý mua thêm hoặc xem quảng cáo.
4.  Kết thúc game -> Popup tổng kết phần thưởng Event.

---

## 4. Cơ Sở Thiết Kế & Tài Liệu Tham Khảo (Rationale & References)

###  Tại sao thiết kế như vậy? (Design Rationale)

1.  **Cơ chế Pre-loading Assets:**
    * *Lý do:* Đảm bảo công bằng trong game Real-time. Mạng chậm không nên ảnh hưởng đến thời gian trả lời. Đây là tiêu chuẩn ngành (Industry Standard) của các game như *QuizUp*, *Confetti*.
    
2.  **Tách biệt Rank Point (RP) & Performance Point:**
    * *Lý do:* Điểm trận đấu (6-10đ) phản ánh **Hiệu suất (Performance)** tức thời. RP phản ánh **Thành tích (Achievement)** dài hạn. Hệ thống ELO giúp bảng xếp hạng dựa trên kỹ năng (Skill-based) thay vì chỉ "cày cuốc" (Grinding).

3.  **Điểm nhanh nhẹn (+20 Bonus):**
    * *Lý do:* Cơ chế **High Risk - High Return**. Khuyến khích người chơi mạo hiểm để lật ngược tình thế, tạo sự kịch tính (Dramatic effect).

###  Tham khảo từ các nguồn sau 

**Matchmaking Logic:**
* **Keyword:** `Matchmaking System Architecture`, `ELO Rating System`.
* **Source:** [Google Cloud - Building a Matchmaking System for Multiplayer Games](https://cloud.google.com/architecture/building-matchmaking-system-unity-agones-open-match)

**Real-time Communication (Socket):**
* **Keyword:** `Socket.io Real-time Quiz`, `Node.js Websocket Architecture`.
* **Source:** [Socket.io - Get Started Chat App](https://socket.io/get-started/chat) (Logic cơ bản về room và emit events).

**UI/UX Inspiration:**
* **Keyword:** `Quiz App UI Kit`, `Gamification Leaderboard UI`.
* **Source:** [Dribbble - Quiz App Search](https://dribbble.com/search/quiz-app)

