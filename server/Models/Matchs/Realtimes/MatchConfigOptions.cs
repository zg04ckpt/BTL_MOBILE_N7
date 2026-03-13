using Models.Quizzes.Entities;
using Models.Quizzes.DTOs;

namespace Models.Matchs.Realtimes
{
    public class MatchConfigOptions
    {
        public object[] TypesOfBattle { get; set; }
        public object[] ContentTypes { get; set; }
        public int[] NumberOfPlayers { get; set; }
        public object[] TopicsOfContent { get; set; }
    }
}
