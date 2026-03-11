namespace Feature.Overview.Models
{
    public class UserRankListItemDto
    {
        public int UserId { get; set; }
        public string AvatarUrl { get; set; }
        public string FullName { get; set; }
        public int RankScore { get; set; }
        public int Rank { get; set; }
    }
}
