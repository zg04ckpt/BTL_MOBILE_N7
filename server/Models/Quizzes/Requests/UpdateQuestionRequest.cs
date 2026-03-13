using Microsoft.AspNetCore.Http;
using Models.Quizzes.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Quizzes.Requests
{
    public class UpdateQuestionRequest
    {
        [Required(ErrorMessage = "Question content is required")]
        [MaxLength(1000, ErrorMessage = "Content must not exceed 1000 characters")]
        public string StringContent { get; set; }

        public IFormFile? Image { get; set; }

        public IFormFile? Audio { get; set; }

        public IFormFile? Video { get; set; }

        [Required(ErrorMessage = "Question type is required")]
        public QuestionType Type { get; set; }

        [Required(ErrorMessage = "Question level is required")]
        public QuestionLevel Level { get; set; }

        [Required(ErrorMessage = "Question status is required")]
        public QuestionStatus Status { get; set; }

        [Required(ErrorMessage = "Topic ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Topic ID must be greater than 0")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Correct answers are required")]
        [MinLength(1, ErrorMessage = "Must have at least 1 correct answer")]
        public List<string> CorrectAnswers { get; set; }

        [Required(ErrorMessage = "Answer list is required")]
        [MinLength(2, ErrorMessage = "Must have at least 2 answers")]
        public List<string> StringAnswers { get; set; }
    }
}
