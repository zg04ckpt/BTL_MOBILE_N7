using Core.Models;

namespace Feature.Quizzes.Models
{
    public class SearchTopicRequest : PagingRequest
    {
        public string? Name { get; set; }
    }
}
