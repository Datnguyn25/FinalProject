using System.Diagnostics;
using FinalProject.Models;
using FinalProject.Data;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebDbContext _context;

        // Inject thêm WebDbContext
        public HomeController(ILogger<HomeController> logger, WebDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy các sản phẩm đang giảm giá
            var saleProducts = _context.tb_Product
                .Include(p => p.Category)
                .Where(p => p.Status &&
                            p.PromotionPrice > 0 &&
                            p.PromotionPrice < p.Price)
                .ToList();

            // Gán dữ liệu vào ViewModel
            var model = new HomeViewModel
            {
                // Thời trang nam
                MensProducts = saleProducts
                    .Where(p => p.CateID == 1)
                    .Take(6)
                    .ToList(),

                // Thời trang nữ
                WomensProducts = saleProducts
                    .Where(p => p.CateID == 2)
                    .Take(6)
                    .ToList(),

                
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}