using Core.Models;
using Models.Overviews.DTOs;
using Models.Overviews.Requests;

namespace Feature.Overview.Interfaces
{
    public interface IAnalyticsService
    {
        Task<SystemAnalyticsDto> GetSystemAnalyticsAsync(AnalyticsFilterRequest filter);
        Task<Paginated<RecentUserDto>> GetRecentUsersAsync(RecentUsersRequest request);
        Task<byte[]> ExportAnalyticsAsync(AnalyticsFilterRequest filter);
    }
}
