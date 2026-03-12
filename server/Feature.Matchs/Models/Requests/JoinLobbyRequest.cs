using Feature.Matchs.Enums;

namespace Feature.Matchs.Models.Requests
{
    public class JoinLobbyRequest
    {
        public BattleType BattleType { get; set; }
        public int NumberOfPlayers { get; set; }
        public int TopicId { get; set; } // 0 là hỗn hợp, -1 là ngẫu nhiên
    }
}
