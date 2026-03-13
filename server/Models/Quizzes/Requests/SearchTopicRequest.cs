using Core.Models;

namespace Models.Quizzes.Requests
{
    public class SearchTopicRequest : PagingRequest
    {
        public string? Name { get; set; }
    }
}
