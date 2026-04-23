# Battle API Docs and Client Contract

Tai lieu nay dung cho ca web admin va mobile client khi tich hop chuc nang thi dau thoi gian thuc.

## 1) Authentication

- Yeu cau auth: moi endpoint co `[Authorize]` bat buoc gui JWT.
- Header:
  - `Authorization: Bearer <token>`
- Neu thieu/het han token: tra `401 Unauthorized`.

## 2) REST Endpoints

### 2.1 Lay cau hinh tran dau

- `GET /api/battles/start-options`
- Auth: optional
- Muc dich: lay danh sach `typesOfBattle`, `contentTypes`, `numberOfPlayers`, `topicsOfContent`.

### 2.2 Join lobby

- `POST /api/battles/lobbies/join`
- Auth: required
- Request:

```json
{
  "battleType": "Single",
  "numberOfPlayers": 2,
  "contentType": "OnlyOne",
  "topicId": 1
}
```

- Rules:
  - `numberOfPlayers` chi chap nhan: `1 | 2 | 5 | 10`.
  - Neu `contentType = OnlyOne` thi `topicId` bat buoc ton tai.
  - `battleType` duoc dung trong key ghep phong (khong bi bo qua).
- Response:

```json
{
  "isSuccess": true,
  "message": "Success",
  "data": {
    "lobbyRoomId": "string-guid"
  }
}
```

### 2.3 Roi lobby

- `POST /api/battles/lobbies/out`
- Auth: required
- Request:

```json
{
  "lobbyRoomId": "string-guid"
}
```

### 2.4 Lay thong tin tran dang dien ra

- `GET /api/battles/match-info`
- Auth: required
- Muc dich: lay `trackingId`, `maxSecondPerQuestions`, va bo cau hoi.

### 2.5 Lay ket qua tran

- `GET /api/battles/match-result`
- Auth: required
- Semantics: lay tran da ket thuc gan nhat cua user hien tai.

- `GET /api/battles/matches/{matchId}/result`
- Auth: required
- Semantics: lay dung ket qua theo `matchId` (khuyen nghi mobile/web su dung endpoint nay de tranh lay nham tran).

### 2.6 Endpoints admin

- `GET /api/battles/paging`
- `GET /api/battles/{id}`
- `DELETE /api/battles/{id}`

## 3) Realtime Contract (Firestore)

Chi tiet schema event xem them tai:
- `Feature.Matchs/REALTIME_EVENT_CONTRACT.md`

### 3.1 Collections

- `lobby-rooms/{lobbyRoomId}`
- `match-rooms/{trackingId}`

### 3.2 Match flow

1. Client join lobby bang REST.
2. Client listen lobby document de theo doi trang thai ghep.
3. Khi lobby du nguoi, backend dong lobby va tao `match-rooms/{trackingId}`.
4. Client goi `GET /api/battles/match-info` de lay `trackingId` + cau hoi.
5. Client submit answers vao field `Answers` cua match-room.
6. Client listen `Players`, `StatusLogs`, `Events` de cap nhat BXH/trang thai.
7. Khi co event `match_ended`, client goi REST lay ket qua.

## 4) Scoring / Ranking Rules (hien tai)

- Moi cau dung: +`ScorePerCorrectAnswer`.
- Duplicate answer theo cap `(userId, questionId)` bi bo qua.
- User hoan thanh se bi khoa cham diem bo sung.
- Tie-break:
  1. `Score` giam dan
  2. `FinishedAtUtc` tang dan (xong som hon xep tren)
  3. `UserId` tang dan (deterministic)

## 5) Timeout and Finalization

- Tran duoc finalize khi:
  - tat ca nguoi choi da xong, hoac
  - het timeout tran (`questionTimeLimit * questionsPerMatch`).
- User chua xong khi timeout se duoc danh dau `EarlyOut`.

## 6) Integration Notes

### Mobile client

- Luon gui JWT cho cac endpoint lobby/match.
- Sau khi nhan `match_ended`, goi:
  - `GET /api/battles/matches/{matchId}/result` neu da co `matchId`
  - fallback `GET /api/battles/match-result`.

### Web admin

- Su dung `paging` + `detail` de hien thi danh sach tran that (khong dung mock data).
- Neu can trang ket qua chi tiet, goi endpoint theo `matchId`.
