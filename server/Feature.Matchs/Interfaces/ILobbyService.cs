using Feature.Matchs.Enums;
using Feature.Matchs.Models.Realtimes;
using Feature.Matchs.Models.Requests;

namespace Feature.Matchs.Interfaces
{
    public interface ILobbyService
    {
        Task<string> JoinMatchLobbyAsync(int userId, JoinLobbyRequest request);
        Task<string> OutLobbyRoomAsync(int userId, OutLobbyRequest request);
        Task<MatchConfigOptions> GetOptionsAsync();
    }
}
