using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Matchs.DTOs
{
    public class MatchRoomMappingDto
    {
        public int MatchId { get; set; }
        public int ScorePerQuestions { get; set; }
        public int MaxSecondPerQuestion { get; set; }
        public Dictionary<int, QuestionMappingDto> Questions { get; set; }
        public Dictionary<int, UserMappingDto> Users { get; set; }
    }

    public class QuestionMappingDto
    {
        public int QuestionId { get; set; }
        public List<string> Answers { get; set; }
    }

    public class UserMappingDto
    {
        public int UserId { get; set; }
        public int Progress { get; set; }
        public int Score { get; set; }
    }
}
