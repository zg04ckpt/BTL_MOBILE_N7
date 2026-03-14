using Models.Events.Enums;
using System.Text.Json.Serialization;

namespace Models.Events.DTOs.TournamentRewards
{
    public class TournamentRewardsTaskDto
    {
        public int TaskId { get; set; }
        public string ShortDesc { get; set; }
        public List<EventRewardDto> Rewards { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TournamentRewardsTaskType Type { get; set; }
        public int NumberOfMatchs { get; set; }
    }
}
