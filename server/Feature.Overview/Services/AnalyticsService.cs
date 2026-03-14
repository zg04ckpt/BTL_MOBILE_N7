using Core.Base;
using Core.Interfaces;
using Core.Models;
using Feature.Overview.Interfaces;
using Feature.Users.Entities;
using Models.Overviews.DTOs;
using Models.Overviews.Requests;
using Models.Users.Enums;
using System.Text;

namespace Feature.Overview.Services
{
    public class AnalyticsService : BaseService, IAnalyticsService
    {
        public AnalyticsService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<SystemAnalyticsDto> GetSystemAnalyticsAsync(AnalyticsFilterRequest filter)
        {
            var (startDate, endDate) = filter.GetDateRange();
            var (prevStartDate, prevEndDate) = filter.GetPreviousDateRange();

            var userRepository = _uow.Repository<User>();

            var overview = await GetOverviewMetricsAsync(userRepository, startDate, endDate, prevStartDate, prevEndDate);
            var userTrend = await GetUserTrendAsync(userRepository, startDate, endDate);
            var statusDistribution = await GetAccountStatusDistributionAsync(userRepository);
            var recentUsers = await GetRecentUsersListAsync(userRepository, startDate, endDate, 5);
            
            return new SystemAnalyticsDto
            {
                Overview = overview,
                UserTrend = userTrend,
                AccountStatusDistribution = statusDistribution,
                RecentUsers = recentUsers,
                GeneratedAt = DateTime.UtcNow
            };
        }

        public async Task<Paginated<RecentUserDto>> GetRecentUsersAsync(RecentUsersRequest request)
        {
            var (startDate, endDate) = request.GetDateRange();
            var userRepository = _uow.Repository<User>();

            var users = await userRepository.GetPagingAsync(
                predicate: u => u.CreatedAt >= startDate && u.CreatedAt <= endDate,
                selector: u => new RecentUserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    AvatarUrl = u.AvatarUrl,
                    RegisteredAt = u.CreatedAt,
                    Status = u.Status.ToString(),
                    TotalMatches = 0 // TODO: Calculate from matches when Match feature is implemented
                },
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                orderBy: nameof(User.CreatedAt),
                asc: false
            );
            
            return users;
        }

