using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("tb_Shop")]
    public class Shop
    {
        [Key]
        public int ShopID { get; set; }

        [StringLength(50)]
        public string ShopName { get; set; }

        [StringLength(255)]
        public string? LogoUrl { get; set; }

        // SQL là XML → map thành string
        public string CoverImageUrl { get; set; }

        public string ShopDescription { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [StringLength(20)]
        public string ContactPhone { get; set; }

        [StringLength(255)]
        public string ShopAddress { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        public int TotalProducts { get; set; } = 0;
        public int TotalFollowers { get; set; } = 0;

        [Column(TypeName = "decimal(3,2)")]
        public decimal RatingAverage { get; set; } = 0;

        public int TotalReviews { get; set; } = 0;
        public int TotalSold { get; set; } = 0;

        public bool IsVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsBanned { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }  // nullable

        // ===== FOREIGN KEY =====
        public int? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual IdentityRole<int> Role { get; set; }

        // NEW: link shop to the owning User (seller)
        public int? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
    }
}
