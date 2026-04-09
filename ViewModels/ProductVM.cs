namespace FinalProject.ViewModels
{
    public class ProductVM
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; } // Dùng để upload ảnh mới
        public int CategoryID { get; set; }
    }
}
