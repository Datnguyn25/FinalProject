using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("Roles")]
    public class Roles
    {
        [Key] // Primary Key
        public int RoleId { get; set; }

        [Required] // Required field
        [StringLength(50)]
        public string RoleName { get; set; }

        // Quan hệ với Customer, Shop
        public ICollection<User> User { get; set; }
        public ICollection<Shop> Shops { get; set; }
    }
}
