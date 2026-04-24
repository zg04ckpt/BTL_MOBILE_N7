using System.ComponentModel.DataAnnotations;

namespace Models.Users.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Display name is required")]
        [RegularExpression(@"^[0-9A-Za-zÀ-ỹ ]{1,50}$", ErrorMessage = "Display name must be 1-50 characters and contain no special characters")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{8,16}$", ErrorMessage = "Password must be 8-16 characters long")]
        public string Password { get; set; }
    }
}
