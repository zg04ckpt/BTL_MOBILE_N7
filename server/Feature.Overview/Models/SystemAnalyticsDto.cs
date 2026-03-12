namespace Feature.Overview.Models
{
    public class SystemAnalyticsDto
    {
        public OverviewMetrics Overview { get; set; }
        public List<DailyUserTrendDto> UserTrend { get; set; }
        public AccountStatusDistributionDto AccountStatusDistribution { get; set; }
        public List<RecentUserDto> RecentUsers { get; set; }
        public DateTime GeneratedAt { get; set; }
    }

    public class OverviewMetrics
    {
        public int NewUsers { get; set; }
        public double NewUsersChangePercent { get; set; }
        
        public int TotalUsers { get; set; }
        public double TotalUsersChangePercent { get; set; }
        
        public int TotalRegistrations { get; set; }
        public double TotalRegistrationsChangePercent { get; set; }
        
        public int PeakCCU { get; set; }
        public double PeakCCUChangePercent { get; set; }
    }

    public class DailyUserTrendDto
    {
        public DateTime Date { get; set; }
        public int NewUsers { get; set; }
        public int ActiveUsers { get; set; }
    }

    public class AccountStatusDistributionDto
    {
        public int Active { get; set; }
        public int Banned { get; set; }
        public int Inactive { get; set; }
        public int Total { get; set; }
    }

    public class RecentUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Status { get; set; }
        public int TotalMatches { get; set; }
    }
}
