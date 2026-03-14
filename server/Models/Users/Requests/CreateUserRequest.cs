using System.ComponentModel.DataAnnotations;

namespace Models.Users.Requests
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(255, ErrorMessage = "Email must not exceed 255 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [MaxLength(15, ErrorMessage = "Phone number must not exceed 15 characters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^.{8,16}$", ErrorMessage = "Password must be 8-16 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Role ID must be greater than 0")]
        public int RoleId { get; set; }
    }
}
