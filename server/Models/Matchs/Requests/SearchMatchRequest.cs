using Core.Models;
using Models.Matchs.Enums;
using System.Text.Json.Serialization;

namespace Models.Matchs.Requests
{
    public class SearchMatchRequest : PagingRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BattleType? BattleType { get; set; }
        public int? NumberOfPlayers { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
