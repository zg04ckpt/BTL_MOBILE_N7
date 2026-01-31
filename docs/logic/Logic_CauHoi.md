# LOGIC NGHIỆP VỤ: CÂU HỎI

## 1. CẤU TRÚC CÂU HỎI
- **Loại:**
  - Single Choice (Chọn 1 đúng).
  - Multiple Choice (Chọn nhiều đúng).
  - True/False.
- **Độ khó:** Dễ, Trung bình, Khó.
- **Media:** Text, Image, Audio.

## 2. QUY TRÌNH DUYỆT (WORKFLOW)
1. **Draft:** Editor tạo, chưa ai thấy ngoài Editor.
2. **Pending:** Editor gửi duyệt. Admin/Mod thấy.
3. **Approved:** Đã duyệt. Có thể xuất hiện trong game.
4. **Rejected:** Từ chối. Kèm lý do. Editor phải sửa và gửi lại.
5. **Needs Revision:** Yêu cầu sửa nhỏ.

## 3. PHÂN PHỐI CÂU HỎI (RANDOMIZATION)
- Thuật toán chọn câu hỏi cho trận đấu phải đảm bảo:
  - **Đúng chủ đề.**
  - **Đúng tỷ lệ độ khó:** Ví dụ 3 Dễ - 4 TB - 3 Khó.
  - **Không lặp lại:** Không gặp lại câu đã trả lời trong X trận gần nhất (nếu có thể).
  - **Xáo trộn:** Đáp án A, B, C, D phải được xáo trộn ngẫu nhiên mỗi lần hiển thị.

## 4. QUẢN LÝ PHIÊN BẢN
- Khi sửa một câu hỏi đã Approved -> Tạo version mới hoặc đưa về Pending.
- Lịch sử trận đấu cũ tham chiếu đến snapshot nội dung câu hỏi tại thời điểm thi đấu (để tránh việc sửa câu hỏi làm sai lệch lịch sử).
