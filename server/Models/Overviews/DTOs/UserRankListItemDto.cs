namespace Models.Overviews.DTOs
{
    public class UserRankListItemDto
    {
        public int UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string DisplayName { get; set; }
        public int RankScore { get; set; }
        public int Rank { get; set; }
        public int Level { get; set; }
    }
}
