using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

namespace Models.Matchs.DTOs
{
    public class MatchRoomMappingDto
    {
        public int MatchId { get; set; }
        public int ScorePerQuestions { get; set; }
        public int MaxSecondPerQuestion { get; set; }
        public DateTime MatchStartedAtUtc { get; set; }
        public bool IsSolo { get; set; }
        public Dictionary<int, QuestionMappingDto> Questions { get; set; }
        public Dictionary<int, UserMappingDto> Users { get; set; }
        public ConcurrentDictionary<string, bool> ProcessedAnswerKeys { get; set; } = new();
    }

    public class QuestionMappingDto
    {
        public int QuestionId { get; set; }
        public List<string> Answers { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
    }

    public class UserMappingDto
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public int Progress { get; set; }
        public int Score { get; set; }
        public bool IsFinished { get; set; }
        public DateTime? FinishedAtUtc { get; set; }
        public ConcurrentDictionary<int, UserQuestionAnswerMappingDto> Answers { get; set; } = new();
    }

    public class UserQuestionAnswerMappingDto
    {
        public int QuestionId { get; set; }
        public List<string> Answers { get; set; } = new();
        public bool IsCorrect { get; set; }
    }
}
