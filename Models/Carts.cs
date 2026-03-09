using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    [Table("tb_Carts")]
    public class Carts
    {
        [Key]
        public int CartId { get; set; }

        // Foreign Key
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public User Customer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Quan hệ với CartItem (sẽ tạo sau)
        public ICollection<CartItems> CartItems { get; set; }
    }
}
