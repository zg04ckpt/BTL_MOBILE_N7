using CNLib.Services.Logs;
using Core.Exceptions;
using Core.Interfaces;
using Feature.Matchs.Enums;
using Feature.Matchs.Interfaces;
using Feature.Matchs.Models;
using Feature.Matchs.Models.Requests;
using Feature.Quizzes.Entities;
using Feature.Quizzes.Models;
using Feature.Users.Entities;
using System.Collections.Concurrent;

namespace Feature.Matchs.Services
{
    public class LobbyService : ILobbyService
    {
        private readonly IMatchRealtimeService _matchRealtimeService;
        private static readonly ConcurrentDictionary<string, LobbyRoomDto> _rooms = new();
        private readonly IUnitOfWork _uow;
        private readonly ILogService<LobbyService> _logger;

        public LobbyService(
            IMatchRealtimeService matchRealtimeService,
            IUnitOfWork uow,
            ILogService<LobbyService> logger)
        {
            _matchRealtimeService = matchRealtimeService;
            _uow = uow;
            _logger = logger;

            _matchRealtimeService.OnCloseRoom += HandleOnCloseRoom;
        }

        public async Task<MatchConfigOptions> GetOptionsAsync()
        {
            var topics = new object[]
            {
                new { Id = -1, Name = "Hỗn hợp" },
                new { Id = 0, Name = "Ngẫu nhiên" }
            }
            .Concat(await _uow.Repository<Topic>().GetAllAsync(
                predicate: e => true,
                selector: e => new
                {
                    e.Id,
                    e.Name
                })
            );

            return new MatchConfigOptions
            {
                NumberOfPlayers = new int[] { 1, 2, 5, 10 },
                TopicsOfContent = topics.ToArray(),
                TypesOfBattle = new[]
                {
                    new { Type = BattleType.Single, Label = "Đấu đơn vượt ải" },
                    new { Type = BattleType.Team, Label = "Đấu đội" },
                }
            };
        }

        public async Task<string> OutLobbyRoomAsync(int userId, OutLobbyRequest request)
        {
            if (!_rooms.ContainsKey(request.LobbyRoomId))
            {
                throw new BadRequestException($"Room {request.LobbyRoomId} not found");
            }

            var removeResult = await _matchRealtimeService.RemovePlayerFromRoomAsync(request.LobbyRoomId, userId);
            if (!removeResult)
            {
                throw new ServerErrorException($"Failed to remove player {userId} from lobby room {request.LobbyRoomId}");
            }

            return request.LobbyRoomId;
        }

        public async Task<string> JoinMatchLobbyAsync(int userId, JoinLobbyRequest request)
        {
            RequireValidJoinOptionsRequest(request);

            var suitableRoom = _rooms.Values.FirstOrDefault(room =>
            {
                if (room.TopicId == request.TopicId &&
                    room.MaxPlayers == request.NumberOfPlayers)
                {
                    return true;
                }
                return false;
            });

            // Create new room if no suitable room
            if (suitableRoom == null)
            {
                suitableRoom = new LobbyRoomDto
                {
                    Id = Guid.NewGuid().ToString(),
                    MaxPlayers = request.NumberOfPlayers,
                    Status = LobbyRoomStatus.InQueue,
                    TopicId = request.TopicId,
                };
                var addResult = await _matchRealtimeService.AddNewLobbyRoomAsync(suitableRoom);
                if (!addResult)
                {
                    throw new ServerErrorException($"Failed to add new lobby room {suitableRoom.Id}");
                }

                _rooms.TryAdd(suitableRoom.Id, suitableRoom);
            }

            // Add player to lobby room
            var user = await _uow.Repository<User>().GetFirstAsync(
                predicate: u => u.Id == userId)
                ?? throw new NotFoundException("User not found");

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

            return suitableRoom.Id;
        }

        private void HandleOnCloseRoom(object? sender, RealtimeRoomLobbyInfoDto e)
        {
            _rooms.TryRemove(e.RoomLobbyId, out _);

            // TODO: start new battle
            _logger.LogInfo($"Start new battle [Battle] from room {e.RoomLobbyId}");

            _matchRealtimeService.OnCloseRoom -= HandleOnCloseRoom;
        }

        private void RequireValidJoinOptionsRequest(JoinLobbyRequest request)
        {
            var defaultPlayers = new[] { 1, 2, 5, 10 }; 
            if (!defaultPlayers.Contains(request.NumberOfPlayers))
            {
                throw new BadRequestException("Number of players must be in 1, 2, 5, 10");
            }
        }
    }
}
