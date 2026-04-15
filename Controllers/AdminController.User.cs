using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public IActionResult Users() => View(_context.tb_Users.ToList());

        public IActionResult LockUser(int id)
        {
            var user = _context.tb_Users.Find(id);
            if (user == null) return NotFound();
            user.IsActive = false;
            user.IsBanned = true;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public IActionResult UnlockUser(int id)
        {
            var user = _context.tb_Users.Find(id);
            if (user == null) return NotFound();
            user.IsActive = true;
            user.IsBanned = false;
            _context.SaveChanges();
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> Sellers()
        {
            // Lọc theo RoleId (ví dụ 2 hoặc 3 tùy database của bạn) thay vì ID cá nhân
            var sellers = await _userManager.GetUsersInRoleAsync("Seller");
            return View(sellers.ToList());
        }

        public IActionResult ApproveSeller(int id)
        {
            var seller = _context.tb_Users.Find(id);
            if (seller == null) return NotFound();
            seller.IsActive = true;
            seller.IsBanned = false;
            _context.SaveChanges();
            return RedirectToAction("Sellers");
        }
    }
}