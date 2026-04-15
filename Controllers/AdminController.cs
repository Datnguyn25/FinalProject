using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class AdminController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(WebDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.Users = _context.tb_Users.Count();
            ViewBag.Products = _context.tb_Product.Count();
            ViewBag.Orders = _context.tb_Order.Count();
            return View();
        }

        private void WriteLog(string action, string details)
        {
            var userIdString = _userManager.GetUserId(User);
            if (userIdString == null) return;

            var log = new SystemLog
            {
                UserId = int.Parse(userIdString),
                Action = action,
                Details = details,
                Timestamp = DateTime.Now
            };
            _context.tb_SystemLog.Add(log);
            // Lưu ý: Không nên gọi SaveChanges ở đây nếu hàm gọi nó cũng gọi SaveChanges
        }

        // Tạo view để xem lịch sử Log
        public IActionResult Logs()
        {
            var logs = _context.tb_SystemLog.OrderByDescending(l => l.Timestamp).ToList();
            return View(logs);
        }
        public IActionResult Revenue()
        {
            var orders = _context.tb_Order.ToList();

            ViewBag.TotalRevenue = orders.Sum(o => o.TotalPrice);
            ViewBag.TotalOrders = orders.Count;

            // 📊 Revenue theo ngày
            var revenueByDate = orders
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new {
                    Date = g.Key.ToString("dd/MM"),
                    Total = g.Sum(x => x.TotalPrice)
                })
                .OrderBy(x => x.Date)
                .ToList();

            ViewBag.Dates = revenueByDate.Select(x => x.Date).ToList();
            ViewBag.Revenues = revenueByDate.Select(x => x.Total).ToList();

            // 🔥 TOP PRODUCTS (FIX LỖI CHO BẠN)
            var topProducts = _context.tb_OrderDetails
                .GroupBy(x => x.Product.ProductName)
                .Select(g => new
                {
                    Name = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(5)
                .ToList();

            ViewBag.TopProducts = topProducts;

            return View();
        }


    }


}