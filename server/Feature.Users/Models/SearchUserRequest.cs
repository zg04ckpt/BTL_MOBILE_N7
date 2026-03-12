using Core.Models;

namespace Feature.Users.Models
{
    public class SearchUserRequest : PagingRequest
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
    }
}
