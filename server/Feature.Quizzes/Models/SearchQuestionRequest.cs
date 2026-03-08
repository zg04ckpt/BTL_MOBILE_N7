using Core.Models;

namespace Feature.Quizzes.Models
{
    public class SearchQuestionRequest : PagingRequest
    {
        public string? StringContent { get; set; }
        public int? TopicId { get; set; }
        public string? Type { get; set; }
        public string? Level { get; set; }
        public string? Status { get; set; }
    }
}
