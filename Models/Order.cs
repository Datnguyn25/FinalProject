using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    [Table("tb_Order")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string OrderStatus { get; set; } = "Pending";

        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Unpaid";

        public bool Delivered { get; set; } = false;

        public DateTime? DeliveryDate { get; set; }

        public int CustomerID { get; set; }

        public int Discount { get; set; } = 0;

        // Navigation
        [ForeignKey("CustomerID")]
        public User Customer { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
