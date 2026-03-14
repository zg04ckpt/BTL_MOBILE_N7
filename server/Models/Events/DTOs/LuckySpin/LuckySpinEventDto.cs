using Models.Events.Entities;
using System.Text.Json;

namespace Models.Events.DTOs.LuckySpin
{
    public class LuckySpinEventDto : EventDto
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public int MaxSpinTimePerDay { get; set; }
        public List<LuckySpinItemDto> SpinItems { get; set; }

        public static LuckySpinEventDto MapFromEntity(Event entity)
        {
            var configJson = UnwrapConfigJson(entity.EventConfigJsonData);
            var config = JsonSerializer.Deserialize<LuckySpinEventDto>(configJson, JsonSerializerOptions)
                ?? new LuckySpinEventDto();

            return new LuckySpinEventDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Desc = entity.Desc,
                EndTime = entity.EndTime?.ToLocalTime(),
                IsLocked = entity.IsLocked,
                MaxSpinTimePerDay = config.MaxSpinTimePerDay,
                SpinItems = config.SpinItems ?? new(),
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
