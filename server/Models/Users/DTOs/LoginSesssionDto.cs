namespace Models.Users.DTOs
{
    public class LoginSesssionDto
    {
        public string AccessToken { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public int RankScore { get; set; }
    }
}
