using Microsoft.AspNetCore.Http;
using Models.Users.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.Users.Requests
{
    public class UpdateUserRequest
    {
        public IFormFile? Avatar { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [MaxLength(15, ErrorMessage = "Phone number must not exceed 15 characters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(255, ErrorMessage = "Email must not exceed 255 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccountStatus Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Level must be greater than 0")]
        public int Level { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Rank must be non-negative")]
        public int Rank { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "RankScore must be non-negative")]
        public int RankScore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Exp must be non-negative")]
        public int Exp { get; set; }

        [Required(ErrorMessage = "Role ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Role ID must be greater than 0")]
        public int RoleId { get; set; }
    }
}
