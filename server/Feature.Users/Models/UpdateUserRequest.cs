using Feature.Users.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Feature.Users.Models
{
    public class UpdateUserRequest
    {
        public IFormFile? Avatar { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 kí tự")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Tên không được vượt quá 50 kí tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [MaxLength(255, ErrorMessage = "Email không được vượt quá 255 kí tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Trạng thái là bắt buộc")]
        public AccountStatus Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Level phải lớn hơn 0")]
        public int Level { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Rank không được âm")]
        public int Rank { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "RankScore không được âm")]
        public int RankScore { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Exp không được âm")]
        public int Exp { get; set; }

        [Required(ErrorMessage = "Role ID là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Role ID phải lớn hơn 0")]
        public int RoleId { get; set; }
    }
}
