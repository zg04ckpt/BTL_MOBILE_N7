# GIAO DIỆN KẾT QUẢ (MATCHRESULTACTIVITY)

## 1. Mục đích
Hiển thị bảng xếp hạng cuối trận cho tất cả người chơi.

## 2. Thành phần chính
- Tiêu đề kết quả.
- Danh sách người chơi bằng `RecyclerView`:
  - Hạng.
  - Avatar (load bằng Glide, cắt tròn).
  - Tên.
  - Điểm.
  - EXP nhận được.
  - Rank score nhận được.
  - Trạng thái bảo vệ rank: hiển thị `-0 rank` kèm icon thẻ bảo vệ khi được áp dụng.
- Nút "Về trang chủ".

## 3. Luồng dữ liệu
1. Nhận `matchId` từ intent.
2. Gọi API `getMatchReview(matchId)`.
3. Sắp xếp user theo điểm giảm dần, sau đó theo `userId`.
4. Bind dữ liệu vào adapter `MatchResultRankingAdapter`.

## 4. Âm thanh
- Khi vào màn kết quả, phát file `showfinalresult` nếu có trong `res/raw`.

## 5. Điều hướng
- Bấm "Về trang chủ": mở `HomeActivity` với cờ `CLEAR_TOP | SINGLE_TOP`.

