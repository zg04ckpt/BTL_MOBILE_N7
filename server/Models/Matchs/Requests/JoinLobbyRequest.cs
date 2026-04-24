using System.Text.Json.Serialization;
using Models.Matchs.Enums;

namespace Models.Matchs.Requests
{
    public class JoinLobbyRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BattleType BattleType { get; set; }
        public int NumberOfPlayers { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MatchContentType ContentType { get; set; }
        public int? TopicId { get; set; }
    }
}
