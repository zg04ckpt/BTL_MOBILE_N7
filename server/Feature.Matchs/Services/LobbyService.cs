using CNLib.Services.Logs;
using Core.Exceptions;
using Core.Interfaces;
using Feature.Matchs.Interfaces;
using Feature.Users.Entities;
using Microsoft.Extensions.DependencyInjection;
using Models.Matchs.Enums;
using Models.Matchs.Realtimes;
using Models.Matchs.Requests;
using Models.Quizzes.Entities;
using System.Collections.Concurrent;
using System.Threading;

namespace Feature.Matchs.Services
{
    public class LobbyService : ILobbyService
    {
        private static int _isCloseRoomSubscribed;
        private readonly IMatchRealtimeService _matchRealtimeService;
        private static readonly ConcurrentDictionary<string, LobbyRoomDto> _rooms = new();
        private readonly IUnitOfWork _uow;
        private readonly ILogService<LobbyService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public LobbyService(
            IMatchRealtimeService matchRealtimeService,
            IUnitOfWork uow,
            ILogService<LobbyService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _matchRealtimeService = matchRealtimeService;
            _uow = uow;
            _logger = logger;
            _scopeFactory = scopeFactory;

            if (Interlocked.Exchange(ref _isCloseRoomSubscribed, 1) == 0)
            {
                _matchRealtimeService.OnCloseRoom += HandleOnCloseRoom;
            }
        }

        public async Task<MatchConfigOptions> GetOptionsAsync()
        {
            var topics = await _uow.Repository<Topic>().GetAllAsync(
                predicate: e => true,
                selector: e => new { e.Id, e.Name });

            return new MatchConfigOptions
            {
                NumberOfPlayers = new int[] { 1, 2, 5, 10 },
                TopicsOfContent = topics.ToArray(),
                ContentTypes = new[]
                {
                    new { Type = nameof(MatchContentType.Random), Label = "Ngẫu nhiên" },
                    new { Type = nameof(MatchContentType.Mix), Label = "Hỗn hợp" },
                    new { Type = nameof(MatchContentType.OnlyOne), Label = "Theo chủ đề" },
                },
                TypesOfBattle = new[]
                {
                    new { Type = nameof(BattleType.Single), Label = "Đấu đơn vượt ải" },
                    new { Type = nameof(BattleType.Team), Label = "Đấu đội" },
                }
            };
        }

        public async Task<string> OutLobbyRoomAsync(int userId, OutLobbyRequest request)
        {
            if (!_rooms.ContainsKey(request.LobbyRoomId))
            {
                throw new BadRequestException($"Room {request.LobbyRoomId} not found");
            }

            try
            {
                var removeResult = await _matchRealtimeService.RemovePlayerFromRoomAsync(request.LobbyRoomId, userId);
                if (!removeResult)
                {
                    throw new ServerErrorException($"Failed to remove player {userId} from lobby room {request.LobbyRoomId}");
                }
            }
            catch (ServerErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"OutLobbyRoomAsync: Failed to remove player {userId} from realtime room {request.LobbyRoomId} - {ex.Message}");
                throw new ServerErrorException("Failed to leave lobby room");
            }

            return request.LobbyRoomId;
        }

        public async Task<string> JoinMatchLobbyAsync(int userId, JoinLobbyRequest request)
        {
            await RequireValidJoinOptionsRequestAsync(request);

            var suitableRoom = _rooms.Values.FirstOrDefault(room =>
                room.BattleType == request.BattleType &&
                room.ContentType == request.ContentType &&
                room.TopicId == request.TopicId &&
                room.MaxPlayers == request.NumberOfPlayers);

            // Create new room if no suitable room
            if (suitableRoom == null)
            {
                suitableRoom = new LobbyRoomDto
                {
                    Id = Guid.NewGuid().ToString(),
                    MaxPlayers = request.NumberOfPlayers,
                    Status = LobbyRoomStatus.InQueue,
                    BattleType = request.BattleType,
                    ContentType = request.ContentType,
                    TopicId = request.TopicId,
                };
                try
                {
                    var addResult = await _matchRealtimeService.AddNewLobbyRoomAsync(suitableRoom);
                    if (!addResult)
                    {
                        throw new ServerErrorException($"Failed to add new lobby room {suitableRoom.Id}");
                    }
                }
                catch (ServerErrorException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"JoinMatchLobbyAsync: Failed to create realtime lobby room {suitableRoom.Id} - {ex.Message}");
                    throw new ServerErrorException("Failed to create lobby room");
                }

                _rooms.TryAdd(suitableRoom.Id, suitableRoom);
            }

            // Add player to lobby room
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: u => u.Id == userId)
                ?? throw new NotFoundException("User not found");

            try
            {
                var addPlayerResult = await _matchRealtimeService.AddPlayerToRoomAsync(suitableRoom.Id, new PlayerInLobbyInfoDto
                {
                    UserId = user.Id,
                    AvatarUrl = user.AvatarUrl,
                    DisplayName = user.Name,
                    Level = user.Level,
                });
                if (!addPlayerResult)
                {
                    throw new ServerErrorException($"Failed to add new player {user.Id} to lobby room {suitableRoom.Id}");
                }
            }
            catch (ServerErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"JoinMatchLobbyAsync: Failed to add player {user.Id} to realtime room {suitableRoom.Id} - {ex.Message}");
                throw new ServerErrorException("Failed to join lobby room");
            }

            return suitableRoom.Id;
        }

        private void HandleOnCloseRoom(object? sender, RealtimeRoomLobbyInfoDto e)
        {
            _ = HandleOnCloseRoomAsync(e);
        }

        private async Task HandleOnCloseRoomAsync(RealtimeRoomLobbyInfoDto e)
        {
            try
            {
                if (_rooms.TryGetValue(e.RoomLobbyId, out var room))
                {
                    using var scope = _scopeFactory.CreateScope();
                    var matchService = scope.ServiceProvider.GetRequiredService<IMatchService>();
                    await matchService.StartMatchAsync(room, e.Players);
                    _rooms.TryRemove(e.RoomLobbyId, out _);
                }
                else
                {
                    _logger.LogError($"HandleOnCloseRoom: lobby room {e.RoomLobbyId} not found in local state");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"HandleOnCloseRoom: Unhandled error when starting match for room {e.RoomLobbyId} - {ex.Message}");
                _rooms.TryRemove(e.RoomLobbyId, out _);
            }
        }

        private async Task RequireValidJoinOptionsRequestAsync(JoinLobbyRequest request)
        {
            var defaultPlayers = new[] { 1, 2, 5, 10 }; 
            if (!defaultPlayers.Contains(request.NumberOfPlayers))
            {
                throw new BadRequestException("Number of players must be in 1, 2, 5, 10");
            }

            if (request.ContentType == MatchContentType.OnlyOne)
            {
                if (request.TopicId == null || request.TopicId <= 0)
                {
                    throw new BadRequestException("TopicId is required when ContentType is OnlyOne");
                }

                var topicExists = await _uow.Repository<Topic>().GetFirstAsync(e => e.Id == request.TopicId) != null;
                if (!topicExists)
                {
                    throw new BadRequestException($"Topic {request.TopicId} not found");
                }
            }
        }
    }
}
