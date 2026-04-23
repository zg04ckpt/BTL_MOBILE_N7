using CNLib.Services.Logs;
using Feature.Matchs.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Models.Matchs.DTOs;
using Models.Matchs.Entities;
using Models.Matchs.Realtimes;

namespace Feature.Matchs.Services
{
    public class FirebaseService : IMatchRealtimeService
    {
        private readonly FirestoreDb _db;
        private readonly ILogService<FirebaseService> _logService;

        public event EventHandler<RealtimeRoomLobbyInfoDto>? OnCloseRoom;
        public event EventHandler<(string trackingId, List<MatchPlayerAnswerDto> answers)>? OnPlayerPickAnswer;

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
                    StartTriggered = false,
                    Events = new()
                    {
                        new MatchRealtimeEventDto
                        {
                            Type = MatchRealtimeEventTypes.LobbyCreated,
                            Message = "Lobby created"
                        }
                    },
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

                var transactionResult = await _db.RunTransactionAsync(async transaction =>
                {
                    var snapshot = await transaction.GetSnapshotAsync(docRef);
                    if (!snapshot.Exists)
                    {
                        return (ok: false, shouldStart: false, room: (RealtimeRoomLobbyInfoDto?)null);
                    }

                    var room = snapshot.ConvertTo<RealtimeRoomLobbyInfoDto>();
                    room.Events ??= new();
                    room.StatusLogs ??= new();
                    room.Players ??= new();

                    if (room.Players.Any(u => u.UserId == player.UserId))
                    {
                        return (ok: true, shouldStart: false, room: room);
                    }

                    if (room.Players.Count >= room.MaxPlayers)
                    {
                        return (ok: false, shouldStart: false, room: room);
                    }

                    room.Players.Add(player);
                    room.StatusLogs.Add($"{player.DisplayName} đã tham gia vào phòng");
                    room.Events.Add(new MatchRealtimeEventDto
                    {
                        Type = MatchRealtimeEventTypes.PlayerJoined,
                        Message = $"{player.DisplayName} joined lobby"
                    });

                    var shouldStart = false;
                    if (room.Players.Count == room.MaxPlayers && !room.StartTriggered)
                    {
                        shouldStart = true;
                        room.StartTriggered = true;
                        var countdownSeconds = room.MaxPlayers == 1 ? 0 : 5;
                        room.StatusLogs.Add(countdownSeconds == 0
                            ? "Đã đủ người, trận đấu bắt đầu ngay"
                            : $"Đã đủ người, trận đấu sẽ bắt đầu sau {countdownSeconds}s");
                        room.Events.Add(new MatchRealtimeEventDto
                        {
                            Type = MatchRealtimeEventTypes.MatchFound,
                            Message = countdownSeconds == 0
                                ? "Match found, starting immediately"
                                : "Match found, starting countdown"
                        });
                    }

                    transaction.Update(docRef, new Dictionary<string, object>
                    {
                        { nameof(room.Players), room.Players },
                        { nameof(room.StatusLogs), room.StatusLogs },
                        { nameof(room.Events), room.Events },
                        { nameof(room.StartTriggered), room.StartTriggered }
                    });

                    return (ok: true, shouldStart: shouldStart, room: room);
                });

                if (!transactionResult.ok)
                {
                    _logService.LogError($"Failed to add player to room {roomId}");
                    return false;
                }

