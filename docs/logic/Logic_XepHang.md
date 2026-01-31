# LOGIC NGHIỆP VỤ: XẾP HẠNG (LEADERBOARD)

## 1. ĐIỂM XẾP HẠNG (RANK POINT - RP)
- Đây là đơn vị tiền tệ của danh vọng, quyết định thứ hạng.
- **Khởi điểm:** 100 điểm (hoặc 1000 tùy scaling).
- **Quy tắc Cộng/Trừ:**
  - **Thắng (Top 50% trên):** +20 RP + (Bonus Win Streak).
  - **Thua (Top 50% dưới):** -10 RP (Không trừ nếu < Rank Đồng/Bảo vệ rank).
  - **Hòa:** +5 RP.
- **Bảo hộ hạng:**
  - Khi mới lên hạng, được bảo hộ 3 trận không bị rớt hạng dù thua.

## 2. PHÂN HẠNG (TIERS)
- **Đồng:** 0 - 500 RP.
- **Bạc:** 501 - 1500 RP.
- **Vàng:** 1501 - 3000 RP.
- **Bạch Kim:** 3001 - 5000 RP.
- **Kim Cương:** > 5000 RP.

## 3. CƠ CHẾ RESET (MÙA GIẢI)
- **Chu kỳ:** 1 tháng/lần hoặc theo Quý.
- **Soft Reset:** Không về 0 mà về mốc thấp hơn của hạng hiện tại.
  - Ví dụ: Đang Vàng -> Reset về đầu Bạc.
- **Phần thưởng mùa giải:** Trao khung, huy hiệu dựa trên Rank cao nhất đạt được trong mùa.

## 4. HIỂN THỊ BXH
- **Scope:** Global (Toàn Server).
- **Timeframe:** Tuần (Reset 00:00 T2), Tháng, All-time.
- **Cache:** BXH không realtime 100% để giảm tải DB. Cập nhật 5-10 phút/lần hoặc Realtime với Redis Sorted Set.
