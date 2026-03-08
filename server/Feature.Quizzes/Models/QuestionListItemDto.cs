using Feature.Quizzes.Enums;
using System.Text.Json.Serialization;

namespace Feature.Quizzes.Models
{
    public class QuestionListItemDto
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string StringContent { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionLevel Level { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionStatus Status { get; set; }
        
        public string TopicName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
