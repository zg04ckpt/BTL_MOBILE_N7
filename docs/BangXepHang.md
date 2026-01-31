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