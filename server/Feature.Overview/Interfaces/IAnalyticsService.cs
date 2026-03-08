using Core.Models;
using Feature.Overview.Models;

namespace Feature.Overview.Interfaces
{
    public interface IAnalyticsService
    {
        Task<SystemAnalyticsDto> GetSystemAnalyticsAsync(AnalyticsFilterRequest filter);
        Task<Paginated<RecentUserDto>> GetRecentUsersAsync(RecentUsersRequest request);
        Task<byte[]> ExportAnalyticsAsync(AnalyticsFilterRequest filter);
    }
}
