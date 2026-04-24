using System.ComponentModel.DataAnnotations;

namespace Models.Events.Requests
{
    public class UpdateMyEventProgressRequest
    {
        [Required(ErrorMessage = "Event id is required.")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Progress JSON data is required.")]
        public string ProgressJsonData { get; set; }
    }
}
