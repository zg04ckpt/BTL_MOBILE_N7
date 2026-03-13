using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Models.Users.Requests
{
    public class UpdateProfileRequest
    {
        [Required(ErrorMessage = "Tên hiển thị là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Tên hiển thị không được vượt quá 50 kí tự")]
        public string Name { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
