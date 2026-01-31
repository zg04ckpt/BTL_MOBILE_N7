# 4.1. CẤU HÌNH XẾP HẠNG

## 1. NGHIỆP VỤ

### Mục đích
Admin cấu hình công thức tính điểm xếp hạng và các tham số liên quan.

### Luồng xử lý
1. Admin truy cập "Cấu hình xếp hạng".
2. Hiển thị form: Công thức điểm (trọng số điểm chính xác + điểm nhanh nhẹn, tổng = 1.0); điều kiện nhận điểm nhanh nhẹn; reset xếp hạng (tự động / ngày reset).
3. Admin chỉnh sửa và bấm "Lưu".
4. Hệ thống validate (tổng trọng số = 1.0, trọng số ∈ [0, 1]); lưu cấu hình; nếu cần thì tính lại điểm cho tất cả người dùng và cập nhật bảng xếp hạng.
5. Hiển thị thông báo thành công.

### Quy tắc nghiệp vụ
- Thay đổi công thức ảnh hưởng đến tất cả người dùng.
- Nên thông báo trước khi thay đổi lớn.
- Có thể preview kết quả trước khi áp dụng.

### Validation
- Tổng trọng số = 1.0; mỗi trọng số ≥ 0 và ≤ 1.

---

## 2. MÔ TẢ GIAO DIỆN

### 2.1. Trang tổng thể

- **Loại trang:** Form cấu hình (settings). Header: tiêu đề "Cấu hình xếp hạng" + nút "Lưu" (primary). Nội dung: các section (Công thức tính điểm, Điều kiện điểm nhanh nhẹn, Reset xếp hạng); tùy chọn section Preview. Nền trang nhạt; form trong vùng trắng, bo góc, shadow nhẹ.

---

### 2.2. Section "Công thức tính điểm"

- Helper text: Điểm xếp hạng = (Điểm chính xác × Trọng số 1) + (Điểm nhanh nhẹn × Trọng số 2). Tổng trọng số phải bằng 1.0.
- Hai trường: Trọng số điểm chính xác, Trọng số điểm nhanh nhẹn (số từ 0 đến 1; có thể dùng slider hoặc input). Có thể chỉ nhập một trường, trường kia tự sync (tổng = 1). Hiển thị lỗi khi tổng khác 1.0.

---

### 2.3. Section "Điều kiện nhận điểm nhanh nhẹn"

- Switch/Radio: Yêu cầu hoàn thành tất cả câu hỏi (Có/Không).
- Input: Số câu đúng tối thiểu để nhận điểm nhanh nhẹn (ví dụ 8/10). Có helper text gợi ý.

---

### 2.4. Section "Reset xếp hạng"

- Dropdown/Radio: Tự động reset — Không / Hàng tuần / Hàng tháng / Hàng năm. Khi chọn chu kỳ: hiện thêm chọn Ngày reset (thứ trong tuần, ngày trong tháng, hoặc ngày-tháng tùy chu kỳ). Có thể có cảnh báo khi bật: "Thay đổi áp dụng từ chu kỳ tiếp theo."

---

### 2.5. Tính lại điểm và Nút Lưu

- Sau khi đổi công thức: có thể hỏi "Tính lại điểm cho tất cả người dùng ngay?" với nút "Tính lại ngay" / "Để sau". Nếu chọn Tính lại: hiển thị tiến độ và thông báo xong.
- Nút "Lưu": ở header hoặc sticky footer (mobile). Trạng thái: bình thường → Đang lưu (spinner) → Toast thành công hoặc lỗi. Disable Lưu khi tổng trọng số khác 1.0.

---

### 2.6. Loading và lỗi

- Load form: skeleton hoặc spinner. Lỗi lưu: toast lỗi; giữ form để sửa.
