using Feature.Quizzes.Entities;
using Feature.Quizzes.Models;

namespace Feature.Matchs.Models.Realtimes
{
    public class MatchConfigOptions
    {
        public object[] TypesOfBattle { get; set; }
        public object[] ContentTypes { get; set; }
        public int[] NumberOfPlayers { get; set; }
        public object[] TopicsOfContent { get; set; }
    }
}
