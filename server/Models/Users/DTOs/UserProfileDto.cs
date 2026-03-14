namespace Models.Users.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public int Exp { get; set; }
        public int RankScore { get; set; }
        public int WinningStreak { get; set; }
        public float WinningRate { get; set; }
        public int NumberOfMatchs { get; set; }
    }
}