        public async Task<byte[]> ExportAnalyticsAsync(AnalyticsFilterRequest filter)
        {
            var (startDate, endDate) = filter.GetDateRange();
            var analytics = await GetSystemAnalyticsAsync(filter);

            var csv = new StringBuilder();
            csv.AppendLine("Quiz Battle - System Analytics Report");
            csv.AppendLine($"Generated: {analytics.GeneratedAt:yyyy-MM-dd HH:mm:ss}");
            csv.AppendLine($"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
            csv.AppendLine();

            // Overview Metrics
            csv.AppendLine("OVERVIEW METRICS");
            csv.AppendLine("Metric,Value,Change %");
            csv.AppendLine($"New Users,{analytics.Overview.NewUsers},{analytics.Overview.NewUsersChangePercent:F2}%");
            csv.AppendLine($"Total Users,{analytics.Overview.TotalUsers},{analytics.Overview.TotalUsersChangePercent:F2}%");
            csv.AppendLine($"Total Registrations,{analytics.Overview.TotalRegistrations},{analytics.Overview.TotalRegistrationsChangePercent:F2}%");
            csv.AppendLine($"Peak CCU,{analytics.Overview.PeakCCU},{analytics.Overview.PeakCCUChangePercent:F2}%");
            csv.AppendLine();

            // Account Status Distribution
            csv.AppendLine("ACCOUNT STATUS DISTRIBUTION");
            csv.AppendLine("Status,Count,Percentage");
            csv.AppendLine($"Active,{analytics.AccountStatusDistribution.Active},{(double)analytics.AccountStatusDistribution.Active / analytics.AccountStatusDistribution.Total * 100:F2}%");
            csv.AppendLine($"Banned,{analytics.AccountStatusDistribution.Banned},{(double)analytics.AccountStatusDistribution.Banned / analytics.AccountStatusDistribution.Total * 100:F2}%");
            csv.AppendLine($"Inactive,{analytics.AccountStatusDistribution.Inactive},{(double)analytics.AccountStatusDistribution.Inactive / analytics.AccountStatusDistribution.Total * 100:F2}%");
            csv.AppendLine();

            // Daily User Trend
            csv.AppendLine("DAILY USER TREND");
            csv.AppendLine("Date,New Users,Active Users");
            foreach (var trend in analytics.UserTrend)
            {
                csv.AppendLine($"{trend.Date:yyyy-MM-dd},{trend.NewUsers},{trend.ActiveUsers}");
            }
            csv.AppendLine();

            // Recent Users
            csv.AppendLine("RECENT USERS");
            csv.AppendLine("ID,Name,Email,Registered At,Status,Total Matches");
            foreach (var user in analytics.RecentUsers)
            {
                csv.AppendLine($"{user.Id},{user.Name},{user.Email},{user.RegisteredAt:yyyy-MM-dd HH:mm:ss},{user.Status},{user.TotalMatches}");
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

        private async Task<OverviewMetrics> GetOverviewMetricsAsync(
            IRepository<User> userRepository,
            DateTime startDate,
            DateTime endDate,
            DateTime prevStartDate,
            DateTime prevEndDate)
        {
            // Current period
            var newUsers = await userRepository.CountAsync(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate);
            var totalUsers = await userRepository.CountAsync();
            var totalRegistrations = totalUsers;

            // Previous period
            var prevNewUsers = await userRepository.CountAsync(u => u.CreatedAt >= prevStartDate && u.CreatedAt < startDate);
            var prevTotalUsers = await userRepository.CountAsync(u => u.CreatedAt < startDate);

            // TODO: Implement CCU tracking when sessions/online tracking is added
            var peakCCU = 0;
            var prevPeakCCU = 0;

            return new OverviewMetrics
            {
                NewUsers = newUsers,
                NewUsersChangePercent = CalculateChangePercent(newUsers, prevNewUsers),
                
                TotalUsers = totalUsers,
                TotalUsersChangePercent = CalculateChangePercent(totalUsers, prevTotalUsers),
                
                TotalRegistrations = totalRegistrations,
                TotalRegistrationsChangePercent = CalculateChangePercent(totalRegistrations, prevTotalUsers),
                
                PeakCCU = peakCCU,
                PeakCCUChangePercent = CalculateChangePercent(peakCCU, prevPeakCCU)
            };
        }

        private async Task<List<DailyUserTrendDto>> GetUserTrendAsync(
            IRepository<User> userRepository,
            DateTime startDate,
            DateTime endDate)
        {
            var users = await userRepository.GetAllAsync(
                predicate: u => u.CreatedAt >= startDate && u.CreatedAt <= endDate,
                selector: u => new { u.CreatedAt }
            );

            var trend = new List<DailyUserTrendDto>();
            var currentDate = startDate.Date;

            while (currentDate <= endDate.Date)
            {
                var nextDate = currentDate.AddDays(1);
                var newUsersCount = users.Count(u => u.CreatedAt >= currentDate && u.CreatedAt < nextDate);
                
                trend.Add(new DailyUserTrendDto
                {
                    Date = currentDate,
                    NewUsers = newUsersCount,
                    ActiveUsers = 0 // TODO: Implement when activity tracking is added
                });

                currentDate = nextDate;
            }

            return trend;
        }

        private async Task<AccountStatusDistributionDto> GetAccountStatusDistributionAsync(IRepository<User> userRepository)
        {
            var activeCount = await userRepository.CountAsync(u => u.Status == AccountStatus.Active);
            var bannedCount = await userRepository.CountAsync(u => u.Status == AccountStatus.Banned);
            var inactiveCount = await userRepository.CountAsync(u => u.Status == AccountStatus.Inactive);
            var total = await userRepository.CountAsync();

            return new AccountStatusDistributionDto
            {
                Active = activeCount,
                Banned = bannedCount,
                Inactive = inactiveCount,
                Total = total
            };
        }

        private async Task<List<RecentUserDto>> GetRecentUsersListAsync(
            IRepository<User> userRepository,
            DateTime startDate,
            DateTime endDate,
            int limit)
        {
            var users = await userRepository.GetAllAsync(
                predicate: u => u.CreatedAt >= startDate && u.CreatedAt <= endDate,
                selector: u => new RecentUserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    AvatarUrl = u.AvatarUrl,
                    RegisteredAt = u.CreatedAt,
                    Status = u.Status.ToString(),
                    TotalMatches = 0 // TODO: Calculate from matches
                },
                pageIndex: 1,
                pageSize: limit,
                orderBy: nameof(User.CreatedAt),
                asc: false
            );

            return users.ToList();
        }

        private double CalculateChangePercent(int current, int previous)
        {
            if (previous == 0)
                return current > 0 ? 100.0 : 0.0;

            return ((double)(current - previous) / previous) * 100.0;
        }
    }
}
