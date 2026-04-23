# GIAO DIỆN THI ĐẤU SOLO (THEO LUỒNG HIỆN TẠI)

## 1. Mục đích
Thi đấu một mình bằng chính `MatchActivity` (dùng chung với multiplayer).

## 2. Cách vào màn
- Từ `MatchConfigActivity`, khi chọn `Single` sẽ gọi `startSoloMatch`.
- Thành công sẽ mở thẳng `MatchActivity`.
- Mở từ thử thách trong sự kiện `QuizMilestoneChallenge` 

## 3. Hành vi trong màn
- Hiển thị câu hỏi tuần tự, đếm thời gian theo từng câu.
- Gửi đáp án bằng API `submitMatchAnswer`.
- Hiển thị feedback đúng/sai/bỏ lỡ tương tự multiplayer.
- Không bật lắng nghe realtime loa trong chế độ solo.

## 4. Kết thúc
- Làm xong câu cuối: chuyển thẳng `MatchResultActivity` (không qua `WaitResultActivity`).
- Solo không cộng/trừ điểm xếp hạng; trọng tâm là điểm bài làm và EXP nhận được.

