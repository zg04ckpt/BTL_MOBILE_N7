using Core.Models;
using Models.Quizzes.Enums;
using System.Text.Json.Serialization;

namespace Models.Quizzes.Requests
{
    public class SearchQuestionRequest : PagingRequest
    {
        public string? StringContent { get; set; }
        public int? TopicId { get; set; }

        [JsonConverter(typeof(string))]
        public QuestionType? Type { get; set; }

        [JsonConverter(typeof(string))]
        public QuestionLevel? Level { get; set; }

        [JsonConverter(typeof(string))]
        public QuestionStatus? Status { get; set; }
    }
}
