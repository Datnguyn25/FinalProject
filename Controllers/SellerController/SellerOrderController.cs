using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers.SellerController
{
    [Authorize(Roles = "Shop")]
    public class SellerOrderController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<User> _userManager;

        public SellerOrderController(WebDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        // GET: SellerOrder
        public IActionResult Index()
        {
            var shop = GetSellerShop();
            if (shop == null) return View(new List<Order>());

            // Orders that contain at least one OrderDetails entry for this shop
            var orders = _context.tb_Order
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.OrderDetails.Any(od => od.ShopId == shop.ShopId))
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            ViewBag.Shop = shop;
            return View(orders);
        }

        // GET: SellerOrder/Details/5
        public IActionResult Details(int id)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var order = _context.tb_Order
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();

            // ensure order includes items for this shop
            var shopItems = order.OrderDetails.Where(od => od.ShopId == shop.ShopId).ToList();
            if (!shopItems.Any()) return Forbid();

            // attach only shop-specific items to ViewBag for display
            ViewBag.Shop = shop;
            ViewBag.ShopItems = shopItems;

            return View(order);
        }

        // POST: SellerOrder/ConfirmOrder/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmOrder(int id)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var order = _context.tb_Order
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();

            if (!order.OrderDetails.Any(od => od.ShopId == shop.ShopId)) return Forbid();
            order.OrderStatus = "Confirmed";
            _context.tb_Order.Update(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: SellerOrder/UpdateShipping/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateShipping(int id, string status)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var order = _context.tb_Order
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();
            if (!order.OrderDetails.Any(od => od.ShopId == shop.ShopId)) return Forbid();

            // Allowed statuses: Processed, Delivering, Completed (or others if needed)
            order.OrderStatus = status ?? order.OrderStatus;

            if (string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase))
            {
                order.Delivered = true;
                order.DeliveryDate = DateTime.Now;
            }

            _context.tb_Order.Update(order);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}