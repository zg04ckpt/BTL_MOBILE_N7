using Core.Models;
using Feature.Overview.Models;
using Feature.Overview.Models.Requests;

namespace Feature.Overview.Interfaces
{
    public interface IAnalyticsService
    {
        Task<SystemAnalyticsDto> GetSystemAnalyticsAsync(AnalyticsFilterRequest filter);
        Task<Paginated<RecentUserDto>> GetRecentUsersAsync(RecentUsersRequest request);
        Task<byte[]> ExportAnalyticsAsync(AnalyticsFilterRequest filter);
    }
}
