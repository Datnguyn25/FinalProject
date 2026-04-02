using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly WebDbContext _context;

        public AdminController(WebDbContext context)
        {
            _context = context;
        }

        // ================= DASHBOARD =================
        public IActionResult Index()
        {
            ViewBag.Users = _context.tb_Users.Count();
            ViewBag.Products = _context.tb_Product.Count();
            ViewBag.Orders = _context.tb_Order.Count();

            return View();
        }

        // ================= USER =================
        public IActionResult Users()
        {
            return View(_context.tb_Users.ToList());
        }

        public IActionResult LockUser(int id)
        {
            var user = _context.tb_Users.Find(id);

            user.IsActive = false;
            user.IsBanned = true;

            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        public IActionResult UnlockUser(int id)
        {
            var user = _context.tb_Users.Find(id);

            user.IsActive = true;
            user.IsBanned = false;

            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        // ================= SELLER =================
        public IActionResult Sellers()
        {
            var sellers = _context.tb_Users
                .Where(x => x.Id == 2)
                .ToList();

            return View(sellers);
        }

        public IActionResult ApproveSeller(int id)
        {
            var seller = _context.tb_Users.Find(id);

            seller.IsActive = true;
            seller.IsBanned = false;

            _context.SaveChanges();

            return RedirectToAction("Sellers");
        }

        public IActionResult BanSeller(int id)
        {
            var seller = _context.tb_Users.Find(id);

            seller.IsActive = false;
            seller.IsBanned = true;

            _context.SaveChanges();

            return RedirectToAction("Sellers");
        }

        // ================= PRODUCTS =================
        // ================= PRODUCTS =================
        public IActionResult Products()
        {
            var products = _context.tb_Product
                .Include(p => p.Category)
                .Include(p => p.Shop)
                .ToList();

            return View(products);
        }

        // ================= CREATE PRODUCT =================
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product model)
        {
            _context.tb_Product.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        // ================= EDIT PRODUCT =================
        public IActionResult EditProduct(int id)
        {
            var product = _context.tb_Product.Find(id);

            ViewBag.Categories = _context.tb_ProductCategory.ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product model)
        {
            _context.tb_Product.Update(model);
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        // ================= DELETE PRODUCT =================
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.tb_Product.Find(id);

            _context.tb_Product.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        // ================= ORDERS =================
        public IActionResult Orders()
        {
            var orders = _context.tb_Order
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ToList();

            return View(orders);
        }

        public IActionResult UpdateOrder(int id, string status)
        {
            var order = _context.tb_Order.Find(id);

            order.OrderStatus = status;

            if (status == "Completed")
            {
                order.Delivered = true;
                order.DeliveryDate = DateTime.Now;
            }

            _context.SaveChanges();

            return RedirectToAction("Orders");
        }

        // ================= CATEGORY =================
        public IActionResult Categories()
        {
            return View(_context.tb_ProductCategory.ToList());
        }

        [HttpPost]
        public IActionResult CreateCategory(ProductCategory cate)
        {
            _context.tb_ProductCategory.Add(cate);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        public IActionResult DeleteCategory(int id)
        {
            var cate = _context.tb_ProductCategory.Find(id);

            _context.tb_ProductCategory.Remove(cate);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        // ================= PROMOTION =================
        public IActionResult Promotions()
        {
            return View(_context.tb_Promotion.ToList());
        }

        [HttpPost]
        public IActionResult CreatePromotion(Promotion promo)
        {
            _context.tb_Promotion.Add(promo);
            _context.SaveChanges();

            return RedirectToAction("Promotions");
        }

        public IActionResult DeletePromotion(int id)
        {
            var promo = _context.tb_Promotion.Find(id);

            _context.tb_Promotion.Remove(promo);
            _context.SaveChanges();

            return RedirectToAction("Promotions");
        }

        // ================= REVENUE =================
        public IActionResult Revenue()
        {
            var orders = _context.tb_Order
                .Include(o => o.OrderDetails)
                .ToList();

            decimal total = 0;

            foreach (var order in orders)
            {
                foreach (var item in order.OrderDetails)
                {
                    total += item.Price * item.Quantity;
                }
            }

            ViewBag.TotalRevenue = total;

            return View();
        }

        // ================= SYSTEM =================
        public IActionResult Settings()
        {
            var setting = _context.tb_SystemSetting.FirstOrDefault();
            return View(setting);
        }

        [HttpPost]
        public IActionResult Settings(SystemSetting model)
        {
            _context.tb_SystemSetting.Update(model);
            _context.SaveChanges();

            return RedirectToAction("Settings");
        }
    }
}
