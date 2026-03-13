using Models.Overviews.DTOs;
using Models.Overviews.Enums;

namespace Feature.Overview.Interfaces
{
    public interface IRankService
    {
        Task<UserRankListItemDto[]> GetRankBoardAsync(RankingType rankingType);
        Task<UserRankDto> GetRankInfoAsync(int userId);
    }
}
