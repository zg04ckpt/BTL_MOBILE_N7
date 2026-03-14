using Models.Events.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Events.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventTimeType TimeType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventType Type { get; set; }

        public bool IsLocked { get; set; }
    }
}
