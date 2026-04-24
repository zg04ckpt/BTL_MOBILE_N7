namespace Models.Overviews.DTOs
{
    public class UserRankDto : UserRankListItemDto
    {
        public int NumberOfMatchs { get; set; }
        public float WinningRate { get; set; }
        public int WinningStreak { get; set; }
    }
}
