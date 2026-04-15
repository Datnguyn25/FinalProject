using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Controllers.SellerController
{
    [Authorize(Roles = "Shop")]
    public class SellerProductController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public SellerProductController(WebDbContext context, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // Helper: get current user's numeric id
        private int GetCurrentUserId()
        {
            var idStr = _userManager.GetUserId(User); // returns string
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        // Helper: load the shop owned by current user
        private Shop GetSellerShop()
        {
            var userId = GetCurrentUserId();
            return _context.tb_Shop.FirstOrDefault(s => s.OwnerId == userId);
        }

        // LIST seller's products
        public IActionResult Index()
        {
            var shop = GetSellerShop();
            if (shop == null)
                return View(new List<Product>());

            var products = _context.tb_Product
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.ShopID == shop.ShopID)
                .ToList();

            ViewBag.Shop = shop;
            return View(products);
        }

        // DETAILS
        public IActionResult Details(int id)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.ProductID == id && p.ShopID == shop.ShopID);

            if (product == null) return NotFound();
            return View(product);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            ViewBag.Brands = _context.tb_Brand.ToList();
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model, IFormFile imageFile)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            if (ModelState.IsValid)
            {
                // handle image upload (optional)
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads", "shops", shop.ShopID.ToString());
                    Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using var fs = new FileStream(filePath, FileMode.Create);
                    imageFile.CopyTo(fs);
                    model.Image = Path.Combine("uploads", "shops", shop.ShopID.ToString(), fileName).Replace('\\', '/');
                }

                model.ShopID = shop.ShopID;
                model.CreatedBy = GetCurrentUserId();
                model.CreatedDate = DateTime.Now;

                _context.tb_Product.Add(model);
                _context.SaveChanges();

                // update shop product count
                shop.TotalProducts = _context.tb_Product.Count(p => p.ShopID == shop.ShopID);
                _context.tb_Shop.Update(shop);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            ViewBag.Brands = _context.tb_Brand.ToList();
            return View(model);
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == id && p.ShopID == shop.ShopID);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            ViewBag.Brands = _context.tb_Brand.ToList();
            return View(product);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product model, IFormFile imageFile)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var existing = _context.tb_Product.FirstOrDefault(p => p.ProductID == model.ProductID && p.ShopID == shop.ShopID);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                // update image if provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads", "shops", shop.ShopID.ToString());
                    Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using var fs = new FileStream(filePath, FileMode.Create);
                    imageFile.CopyTo(fs);
                    existing.Image = Path.Combine("uploads", "shops", shop.ShopID.ToString(), fileName).Replace('\\', '/');
                }

                // update allowed fields (name, desc, detail, seo, brand, category, status)
                existing.ProductName = model.ProductName;
                existing.SeoTitle = model.SeoTitle;
                existing.ProductDescription = model.ProductDescription;
                existing.Detail = model.Detail;
                existing.MetaKeywords = model.MetaKeywords;
                existing.MetaDescription = model.MetaDescription;
                existing.BrandID = model.BrandID;
                existing.CateID = model.CateID;
                existing.Status = model.Status;
                existing.Hot = model.Hot;
                existing.UpdatedBy = GetCurrentUserId();
                existing.UpdatedDate = DateTime.Now;

                _context.tb_Product.Update(existing);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            ViewBag.Brands = _context.tb_Brand.ToList();
            return View(model);
        }

        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == id && p.ShopID == shop.ShopID);
            if (product == null) return NotFound();

            _context.tb_Product.Remove(product);
            _context.SaveChanges();

            // update shop product count
            shop.TotalProducts = _context.tb_Product.Count(p => p.ShopID == shop.ShopID);
            _context.tb_Shop.Update(shop);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // UPDATE PRICE (POST) - model binding with price fields
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePrice(int id, decimal price, decimal promotionPrice)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == id && p.ShopID == shop.ShopID);
            if (product == null) return NotFound();

            product.Price = price;
            product.PromotionPrice = promotionPrice;
            product.UpdatedBy = GetCurrentUserId();
            product.UpdatedDate = DateTime.Now;

            _context.tb_Product.Update(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id });
        }

        // UPDATE INVENTORY (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateInventory(int id, int quantity)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == id && p.ShopID == shop.ShopID);
            if (product == null) return NotFound();

            product.Quantity = quantity;
            product.UpdatedBy = GetCurrentUserId();
            product.UpdatedDate = DateTime.Now;

            _context.tb_Product.Update(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id });
        }

        // Small utility to update list images (comma separated paths)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateListImages(int id, string listImages)
        {
            var shop = GetSellerShop();
            var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == id && p.ShopID == shop.ShopID);
            if (product == null) return NotFound();

            product.ListImages = listImages; // expects "img1.jpg,img2.jpg"
            product.UpdatedBy = GetCurrentUserId();
            product.UpdatedDate = DateTime.Now;

            _context.tb_Product.Update(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id });
        }
    }
}