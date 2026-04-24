# GIAO DIỆN CHỜ KẾT QUẢ (WAITRESULTACTIVITY)

## 1. Mục đích
Hiển thị tiến độ hoàn thành của người chơi trong trận multiplayer sau khi người dùng làm xong bài, trước khi vào màn kết quả cuối.

## 2. Thành phần giao diện
- Tiêu đề trạng thái chờ (`tv_waiting_status`).
- Vùng tóm tắt của bản thân người chơi:
  - Điểm hiện tại (`tv_score`).
  - Số câu đúng/sai (`tv_correct`).
- Danh sách tiến độ người chơi (`lv_match_progress`):
  - Tên người chơi.
  - Tiến độ theo số câu đã làm / tổng số câu.
  - Trạng thái hoàn thành.
- Nút về (`btn_home`) để thoát màn chờ.

## 3. Nguồn dữ liệu và cách cập nhật
- Màn hình polling API `getMatchState` theo chu kỳ:
  - Mặc định: mỗi 2 giây.
  - Khi tất cả người chơi đã hoàn thành: mỗi 1 giây.
- Dữ liệu trả về dùng để:
  - Cập nhật danh sách tiến độ.
  - Đồng bộ `totalQuestions`.
  - Cập nhật text trạng thái.

## 4. Trạng thái hiển thị
- Khi chưa đủ người hoàn thành:
  - Hiển thị: `Đang cập nhật realtime: X người chơi`.
- Khi tất cả đã hoàn thành:
  - Hiển thị: `Tất cả đã hoàn thành, đang tổng kết kết quả...`.
- Khi gọi API lỗi:
  - Hiển thị: `Chưa thể tải trạng thái trận: <message>`.

## 5. Điều hướng
- Khi `state.ended == true`:
  - Chờ ngắn 700ms để người dùng thấy trạng thái cuối.
  - Tự chuyển sang `MatchResultActivity` kèm `matchId`.
- Khi bấm `Về trang chủ`:
  - Đóng `WaitResultActivity`.

## 6. Phạm vi áp dụng
- Áp dụng cho luồng multiplayer.
- Solo không đi qua màn này (đi thẳng `MatchResultActivity`).
