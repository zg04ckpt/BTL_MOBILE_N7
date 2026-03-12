using Core.Models;
using Feature.Matchs.Models;
using Feature.Matchs.Models.Realtimes;
using Feature.Matchs.Models.Requests;

namespace Feature.Matchs.Interfaces
{
    public interface IMatchService
    {
        Task StartMatchAsync(LobbyRoomDto roomInfo, List<PlayerInLobbyInfoDto> players);
        Task<StartMatchInfoDto> GetMatchInfoAsync(int userId);
        Task<MatchResultDto> GetMatchResultAsync(int userId);
        Task<Paginated<MatchListItemDto>> GetAllMatchsPagingAsync(SearchMatchRequest request);
        Task<MatchDetailDto> GetMatchDetailAsync(int matchId);
        Task<ChangedResponse> DeleteMatchAsync(int matchId);
    }
}
