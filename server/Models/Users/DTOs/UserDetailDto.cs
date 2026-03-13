using Models.Users.Enums;

namespace Models.Users.DTOs
{
    public class UserDetailDto
    {
        public int Id { get; set; }
        public string AvatarUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public AccountStatus Status { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public int RankScore { get; set; }
        public int Exp { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
