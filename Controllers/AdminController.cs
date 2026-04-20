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
    }
}