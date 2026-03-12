using Feature.Matchs.Models;

namespace Feature.Matchs.Interfaces
{
    public interface IMatchRealtimeService
    {
        event EventHandler<RealtimeRoomLobbyInfoDto>? OnCloseRoom;
        Task<bool> AddNewLobbyRoomAsync(LobbyRoomDto room);
        Task<bool> AddPlayerToRoomAsync(string roomId, PlayerInLobbyInfoDto player);
        Task<bool> RemovePlayerFromRoomAsync(string roomId, int playerId);
    }
}
