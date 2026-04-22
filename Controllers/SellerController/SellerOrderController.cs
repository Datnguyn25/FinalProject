using FinalProject.Data;
using FinalProject.Hubs;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers.SellerController
{
    [Authorize(Roles = "Shop")]
    public class SellerOrderController : Controller
    {
        private readonly WebDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHubContext<OrderHub> _hub;

        public SellerOrderController(
            WebDbContext context,
            UserManager<User> userManager,
            IHubContext<OrderHub> hub)
        {
            _context = context;
            _userManager = userManager;
            _hub = hub;
        }

        // ================= LẤY USER =================
        private int GetCurrentUserId()
        {
            var idStr = _userManager.GetUserId(User);
            return int.TryParse(idStr, out var id) ? id : 0;
        }

        // ================= LẤY SHOP =================
        private Shop GetSellerShop()
        {
            var userId = GetCurrentUserId();
            return _context.tb_Shop.FirstOrDefault(s => s.OwnerId == userId);
        }

        // ================= DANH SÁCH ĐƠN =================
        public IActionResult Index()
        {
            var shop = GetSellerShop();
            if (shop == null) return Content("Bạn chưa có shop");

            var orders = _context.tb_Order
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Where(o => o.OrderDetails.Any(od => od.Product.ShopId == shop.ShopId))
                .OrderByDescending(o => o.CreatedDate)
                .ToList();

            ViewBag.Shop = shop;
            return View(orders);
        }

        // ================= CHI TIẾT =================
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

            // 🔥 LỌC ITEM THEO SHOP
            var shopItems = order.OrderDetails
                .Where(od => od.Product.ShopId == shop.ShopId)
                .ToList();

            if (!shopItems.Any()) return Forbid();

            ViewBag.Shop = shop;
            ViewBag.ShopItems = shopItems;

            return View(order);
        }

        // ================= XÁC NHẬN ĐƠN =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var order = _context.tb_Order
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();

            if (!order.OrderDetails.Any(od => od.Product.ShopId == shop.ShopId))
                return Forbid();

            order.OrderStatus = "Processing";

            _context.tb_Order.Update(order);
            await _context.SaveChangesAsync();

            // 🔥 REALTIME
            await _hub.Clients.All.SendAsync("OrderUpdated", new
            {
                order.OrderId,
                order.OrderStatus
            });

            return RedirectToAction(nameof(Details), new { id });
        }

        // ================= UPDATE SHIPPING =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateShipping(int id, string status)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var order = _context.tb_Order
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();

            if (!order.OrderDetails.Any(od => od.Product.ShopId == shop.ShopId))
                return Forbid();

            order.OrderStatus = status ?? order.OrderStatus;

            if (status == "Completed")
            {
                order.Delivered = true;
                order.DeliveryDate = DateTime.Now;
            }

            _context.tb_Order.Update(order);
            await _context.SaveChangesAsync();

            // 🔥 REALTIME
            await _hub.Clients.All.SendAsync("OrderUpdated", new
            {
                order.OrderId,
                order.OrderStatus,
                order.Delivered
            });

            return RedirectToAction(nameof(Details), new { id });
        }

        // ================= HỦY ĐƠN (THÊM MỚI) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var shop = GetSellerShop();
            if (shop == null) return Forbid();

            var order = _context.tb_Order
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();

            if (!order.OrderDetails.Any(od => od.Product.ShopId == shop.ShopId))
                return Forbid();

            order.OrderStatus = "Cancelled";

            _context.tb_Order.Update(order);
            await _context.SaveChangesAsync();

            // 🔥 REALTIME
            await _hub.Clients.All.SendAsync("OrderUpdated", new
            {
                order.OrderId,
                order.OrderStatus
            });

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}