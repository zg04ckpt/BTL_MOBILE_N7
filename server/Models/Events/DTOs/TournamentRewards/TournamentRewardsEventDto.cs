using Models.Events.DTOs.LuckySpin;
using Models.Events.Entities;
using System.Text.Json;

namespace Models.Events.DTOs.TournamentRewards
{
    public class TournamentRewardsEventDto : EventDto
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public List<TournamentRewardsTaskDto> Tasks { get; set; }

        public static TournamentRewardsEventDto MapFromEntity(Event entity)
        {
            var configJson = UnwrapConfigJson(entity.EventConfigJsonData);

            return new TournamentRewardsEventDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Desc = entity.Desc,
                EndTime = entity.EndTime?.ToLocalTime(),
                IsLocked = entity.IsLocked,
                Tasks = JsonSerializer.Deserialize<List<TournamentRewardsTaskDto>>(configJson, JsonSerializerOptions)!,
                StartTime = entity.StartTime.ToLocalTime(),
                TimeType = entity.EventTimeType,
                Type = entity.EventType
            };
        }

        private static string UnwrapConfigJson(string rawJson)
        {
            try
            {
                using var doc = JsonDocument.Parse(rawJson);
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                {
                    return rawJson;
                }

                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (string.Equals(prop.Name, "EventConfigJsonData", StringComparison.OrdinalIgnoreCase)
                        && prop.Value.ValueKind == JsonValueKind.String)
                    {
                        return prop.Value.GetString() ?? rawJson;
                    }
                }
            }
            catch
            {
            }

            return rawJson;
        }
    }
}
