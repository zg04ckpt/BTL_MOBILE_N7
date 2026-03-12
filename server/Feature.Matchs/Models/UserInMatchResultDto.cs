using Feature.Matchs.Enums;

namespace Feature.Matchs.Models
{
    public class UserInMatchResultDto
    {
        public int UserId { get; set; }
        public decimal Duration { get; set; }
        public int Progress { get; set; }
        public int TotalQuestion { get; set; }
        public int Correct { get; set; }
        public int Score { get; set; }
        public UserInMatchStatus Status { get; set; }
        public string UserDisplayName { get; set; }
        public int Level { get; set; }
    }
}
