using Core.Models;

namespace Feature.Quizzes.Models.Requests
{
    public class SearchTopicRequest : PagingRequest
    {
        public string? Name { get; set; }
    }
}
