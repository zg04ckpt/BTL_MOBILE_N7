using Feature.Quizzes.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Feature.Quizzes.Models
{
    public class UpdateQuestionRequest
    {
        [Required(ErrorMessage = "N?i dung câu h?i là b?t bu?c")]
        [MaxLength(1000, ErrorMessage = "N?i dung không ???c v??t quá 1000 ký t?")]
        public string StringContent { get; set; }

        public IFormFile? Image { get; set; }

        public IFormFile? Audio { get; set; }

        public IFormFile? Video { get; set; }

        [Required(ErrorMessage = "Lo?i câu h?i là b?t bu?c")]
        public QuestionType Type { get; set; }

        [Required(ErrorMessage = "?? khó là b?t bu?c")]
        public QuestionLevel Level { get; set; }

        [Required(ErrorMessage = "Tr?ng thái là b?t bu?c")]
        public QuestionStatus Status { get; set; }

        [Required(ErrorMessage = "Topic ID là b?t bu?c")]
        [Range(1, int.MaxValue, ErrorMessage = "Topic ID ph?i l?n h?n 0")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "Câu tr? l?i ?úng là b?t bu?c")]
        [MinLength(1, ErrorMessage = "Ph?i có ít nh?t 1 câu tr? l?i ?úng")]
        public List<string> CorrectAnswers { get; set; }

        [Required(ErrorMessage = "Danh sách ?áp án là b?t bu?c")]
        [MinLength(2, ErrorMessage = "Ph?i có ít nh?t 2 ?áp án")]
        public List<string> StringAnswers { get; set; }
    }
}
