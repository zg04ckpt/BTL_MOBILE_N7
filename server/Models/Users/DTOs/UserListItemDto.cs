using Models.Users.Enums;
using System.Text.Json.Serialization;

namespace Models.Users.DTOs
{
    public class UserListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountStatus Status { get; set; }

        public string RoleName { get; set; }
    }
}