                if (transactionResult.shouldStart && transactionResult.room != null)
                {
                    _ = Task.Run(async () =>
                    {
                        var countdownSeconds = transactionResult.room.MaxPlayers == 1 ? 0 : 5;
                        if (countdownSeconds > 0)
                        {
                            await Task.Delay(countdownSeconds * 1000);
                        }

                        var latestSnapshot = await docRef.GetSnapshotAsync();
                        if (!latestSnapshot.Exists)
                        {
                            return;
                        }

                        var latestRoom = latestSnapshot.ConvertTo<RealtimeRoomLobbyInfoDto>();
                        if (latestRoom.Players.Count == latestRoom.MaxPlayers && latestRoom.StartTriggered)
                        {
                            await CloseLobbyRoomToStartMatchAsync(docRef, latestRoom);
                        }
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

                return await _db.RunTransactionAsync(async transaction =>
                {
                    var snapshot = await transaction.GetSnapshotAsync(docRef);
                    if (!snapshot.Exists)
                    {
                        _logService.LogError($"Failed to remove player from room {roomId} because it not exist");
                        return false;
                    }

                    RealtimeRoomLobbyInfoDto room = snapshot.ConvertTo<RealtimeRoomLobbyInfoDto>();
                    room.Events ??= new();
                    room.StatusLogs ??= new();
                    room.Players ??= new();

                    var player = room.Players.FirstOrDefault(u => u.UserId == playerId);
                    if (player == null)
                    {
                        _logService.LogError($"Failed to remove player from room {roomId}: Player not found");
                        return false;
                    }

                    room.Players.Remove(player);
                    if (room.Players.Count < room.MaxPlayers)
                    {
                        room.StartTriggered = false;
                    }
                    room.StatusLogs.Add($"{player.DisplayName} đã thoát khỏi phòng");
                    room.Events.Add(new MatchRealtimeEventDto
                    {
                        Type = MatchRealtimeEventTypes.PlayerLeft,
                        Message = $"{player.DisplayName} left lobby"
                    });

                    transaction.Update(docRef, new Dictionary<string, object>
                    {
                        { nameof(room.Players), room.Players },
                        { nameof(room.StatusLogs), room.StatusLogs },
                        { nameof(room.Events), room.Events },
                        { nameof(room.StartTriggered), room.StartTriggered }
                    });

                    return true;
                });
            }
            catch (Exception ex)
            {
                _logService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddNewMatchRoomAsync(string trackingId, List<PlayerInLobbyInfoDto> players, Match match)
        {
            try
            {
                var room = new MatchRoomDto
                {
                    RoomId = match.Id,
                    StatusLogs = new List<string>
                    {
                        "Bắt đầu thi đấu sau 3s"
                    },
                    Events = new List<MatchRealtimeEventDto>
                    {
                        new MatchRealtimeEventDto
                        {
                            Type = MatchRealtimeEventTypes.MatchCreated,
                            Message = "Match room created"
                        }
                    },
                    Answers = new List<MatchPlayerAnswerDto>(),
                    Players = players.Select(e => new MatchPlayerInfoDto
                    {
                        UserId = e.UserId,
                        AvatarUrl = e.AvatarUrl,
                        DisplayName = e.DisplayName,
                        Level = e.Level,
                        Progress = 0,
                        Score = 0
                    }).ToList()
                };

                // Random rank
                for (int i = 0; i < room.Players.Count; i++)
                {
                    room.Players[i].Rank = i + 1;
                }

                var docRef = _db.Collection("match-rooms").Document(trackingId);
                await docRef.CreateAsync(room);

                // Set player answer event listener
                docRef.Listen(async snapshot =>
                {
                    if (!snapshot.Exists)
                    {
                        return;
                    }
                    var data = snapshot.ConvertTo<MatchRoomDto>();
                    data.Events ??= new();
                    if (data.Answers.Count > 0)
                    {
                        data.Events.Add(new MatchRealtimeEventDto
                        {
                            Type = MatchRealtimeEventTypes.AnswerSubmitted,
                            Message = $"Received {data.Answers.Count} answer(s)"
                        });
                        OnPlayerPickAnswer?.Invoke(this, (trackingId, data.Answers));
                        data.Answers.Clear();
                        await docRef.UpdateAsync(new Dictionary<string, object>
                        {
                            { nameof(data.Answers), data.Answers },
                            { nameof(data.Events), data.Events }
                        });
                    }
                });

                // Debug
                _logService.LogSuccess($"New match {trackingId} started with {players.Count} players");

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Failed to start match {trackingId}: {ex.Message}");
                return false;
            }
            
        }

        public async Task<bool> AddPlayerAnswerAsync(string trackingId, MatchPlayerAnswerDto answer)
        {
            try
            {
                var docRef = _db.Collection("match-rooms").Document(trackingId);
                await docRef.UpdateAsync(new Dictionary<string, object>
                {
                    { nameof(MatchRoomDto.Answers), FieldValue.ArrayUnion(answer) }
                });
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Failed to append answer for match {trackingId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateLeaderboardAsync(string trackingId, List<UserMappingDto> updatedUsers)
        {
            try
            {
                var docRef = _db.Collection("match-rooms").Document(trackingId);
                var snapshot = await docRef.GetSnapshotAsync();
                if (!snapshot.Exists)
                {
                    _logService.LogError($"Failed to update leaderboard: match room {trackingId} not found");
                    return false;
                }

                var room = snapshot.ConvertTo<MatchRoomDto>();
                room.Events ??= new();

                // Snapshot old ranks before any update
                var oldRanks = room.Players.ToDictionary(p => p.UserId, p => p.Rank);

                foreach (var updated in updatedUsers)
                {
                    var player = room.Players.FirstOrDefault(p => p.UserId == updated.UserId);
                    if (player != null)
                    {
                        player.Score = updated.Score;
                        player.Progress = updated.Progress;
                    }
                }

                // Recalculate ranks sorted by score descending
                var ranked = room.Players.OrderByDescending(p => p.Score).ToList();
                for (int i = 0; i < ranked.Count; i++)
                {
                    ranked[i].Rank = i + 1;
                }

                // Append status logs for players whose rank changed
                foreach (var player in ranked)
                {
                    if (!oldRanks.TryGetValue(player.UserId, out var oldRank) || oldRank == player.Rank)
                        continue;

                    string log = player.Rank == 1
                        ? $"{player.DisplayName} đã vươn lên dẫn đầu! ({player.Score} điểm)"
                        : $"{player.DisplayName} thay đổi hạng: #{oldRank} → #{player.Rank} ({player.Score} điểm)";

                    room.StatusLogs.Add(log);
                }

                await docRef.UpdateAsync(new Dictionary<string, object>
                {
                    { nameof(room.Players), room.Players },
                    { nameof(room.StatusLogs), room.StatusLogs },
                    {
                        nameof(room.Events),
                        room.Events.Append(new MatchRealtimeEventDto
                        {
                            Type = MatchRealtimeEventTypes.LeaderboardUpdated,
                            Message = $"Leaderboard updated for {updatedUsers.Count} player(s)"
                        }).ToList()
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Failed to update leaderboard for match {trackingId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CloseMatchRoomAsync(string trackingId)
        {
            try
            {
                var docRef = _db.Collection("match-rooms").Document(trackingId);
                await docRef.DeleteAsync();
                _logService.LogSuccess($"Match room {trackingId} closed and removed from Firebase");
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Failed to close match room {trackingId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AppendMatchEventAsync(string trackingId, MatchRealtimeEventDto matchEvent, string? statusLog = null)
        {
            try
            {
                var docRef = _db.Collection("match-rooms").Document(trackingId);
                var snapshot = await docRef.GetSnapshotAsync();
                if (!snapshot.Exists)
                {
                    _logService.LogError($"Failed to append event: match room {trackingId} not found");
                    return false;
                }

                var room = snapshot.ConvertTo<MatchRoomDto>();
                room.Events ??= new();
                room.StatusLogs ??= new();

                room.Events.Add(matchEvent);
                if (!string.IsNullOrWhiteSpace(statusLog))
                {
                    room.StatusLogs.Add(statusLog);
                }

                await docRef.UpdateAsync(new Dictionary<string, object>
                {
                    { nameof(room.Events), room.Events },
                    { nameof(room.StatusLogs), room.StatusLogs }
                });

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError($"Failed to append event for match {trackingId}: {ex.Message}");
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
