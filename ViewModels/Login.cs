using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "Username/Email is required")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
