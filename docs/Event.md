## 3. Chức Năng và Giao diện :  Sự Kiện (Events)

###  Mục tiêu (Goal)
Tạo nội dung tươi mới, giới hạn thời gian để tăng tương tác người dùng (Engagement).


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