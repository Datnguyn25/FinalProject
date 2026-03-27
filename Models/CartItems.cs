using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    [Table("tb_CartItems")]
    public class CartItems
    {
        [Key]
        public int CartItemId { get; set; }

        // Foreign Keys
        public int CartId { get; set; }
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        // Navigation Properties
        [ForeignKey("CartId")]
        public Carts Cart { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }

        // Product properties
        public string ProductName { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; } // Add this property to fix CS1061
    }
}
