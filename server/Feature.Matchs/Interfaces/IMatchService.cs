using Core.Models;
using Models.Matchs.DTOs;
using Models.Matchs.Realtimes;
using Models.Matchs.Requests;

namespace Feature.Matchs.Interfaces
{
    public interface IMatchService
    {
        Task StartMatchAsync(LobbyRoomDto roomInfo, List<PlayerInLobbyInfoDto> players);
        Task<StartMatchInfoDto> StartSoloMatchAsync(int userId, StartSoloMatchRequest request);
        Task<StartMatchInfoDto> GetMatchInfoAsync(int userId);
        Task<StartMatchInfoDto> GetMatchInfoByTrackingAsync(int userId, string trackingId);
        Task<MatchStateDto> GetMatchStateAsync(int userId);
        Task SubmitMatchAnswerAsync(int userId, SubmitMatchAnswerRequest request);
        Task<MatchLoudspeakerInventoryDto> GetMyLoudspeakerInventoryAsync(int userId);
        Task<MatchLoudspeakerInventoryDto> UseLoudspeakerAsync(int userId, UseMatchLoudspeakerRequest request);
        Task<MatchResultDto> GetMatchResultAsync(int userId);
        Task<MatchResultDto> GetMatchResultByMatchIdAsync(int userId, int matchId);
        Task<MatchReviewDto> GetMatchReviewByMatchIdAsync(int userId, int matchId);
        Task<Paginated<MatchListItemDto>> GetAllMatchsPagingAsync(SearchMatchRequest request);
        Task<MatchDetailDto> GetMatchDetailAsync(int matchId);
        Task<ChangedResponse> DeleteMatchAsync(int matchId);
    }
}
