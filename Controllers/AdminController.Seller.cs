using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public async Task<IActionResult> Sellers()
        {
            // 1. Get IDs of users who have the "Seller" role
            var sellerIds = (await _userManager.GetUsersInRoleAsync("Shop"))
                            .Select(u => u.Id);

            // 2. Query the context directly to use .Include(u => u.MyShop)
            var sellers = await _context.Users
                .Include(u => u.MyShop) // 🔥 THIS IS THE PART I MENTIONED
                .Where(u => sellerIds.Contains(u.Id))
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(sellers);
        }

        [HttpPost] // Should be Post for data changes
        public IActionResult ApproveSeller(int id)
        {
            // Include MyShop so we can verify the shop profile too
            var user = _context.Users
                .Include(u => u.MyShop)
                .FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound();

            // Activate the user account
            user.IsActive = true;
            user.IsBanned = false;

            // Also verify the shop if it exists
            if (user.MyShop != null)
            {
                user.MyShop.IsVerified = true;
                user.MyShop.IsActive = true;
            }

            string targetName = user.MyShop?.ShopName ?? user.FullName;
            WriteLog($" approved seller: {targetName}");
            _context.SaveChanges();
            return RedirectToAction("Sellers");
        }

        [HttpPost]
        public IActionResult BanSeller(int id)
        {
            var user = _context.Users
            .Include(u => u.MyShop)
            .FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound();

            user.IsBanned = true;
            user.IsActive = false;

            string targetName = user.MyShop?.ShopName ?? user.FullName;
            WriteLog($"banned seller: {targetName}");
            _context.SaveChanges();

            return RedirectToAction("Sellers");
        }

        [HttpPost]
        public IActionResult UnbanSeller(int id)
        {
            var user = _context.Users
                .Include(u => u.MyShop)
                .FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound();

            user.IsBanned = false;
            user.IsActive = true; // Mở băng thì kích hoạt lại luôn

            string targetName = user.MyShop?.ShopName ?? user.FullName;
            WriteLog($"unbanned seller: {targetName}");

            _context.SaveChanges();
            return RedirectToAction("Sellers");
        }
    }
}