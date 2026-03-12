using Feature.Matchs.Enums;

namespace Feature.Matchs.Models.Requests
{
    public class JoinLobbyRequest
    {
        public BattleType BattleType { get; set; }
        public int NumberOfPlayers { get; set; }
        public MatchContentType ContentType { get; set; }
        public int? TopicId { get; set; }
    }
}
