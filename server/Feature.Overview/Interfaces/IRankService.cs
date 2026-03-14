using Models.Overviews.DTOs;
using Models.Overviews.Enums;
using Models.Overviews.Requests;

namespace Feature.Overview.Interfaces
{
    public interface IRankService
    {
        Task<UserRankListItemDto[]> GetRankBoardAsync(GetRankBoardRequest request);
        Task<UserRankDto> GetRankInfoAsync(int userId);
    }
}
