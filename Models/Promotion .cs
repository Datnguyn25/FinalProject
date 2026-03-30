using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("tb_Promotion")]
    public class Promotion
    {
        [Key]
        public int PromotionID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public int DiscountPercent { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Status { get; set; } = true;
    }
}