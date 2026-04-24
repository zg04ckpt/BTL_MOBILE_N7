using Models.Events.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Events.Requests
{
    public class CreateEventRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, ErrorMessage = "Name must be at most 200 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Desc is required.")]
        [StringLength(1000, ErrorMessage = "Desc must be at most 200 characters.")]
        public string Desc { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required(ErrorMessage = "Event type is required.")]
        [EnumDataType(typeof(EventType), ErrorMessage = "Event type is invalid.")]
        public EventType Type { get; set; }

        [Required(ErrorMessage = "Event config JSON data is required.")]
        public string EventConfigJsonData { get; set; }
    }
}
