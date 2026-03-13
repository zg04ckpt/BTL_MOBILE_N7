using Models.Overviews.Enums;
using System.Text.Json.Serialization;

namespace Models.Overviews.Requests
{
    public class GetRankBoardRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RankingType RankingType { get; set; }
    }
}
