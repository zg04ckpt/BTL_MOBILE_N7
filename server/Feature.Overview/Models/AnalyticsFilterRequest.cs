using System.ComponentModel.DataAnnotations;

namespace Feature.Overview.Models
{
    public class AnalyticsFilterRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Range(1, 365, ErrorMessage = "Days must be between 1 and 365")]
        public int? LastDays { get; set; }

        public AnalyticsFilterRequest()
        {
            if (!StartDate.HasValue && !EndDate.HasValue && !LastDays.HasValue)
            {
                LastDays = 7;
            }
        }

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
            var start = end.AddDays(-7);
            return (start, end);
        }

        public (DateTime previousStartDate, DateTime previousEndDate) GetPreviousDateRange()
        {
            var (start, end) = GetDateRange();
            var duration = (end - start).TotalDays;
            
            var prevEnd = start;
            var prevStart = prevEnd.AddDays(-duration);
            
            return (prevStart, prevEnd);
        }
    }
}
