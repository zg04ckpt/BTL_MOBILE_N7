namespace Feature.Overview.Models
{
    public class UserRankDto : UserRankListItemDto
    {
        public int NumberOfMatchs { get; set; }
        public decimal WinRate { get; set; }
    }
}
