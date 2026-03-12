using System.ComponentModel.DataAnnotations;

namespace Feature.Quizzes.Models.Requests
{
    public class CreateTopicRequest
    {
        [Required(ErrorMessage = "Topic name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; }
    }
}
