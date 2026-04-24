using Models.Matchs.DTOs;
using Models.Matchs.Entities;
using Models.Matchs.Realtimes;

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
        Task<bool> AddPlayerAnswerAsync(string trackingId, MatchPlayerAnswerDto answer);
        Task<bool> UpdateLeaderboardAsync(string trackingId, List<UserMappingDto> updatedUsers);
        Task<Dictionary<int, MatchPlayerInfoDto>> GetPlayerPresenceAsync(string trackingId);
        Task<bool> AppendMatchEventAsync(string trackingId, MatchRealtimeEventDto matchEvent, string? statusLog = null);
        Task<bool> CloseMatchRoomAsync(string trackingId);
    }
}
