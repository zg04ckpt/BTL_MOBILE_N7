using System.ComponentModel.DataAnnotations;

namespace Models.Users.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Tên hiển thị là bắt buộc")]
        [RegularExpression(@"^[0-9A-Za-zÀ-ỹ ]{1,50}$", ErrorMessage = "Tên hiển thị dài 1-50 kí tự, không chứa kí tự đặc biệt")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [RegularExpression(@"^.{8,16}$", ErrorMessage = "Mật khẩu phải dài từ 8-16 kí tự")]
        public string Password { get; set; }
    }
}
