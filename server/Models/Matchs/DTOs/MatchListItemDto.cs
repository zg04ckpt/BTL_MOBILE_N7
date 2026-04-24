using Models.Matchs.Enums;
using Models.Matchs.Enums;
using System.Text.Json.Serialization;

namespace Models.Matchs.DTOs
{
    public class MatchListItemDto
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BattleType BattleType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MatchStatus BattleStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MatchContentType ContentType { get; set; }

        public int NumberOfPlayers { get; set; }
        public string? TopicName { get; set; }
    }
}
