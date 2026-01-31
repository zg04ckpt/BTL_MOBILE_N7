## Mô tả giao diện ghép trận
Giao diện này hiển thị cho bước chờ ghép trận cho đủ người trước khi thi đấu

### Luồng xử lý chính
- Chờ -> Chuyển sang giao diện thi đấu
- Chờ -> Ấn nút hủy -> Hiện comfirm  -> Quay lại giao diện cấu hình

### Các thành phần UI
- Tiêu đề: Đang ghép trận

- Bộ đếm thời gian chờ theo giây, tính từ 1s -> ns:
    - Text: số giây đã trôi qua kể từ khi chờ ghép trận
    - Logo/Icon/Animation động (Tùy chỉnh)

- Trang thái ghép trận, bao gồm các nội dung:
    - Số lượng người đã ghép được vào phòng / Số lượng tối đa
    - Danh sách người đã được ghép (STT, Avatar, Tên)

- Nút bấm hủy trạng thái ghép trận: 
    - Text: Hủy ghép trận

- Dialog Confirm hủy:
    - Text: Xác nhận hủy ghép trận
    - Buttons: Đóng, Xác nhận
    - Kiểu: BottomSheetDialog
