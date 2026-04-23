# Realtime Match Event Contract

Tai lieu nay mo ta contract toi thieu cho realtime battle giua backend va client.

## Collections

- `lobby-rooms/{lobbyRoomId}`
- `match-rooms/{trackingId}`

## Event schema

Moi event duoc append vao truong `Events`:

```json
{
  "Type": "player_finished",
  "Message": "User 101 finished all questions",
  "Timestamp": "2026-04-23T10:15:30.000Z"
}
```

## Supported event types

- `lobby_created`: backend tao lobby.
- `player_joined`: nguoi choi vao lobby.
- `player_left`: nguoi choi roi lobby.
- `match_found`: lobby du nguoi, bat dau dem nguoc.
- `match_created`: backend tao phong thi dau.
- `answer_submitted`: backend nhan batch answer tu client.
- `player_finished`: 1 nguoi choi da hoan thanh tat ca cau hoi.
- `leaderboard_updated`: backend da cap nhat BXH realtime.
- `match_ended`: tran ket thuc, backend chot ket qua.

## Minimal client flow

1. Join lobby qua API `POST /api/battles/lobbies/join`.
2. Listen document `lobby-rooms/{lobbyRoomId}`.
3. Khi thay `match_found`, goi `GET /api/battles/match-info` de lay `trackingId` + bo cau hoi.
4. Trong tran:
   - submit answer vao `match-rooms/{trackingId}.Answers`;
   - listen `Players`, `StatusLogs`, `Events` de cap nhat UI.
5. Khi thay `match_ended`, goi `GET /api/battles/match-result` de lay ket qua chinh thuc.

## Backend rules

- Duplicate answer theo cap `(userId, questionId)` bi bo qua.
- User da `player_finished` se khong duoc cham diem them.
- Tran auto finalize khi:
  - tat ca nguoi choi da xong, hoac
  - qua timeout tran (questionTimeLimit * questionsPerMatch).
