namespace Models.Matchs.DTOs
{
    public class MatchResultDto
    {
        public int MatchId { get; set; }
        public List<UserMatchResultItemDto> Users { get; set; }
    }

    public class UserMatchResultItemDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public int Score { get; set; }
        public int ExpGained { get; set; }
        public int RankScoreGained { get; set; }
        public bool IsRankProtected { get; set; }
    }
}
