using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    [Table("tb_Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        [StringLength(50)]
      

        // 🔥 Ngày tạo đơn (dùng cho thống kê)
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // 🧾 Ngày đặt hàng (hiển thị)
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime? PaymentDate { get; set; }

        [StringLength(50)]
        public string OrderStatus { get; set; } = "Pending";

        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Unpaid";
        [Required]
        [StringLength(10)]
        public string ReceiverPhone { get; set; }

        public bool Delivered { get; set; } = false;

        public DateTime? DeliveryDate { get; set; }

        [Required]
        [StringLength(250)]
        public string ShippingAddress { get; set; }
        [StringLength(250)]
        public string? CustomerName { get; set; }

        public int? UserId { get; set; }

        public int Discount { get; set; } = 0;

        // 🔥 Tổng tiền (QUAN TRỌNG cho Revenue)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } = 0;
        public int? PromotionId { get; set; }
        [ForeignKey("PromotionId")]
        public virtual Promotion Promotion { get; set; }

        public decimal DiscountAmount { get; set; } = 0;

        // Navigation
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        
    }
}