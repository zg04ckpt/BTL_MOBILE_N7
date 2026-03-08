using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Feature.Users.Models
{
    public class UpdateProfileRequest
    {
        [Required(ErrorMessage = "Tên hi?n th? là b?t bu?c")]
        [MaxLength(50, ErrorMessage = "Tên hi?n th? không ???c v??t quá 50 kí t?")]
        public string Name { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
