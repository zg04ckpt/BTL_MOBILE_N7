# GIAO DIỆN SỰ KIỆN (EVENTS)

Hệ thống giao diện sự kiện được thiết kế nhất quán với các hiệu ứng âm thanh và hình ảnh sinh động.

## 1. CÁC MÀN HÌNH CHÍNH

### 1.1. Danh Sách Sự Kiện (EventActivity)
- **Hiển thị:** Danh sách (RecyclerView) các sự kiện đang diễn ra.
- **Thành phần:** Mỗi item hiển thị Banner, Tên và Trạng thái của sự kiện.
- **Tương tác:** Nhấn vào sự kiện sẽ điều hướng đến màn hình chi tiết tương ứng dựa trên `type` (LuckySpin, QuizMilestoneChallenge, TournamentRewards).
- **Âm thanh:** Nhạc nền (`event_home.raw`) tự động phát khi vào màn hình.

### 1.2. Màn Hình Vòng Quay May Mắn (LuckySpinEventActivity)
- **Thành phần:**
    - Vòng quay (LuckyWheelView) chia thành các nan quạt chứa icon phần thưởng.
    - Text hiển thị "Số lượt quay còn lại trong ngày".
    - Nút "Quay" (Spin).
- **Hiệu ứng:** 
    - Vòng quay quay trong 5 giây với gia tốc giảm dần (DecelerateInterpolator).
    - Âm thanh: Nhạc nền sôi động, tiếng quay và nhạc chúc mừng khi trúng thưởng.
- **Popup:** Hiển thị phần thưởng nhận được với nút xác nhận.

### 1.3. Màn Hình Thử Thách Cột Mốc (QuizMilestoneChallengeActivity)
- **Thành phần:**
    - Thanh tiến trình dọc (Progress Bar) thể hiện các ải.
    - Danh sách các cột mốc: Ải đã xong (có dấu check), Ải hiện tại, Ải chưa đạt.
- **Tương tác:**
    - Nhấn vào ải chưa hoàn thành -> Bắt đầu trận đấu thử thách.
    - Nhấn vào ải đã hoàn thành nhưng chưa nhận quà -> Hiện Popup nhận quà.

[//]: # (### 1.4. Màn Hình Nhiệm Vụ &#40;TournamentRewardsEventActivity&#41;)

[//]: # (- **Thành phần:** Danh sách các thẻ nhiệm vụ.)

[//]: # (- **Nội dung thẻ:** Mô tả ngắn gọn nhiệm vụ, phần thưởng và tiến độ &#40;ví dụ: 2/5 trận&#41;.)

[//]: # (- **Tương tác:** Nút "Nhận thưởng" sẽ sáng lên khi tiến độ đạt 100%.)

## 2. CÁC THÀNH PHẦN DÙNG CHUNG

### 2.1. Popup Phần Thưởng (DialogReward)
- Giao diện trong suốt (`TransparentDialog`).
- Hiển thị: Icon phần thưởng, Tên phần thưởng và Giá trị (ví dụ: +100).
- Hiệu ứng: Phát nhạc `reward_event.raw` khi hiển thị.

### 2.2. Hiệu Ứng Âm Thanh (Sound Effects)
- `event_home`: Nhạc nền chung.
- `event_cirle_round`: Nhạc nền vòng quay.
- `spinning_event`: Tiếng quay.
- `reward_event`: Nhạc khi nhận quà.

## 3. LUỒNG THAO TÁC (USER FLOW)

1.  **Vào Sự Kiện:** Home -> Click Banner Sự kiện -> Mở `EventActivity`.
2.  **Tham Gia:** Click 1 sự kiện cụ thể -> Mở màn hình chi tiết.
3.  **Hoàn Thành:** Thực hiện quay (Spin), thi đấu (Challenge) hoặc làm nhiệm vụ (Task).
4.  **Nhận Thưởng:** Hệ thống kiểm tra kết quả -> Hiển thị Popup Reward -> Cập nhật tài khoản người dùng.
