using System.Text.Json.Serialization;
using Models.Matchs.Enums;

namespace Models.Matchs.Requests
{
    public class StartSoloMatchRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MatchContentType ContentType { get; set; }
        public int? TopicId { get; set; }
        public List<int>? FixedQuestionIds { get; set; }
    }
}
