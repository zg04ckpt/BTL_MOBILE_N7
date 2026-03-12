using CNLib.Services.Logs;
using Feature.Matchs.Interfaces;
using Feature.Matchs.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace Feature.Matchs.Services
{
    public class FirebaseService : IMatchRealtimeService
    {
        private readonly FirestoreDb _db;
        private readonly ILogService<FirebaseService> _logService;

        public event EventHandler<RealtimeRoomLobbyInfoDto>? OnCloseRoom;

        public FirebaseService(ILogService<FirebaseService> logService)
        {
            _logService = logService;

            var credential = CredentialFactory
                .FromFile<ServiceAccountCredential>(Path.Combine(
                    AppContext.BaseDirectory,
                    "quizbattle-8014d-firebase-adminsdk-fbsvc-b85a8702bc.json"))
                .ToGoogleCredential();

            _db = new FirestoreDbBuilder
            {
                ProjectId = "quizbattle-8014d",
                Credential = credential
            }.Build();
        }

        public async Task<bool> AddNewLobbyRoomAsync(LobbyRoomDto room)
        {
            try
            {
                DocumentReference docRef = _db.Collection("lobby-rooms").Document(room.Id);
                await docRef.SetAsync(new RealtimeRoomLobbyInfoDto
                {
                    RoomLobbyId = room.Id,
                    MaxPlayers = room.MaxPlayers,
                    Players = new(),
                    StatusLogs = new()
                    {
                        "Bắt đầu khởi tạo phòng ghép"
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddPlayerToRoomAsync(string roomId, PlayerInLobbyInfoDto player)
        {
            try
            {
                var docRef = _db.Collection("lobby-rooms").Document(roomId);

                var snapshot = await docRef.GetSnapshotAsync();
                if (!snapshot.Exists)
                {
                    _logService.LogError($"Failed to add player to room {roomId} because it not exist");
                    return false;
                }

                RealtimeRoomLobbyInfoDto room = snapshot.ConvertTo<RealtimeRoomLobbyInfoDto>();
                if (room.Players.Count == room.MaxPlayers)
                {
                    _logService.LogError($"Failed to add player to room {roomId} because has no slot in lobby room");
                    return false;
                }

                // Update status
                room.Players.Add(player);
                room.StatusLogs.Add($"{player.DisplayName} đã tham gia vào phòng");
                await docRef.UpdateAsync(nameof(room.Players), room.Players);
                await docRef.UpdateAsync(nameof(room.StatusLogs), room.StatusLogs);

                // If reach the limit, close room after 3s
                if (room.Players.Count == room.MaxPlayers)
                {
                    _ = Task.Run(async () =>
                    {
                        room.StatusLogs.Add($"Đã đủ người, trận đấu sẽ bắt đầu sau 10s");
                        await docRef.UpdateAsync(nameof(room.StatusLogs), room.StatusLogs);
                        await Task.Delay(10000);
                        await CloseLobbyRoomToStartMatchAsync(docRef, room);
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> RemovePlayerFromRoomAsync(string roomId, int playerId)
        {
            try
            {
                var docRef = _db.Collection("lobby-rooms").Document(roomId);

                var snapshot = await docRef.GetSnapshotAsync();
                if (!snapshot.Exists)
                {
                    _logService.LogError($"Failed to add player to room {roomId} because it not exist");
                    return false;
                }

                RealtimeRoomLobbyInfoDto room = snapshot.ConvertTo<RealtimeRoomLobbyInfoDto>();
                var player = room.Players.FirstOrDefault(u => u.UserId == playerId);
                if (player == null)
                {
                    _logService.LogError($"Failed to remove player from room {roomId}: Player not found");
                    return false;
                }
                room.Players.Remove(player);
                room.StatusLogs.Add($"{player.DisplayName} đã thoát khỏi phòng");
                await docRef.UpdateAsync(nameof(room.Players), room.Players);
                await docRef.UpdateAsync(nameof(room.StatusLogs), room.StatusLogs);

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError(ex.Message);
                return false;
            }
        }

        private async Task CloseLobbyRoomToStartMatchAsync(DocumentReference docRef, RealtimeRoomLobbyInfoDto room)
        {
            try
            {
                await docRef.DeleteAsync();
                OnCloseRoom?.Invoke(this, room);
            }
            catch (Exception ex)
            {
                _logService.LogError($"Failed to close room {room.RoomLobbyId}: {ex.Message}");
            }
        }
    }
}
