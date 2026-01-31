# LOGIC NGHIỆP VỤ: THI ĐẤU

## 1. CÁC CHẾ ĐỘ THI ĐẤU
### 1.1. Vượt Ải Đơn (Solo)
- **Mục tiêu:** Trả lời đúng liên tục nhiều câu nhất có thể.
- **Luật dừng:** Trả lời sai 1 câu -> Kết thúc ngay.
- **Điểm:** Tính theo số câu đúng. Không có đối thủ.
- **Thời gian:** Không giới hạn tổng thời gian, nhưng có thể giới hạn thời gian từng câu (ví dụ 30s/câu).

### 1.2. Ghép Trận (Multiplayer)
- **Số lượng:** 2, 5, 10 người.
- **Cấu trúc:** 1 bộ đề cố định (ví dụ 10 câu).
- **Đồng bộ:** Tất cả bắt đầu cùng lúc.
- **Luật:**
  - Làm hết 10 câu hoặc hết giờ -> Kết thúc.
  - Được phép sai, được phép quay lại sửa (trong thời gian cho phép).

## 2. QUY TẮC GHÉP TRẬN (MATCHMAKING)
- **Tiêu chí:**
  1. **ELO/Rank Point:** Tìm người chơi trong khoảng `+/- 100` điểm.
  2. **Chủ đề:** Phải chọn cùng chủ đề (hoặc mode Ngẫu nhiên).
  3. **Ping/Khu vực:** Ưu tiên kết nối tốt (nếu có server phân tán).
- **Cơ chế mở rộng (Expansion):**
  - `< 10s`: Tìm chính xác +/- 100 ELO.
  - `10s - 30s`: Mở rộng khoảng ELO +/- 300.
  - `> 30s`: Gợi ý đấu với Bot hoặc mở rộng tối đa.
- **Bot:** Bot có ELO giả lập tương đương người chơi để cân bằng.

## 3. CƠ CHẾ TÍNH ĐIỂM IN-GAME
- **Điểm cơ bản:** Mỗi câu đúng +10 điểm.
- **Điểm độ khó:**
  - Dễ: x1.0
  - Trung bình: x1.2
  - Khó: x1.5
- **Điểm thời gian (Chỉ multiplayer):** Trả lời nhanh trong 3s đầu: +Bonus.
- **Điểm Combo:** Trả lời đúng liên tiếp 3 câu: +Bonus.
- **Điểm Nhanh nhẹn (Hoàn thành bài):** +20 điểm nếu xong sớm nhất và đúng >80%.

## 4. XỬ LÝ NGẮT KẾT NỐI
- **Mất mạng:**
  - Server đợi trong khoảng thời gian ngắn (Reconnect Window - VD: 10s).
  - Nếu quay lại kịp: Tiếp tục trạng thái cũ.
  - Nếu không: Tính là bỏ cuộc (Thua), bị trừ điểm.
