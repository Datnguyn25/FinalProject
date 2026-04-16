using FinalProject.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    [Table("tb_Product")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(250)]
        public string ProductName { get; set; }

        [StringLength(250)]
        public string SeoTitle { get; set; }

        public bool Status { get; set; } = true;

        [StringLength(255)]
        public string Image { get; set; }

        // XML trong SQL → map thành string
        public string ListImages { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PromotionPrice { get; set; } = 0;

        public bool VAT { get; set; }

        public int Quantity { get; set; } = 0;

        public bool Hot { get; set; }

        [StringLength(500)]
        public string ProductDescription { get; set; }

        // mô tả chi tiết (HTML)
        public string Detail { get; set; }

        public int ViewCount { get; set; } = 0;

        // ===== SEO =====
        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescription { get; set; }

        // ===== FOREIGN KEYS =====

        public int? CateID { get; set; }

        [ForeignKey("CateID")]
        public virtual ProductCategory Category { get; set; }

        public int? BrandID { get; set; }

        [ForeignKey("BrandID")]
        public virtual Brand Brand { get; set; }

        public int ShopID { get; set; }

        [ForeignKey("ShopID")]
        public virtual Shop Shop { get; set; }

        // ===== AUDIT =====

        public int? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
    }
}