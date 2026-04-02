using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers.SellerController
{
    [Authorize(Roles = "Shop")]
    public class SellerShopController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public SellerShopController(WebDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        private int GetCurrentUserId()
        {
            var idStr = _userManager.GetUserId(User);
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        private Shop GetSellerShop()
        {
            var userId = GetCurrentUserId();
            return _context.tb_Shop.FirstOrDefault(s => s.OwnerId == userId);
        }

        // GET: SellerShop/Index
        public IActionResult Index()
        {
            var shop = GetSellerShop();
            if (shop == null) return View(null);
            return View(shop);
        }

        // GET: SellerShop/Edit
        public IActionResult Edit()
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();
            return View(shop);
        }

        // POST: SellerShop/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Shop model, Microsoft.AspNetCore.Http.IFormFile logoFile)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            if (ModelState.IsValid)
            {
                // handle logo upload
                if (logoFile != null && logoFile.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads", "shops", shop.ShopID.ToString());
                    Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using var fs = new FileStream(filePath, FileMode.Create);
                    logoFile.CopyTo(fs);
                    shop.LogoUrl = Path.Combine("uploads", "shops", shop.ShopID.ToString(), fileName).Replace('\\', '/');
                }

                // update fields allowed for seller
                shop.ShopName = model.ShopName;
                shop.ShopDescription = model.ShopDescription;
                shop.ContactEmail = model.ContactEmail;
                shop.ContactPhone = model.ContactPhone;
                shop.ShopAddress = model.ShopAddress;
                shop.City = model.City;
                shop.Country = model.Country;
                shop.UpdatedAt = DateTime.Now;

                _context.tb_Shop.Update(shop);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: SellerShop/Reviews
        public IActionResult Reviews()
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var reviews = _context.tb_ShopReview
                .Where(r => r.ShopId == shop.ShopID && r.IsApproved)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            ViewBag.Shop = shop;
            return View(reviews);
        }

        // GET: SellerShop/ReviewDetails/5
        public IActionResult ReviewDetails(int id)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var review = _context.tb_ShopReview
                .Include(r => r.Shop)
                .FirstOrDefault(r => r.ReviewId == id && r.ShopId == shop.ShopID);

            if (review == null) return NotFound();

            return View(review);
        }
    }
}