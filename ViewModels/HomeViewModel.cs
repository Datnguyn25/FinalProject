using FinalProject.Models;
using System.Collections.Generic;

namespace FinalProject.ViewModels
{
    public class HomeViewModel
    {
       
        // Sản phẩm thời trang nam đang giảm giá
        public List<Product> MensProducts { get; set; } = new List<Product>();

        // Sản phẩm thời trang nữ đang giảm giá
        public List<Product> WomensProducts { get; set; } = new List<Product>();
        public List<Product> HotProducts { get; set; }
        public List<Product> SaleProducts { get; set; }


    }
}