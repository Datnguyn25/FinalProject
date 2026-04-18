using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    [Table("tb_ProductCategory")]
    public class ProductCategory
    {
        [Key]
        public int CateId { get; set; }

        [StringLength(250)]
        public string CateName { get; set; }

        [StringLength(250)]
        public string SeoTitle { get; set; }

        public bool Statuss { get; set; } = true;

        public int? Sort { get; set; }

        // Self reference (Danh mục cha)
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public ProductCategory ParentCategory { get; set; }

        public ICollection<ProductCategory> ChildCategories { get; set; }

        [StringLength(50)]
        public string MetaKeywords { get; set; }

        [StringLength(50)]
        public string MetaDescription { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        // Relationship with Product
        public ICollection<Product> Products { get; set; }
    }


}
