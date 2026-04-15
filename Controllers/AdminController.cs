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




    }


}