using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
