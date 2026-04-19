using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // ================== DANH SÁCH ĐƠN ==================
        public IActionResult Order()
        {
            var orders = _context.tb_Order
                .OrderByDescending(o => o.CreatedDate)
                .ToList();

            return View(orders);
        }

        // ================== CHI TIẾT ĐƠN ==================
        public IActionResult OrderDetails(int id)
        {
            var order = _context.tb_Order
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // ================== LOG ==================
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
        }

        public IActionResult Logs()
        {
            var logs = _context.tb_SystemLog
                .OrderByDescending(l => l.Timestamp)
                .ToList();

            return View(logs);
        }
    }
}