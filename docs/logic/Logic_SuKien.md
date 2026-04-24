# LOGIC NGHIỆP VỤ: SỰ KIỆN (EVENTS)

Dựa trên mã nguồn hiện tại, hệ thống sự kiện được chia thành 3 loại chính với các quy tắc nghiệp vụ riêng biệt.

## 1. PHÂN LOẠI SỰ KIỆN (EVENT TYPES)

### 1.1. Vòng Quay May Mắn (LuckySpin)
- **Cơ chế:** Người dùng thực hiện quay vòng quay để nhận thưởng ngẫu nhiên.
- **Giới hạn:** Có thuộc tính `maxSpinTimePerDay` giới hạn số lượt quay tối đa mỗi ngày.
- **Xử lý kết quả:** Backend quyết định kết quả trúng thưởng (`getWinSpinItem`) để đảm bảo tính minh bạch và bảo mật.
- **Âm thanh:** Có nhạc nền riêng, âm thanh khi đang quay và âm thanh khi nhận thưởng.

### 1.2. Thử Thách Cột Mốc (QuizMilestoneChallenge)
- **Cơ chế:** Chia thành nhiều cột mốc (Thresholds). Mỗi cột mốc yêu cầu người dùng vượt qua một bộ câu hỏi cụ thể (`challengeQuestionIds`).
- **Tiến độ:** Lưu trữ trạng thái hoàn thành từng cột mốc (`ThresholdProgressModel`). Khi người dùng hoàn thành bộ câu hỏi trong trận đấu thử thách, hệ thống tự động gọi API `updateQuizMilestoneProgress` để đánh dấu hoàn thành ải.
- **Nhận thưởng:** Sau khi hoàn thành ải, người dùng phải nhấn "Nhận thưởng" thủ công tại màn hình sự kiện để nhận vật phẩm.
- **Đặc điểm:** Sử dụng chế độ chơi Solo đặc biệt (`isChallengeMatch`).

[//]: # (### 1.3. Nhiệm Vụ Giải Đấu &#40;TournamentRewards&#41;)

[//]: # (- **Cơ chế:** Gồm danh sách các nhiệm vụ &#40;`tasks`&#41; cần thực hiện trong game.)

[//]: # (- **Loại nhiệm vụ:** Có thể là số lượng trận đấu cần tham gia &#40;`numberOfMatchs`&#41; hoặc các hành động cụ thể.)

[//]: # (- **Trạng thái:** Theo dõi tiến độ thực hiện nhiệm vụ &#40;`TaskProgressModel`&#41;.)

## 2. QUY TẮC CHUNG (COMMON RULES)

- **Thời gian:** Mọi sự kiện đều có `startTime` và `endTime`. Ngoài ra còn có `timeType` để xác định chu kỳ (ví dụ: hàng ngày).
- **Trạng thái Khóa:** Thuộc tính `isLocked` xác định sự kiện có đang mở cho người dùng hay không.
- **Hệ thống Phần thưởng:**
    - Sử dụng ID phần thưởng (`eventRewardId`) để ánh xạ sang tên và biểu tượng (Coin, Gem, Items...).
    - Phần thưởng được quản lý tập trung qua `RewardMapping`.
- **Đồng bộ dữ liệu:** Tiến độ sự kiện được lưu trữ và đồng bộ với Backend qua `EventService`.

## 3. CÁC ĐIỀU KIỆN RÀNG BUỘC (CONSTRAINTS)
- **Lượt quay:** Reset theo ngày đối với `LuckySpin`.
- **Thứ tự ải:** Đối với `QuizMilestoneChallenge`, người dùng thường phải hoàn thành theo thứ tự từ thấp đến cao (thể hiện qua UI progress bar).
- **Yêu cầu API:** Mọi hành động nhận thưởng đều phải thông qua API `claimReward` để kiểm tra tính hợp lệ ở phía Server.
