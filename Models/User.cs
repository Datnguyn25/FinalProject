using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("tb_Users")]
    public class User : IdentityUser<int>
    {
        //[Key]
        //public int UserID { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string UserName { get; set; } = null!;

        //[Required]
        //[StringLength(100)]
        //[EmailAddress]
        //public string Email { get; set; } = null!;

        //[StringLength(255)]
        //public string? PasswordHash { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }

        //[StringLength(20)]
        //public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }
        [StringLength(100)]
        public string? District { get; set; }
        [StringLength(100)]
        public string? Ward { get; set; }
        [StringLength(100)]
        public string? Country { get; set; }

        public bool EmailVerified { get; set; } = false;
        public bool PhoneVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsBanned { get; set; } = false;

        public DateTime? LastLogin { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        //===== FOREIGN KEY =====
        public virtual Shop MyShop { get; set; }

        //[ForeignKey("RoleId")]
        //public virtual IdentityRole<int> Role { get; set; } = null!;
    }
}