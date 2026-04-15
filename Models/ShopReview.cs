using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("tb_ShopReview")]
    public class ShopReview
    {
        [Key]
        public int ReviewId { get; set; }

        public int ShopId { get; set; }

        [ForeignKey("ShopId")]
        public Shop Shop { get; set; }

        [StringLength(100)]
        public string ReviewerName { get; set; }

        [Range(1,5)]
        public int Rating { get; set; } = 5;

        [StringLength(1000)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // optional: whether review is approved by admin
        public bool IsApproved { get; set; } = true;
    }
}