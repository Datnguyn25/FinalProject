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
            var now = DateTime.Now;

            // Lấy các sản phẩm có chương trình khuyến mãi đang hoạt động
            var productsWithPromotion = _context.tb_Product
                .Include(p => p.Category)
                .Include(p => p.Promotion) // Phải Include mới lấy được data từ bảng Promotion
                .Where(p => p.Status &&
                            p.PromotionId != null && // Có gắn mã giảm giá
                            p.Promotion.Status &&     // Mã giảm giá còn hoạt động
                            p.Promotion.StartDate <= now && // Đã bắt đầu
                            p.Promotion.EndDate >= now)     // Chưa kết thúc
                .ToList();

            // Gán dữ liệu vào ViewModel
            var model = new HomeViewModel
            {
                // Thời trang nam (CateID == 1)
                MensProducts = productsWithPromotion
                    .Where(p => p.CateId == 1)
                    .Take(6)
                    .ToList(),

                // Thời trang nữ (CateID == 2)
                WomensProducts = productsWithPromotion
                    .Where(p => p.CateId == 2)
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