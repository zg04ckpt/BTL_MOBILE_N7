using Models.Matchs.Enums;

namespace Models.Matchs.Requests
{
    public class JoinLobbyRequest
    {
        public BattleType BattleType { get; set; }
        public int NumberOfPlayers { get; set; }
        public MatchContentType ContentType { get; set; }
        public int? TopicId { get; set; }
    }
}
