using Core.Models;

namespace Models.Overviews.Requests
{
    public class RecentUsersRequest : PagingRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? LastDays { get; set; }

        public (DateTime startDate, DateTime endDate) GetDateRange()
        {
            if (StartDate.HasValue && EndDate.HasValue)
            {
                return (StartDate.Value, EndDate.Value);
            }

            if (LastDays.HasValue)
            {
                var endDate = DateTime.UtcNow;
                var startDate = endDate.AddDays(-LastDays.Value);
                return (startDate, endDate);
            }

            var end = DateTime.UtcNow;
            var start = end.AddDays(-30);
            return (start, end);
        }
    }
}
