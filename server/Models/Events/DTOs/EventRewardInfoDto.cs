using Models.Events.Enums;
using System.Text.Json.Serialization;

namespace Models.Events.DTOs
{
    public class EventRewardInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventRewardType Type { get; set; }

        public string Desc { get; set; }
        public string Unit { get; set; }
    }
}
