using Models.Events.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Events.Requests
{
    public class UpdateEventRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, ErrorMessage = "Name must be at most 200 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Desc is required.")]
        [StringLength(1000, ErrorMessage = "Desc must be at most 200 characters.")]
        public string Desc { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Required(ErrorMessage = "Event config JSON data is required.")]
        public string EventConfigJsonData { get; set; }

        [Required(ErrorMessage = "Lock status is required.")]
        public bool IsLocked { get; set; }
    }
}
