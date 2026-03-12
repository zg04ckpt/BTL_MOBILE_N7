using Feature.Matchs.Entities;
using Feature.Matchs.Enums;
using Feature.Quizzes.Entities;
using Feature.Quizzes.Models;

namespace Feature.Matchs.Models
{
    public class MatchDetailDto
    {
        public int Id { get; set; }
        public int MaxSecondPerQuestion { get; set; }
        public int ScorePerCorrectAnswer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public int? TopicId { get; set; }
        public MatchContentType ContentType { get; set; }
        public BattleType BattleType { get; set; }
        public MatchStatus Status { get; set; }
        public int NumberOfPlayers { get; set; }
        public string? TopicName { get; set; }

        public List<QuestionListItemDto> Questions { get; set; }
        public List<UserInMatchResultDto> Users { get; set; }
    }
}
