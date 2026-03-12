using Feature.Overview.Enums;
using Feature.Overview.Models;

namespace Feature.Overview.Interfaces
{
    public interface IRankService
    {
        Task<UserRankListItemDto[]> GetRankBoardAsync(RankingType rankingType);
        Task<UserRankDto> GetRankInfoAsync(int userId);
    }
}
