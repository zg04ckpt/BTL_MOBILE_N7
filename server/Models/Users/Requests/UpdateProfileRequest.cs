using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Models.Users.Requests
{
    public class UpdateProfileRequest
    {
        [Required(ErrorMessage = "Display name is required")]
        [MaxLength(50, ErrorMessage = "Display name must not exceed 50 characters")]
        public string Name { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
