using Feature.Overview.Enums;
using System.Text.Json.Serialization;

namespace Feature.Overview.Models.Requests
{
    public class GetRankBoardRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RankingType RankingType { get; set; }
    }
}
