using Models.Quizzes.Enums;
using System.Text.Json.Serialization;

namespace Models.Quizzes.DTOs
{
    public class QuestionDetailDto
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string StringContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? AudioUrl { get; set; }
        public string? VideoUrl { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionLevel Level { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionStatus Status { get; set; }
        
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> CorrectAnswers { get; set; }
        public List<string> StringAnswers { get; set; }
    }
}

