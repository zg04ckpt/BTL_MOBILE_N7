using System.ComponentModel.DataAnnotations;

namespace Models.Quizzes.Requests
{
    public class UpdateTopicRequest
    {
        [Required(ErrorMessage = "Topic name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; }
    }
}
