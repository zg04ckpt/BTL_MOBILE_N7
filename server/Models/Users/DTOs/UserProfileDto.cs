namespace Models.Users.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public int RankScore { get; set; }
        public int WinningStreak { get; set; }
        public decimal WinningRate { get; set; }
    }
}
