# GIAO DIỆN THI ĐẤU GHÉP TRẬN (MATCHACTIVITY)

## 1. Mục đích
Hiển thị câu hỏi và xử lý làm bài trong trận multiplayer.

## 2. Thành phần chính trên màn hình
- Thông tin câu hiện tại (`Câu i/n`).
- Bộ đếm thời gian theo câu.
- Nội dung câu hỏi, ảnh câu hỏi (nếu có).
- Danh sách đáp án (ẩn/hiện theo số lượng đáp án thật).
- Điểm hiện tại của người chơi.
- Feedback sau khi trả lời: đúng/sai/bỏ lỡ.
- Thanh công cụ có tính năng loa.
- Banner nổi thông báo loa từ đối thủ (`tvLoudspeakerNotice`), không làm đổi bố cục chính.

## 3. Luồng trả lời
1. Hiển thị câu hỏi theo thứ tự.
2. Người chơi chọn đáp án.
3. App gửi `submitMatchAnswer` qua API.
4. Nếu đúng: cộng điểm cục bộ, hiển thị feedback đúng.
5. Nếu sai: hiển thị feedback sai.
6. Hết giờ: hiển thị "Bạn đã bỏ lỡ 1 câu", gửi đáp án rỗng rồi tự sang câu sau.

## 4. Loa (loudspeaker)
- App gọi API lấy số lượng loa còn lại.
- Người chơi chọn câu có sẵn trong dialog rồi gửi.
- App nghe Firestore `match-rooms/{trackingId}.Events` để hiện thông báo loa từ người khác.
- Không hiển thị lại thông báo do chính người gửi phát.

## 5. Âm thanh theo trạng thái
- Đúng: phát âm thanh nhóm `correct*`.
- Sai hoặc hết giờ: phát `incorrect`.

## 6. Điều hướng sau khi làm xong
- Multiplayer: chuyển `WaitResultActivity`.
- Solo: chuyển thẳng `MatchResultActivity`.
- Challenge quiz milestone: cập nhật tiến độ sự kiện rồi quay về màn challenge.

## 7. AFK / presence trong trận multiplayer
- Trong lúc làm bài, app heartbeat lên Firestore mỗi 5 giây để cập nhật `IsOnline` và `LastSeenAtUtc`.
- Khi bấm thoát hoặc bấm phím back, app gửi đáp án rỗng cho các câu còn lại rồi rời màn.

