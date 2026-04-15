using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class Register
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "This phone number is mandatory due to security purposes")]
        [RegularExpression(@"^(0|84)[3|5|7|8|9][0-9]{8}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = null!;
    }
}