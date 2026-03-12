using Feature.Matchs.Entities;
using Feature.Matchs.Models;
using Feature.Matchs.Models.Realtimes;
using Feature.Quizzes.Entities;

namespace Feature.Matchs.Interfaces
{
    public interface IMatchRealtimeService
    {
        event EventHandler<RealtimeRoomLobbyInfoDto>? OnCloseRoom;
        event EventHandler<(string trackingId, List<MatchPlayerAnswerDto> answers)>? OnPlayerPickAnswer;

        Task<bool> AddNewLobbyRoomAsync(LobbyRoomDto room);
        Task<bool> AddPlayerToRoomAsync(string roomId, PlayerInLobbyInfoDto player);
        Task<bool> RemovePlayerFromRoomAsync(string roomId, int playerId);
        Task<bool> AddNewMatchRoomAsync(string trackingId, List<PlayerInLobbyInfoDto> players, Match match);
        Task<bool> UpdateLeaderboardAsync(string trackingId, List<UserMappingDto> updatedUsers);
        Task<bool> CloseMatchRoomAsync(string trackingId);
    }
}
