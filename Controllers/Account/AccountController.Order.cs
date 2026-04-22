using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AccountController
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ConfirmReceived(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);

            // Tìm đơn hàng của đúng user đó
            var order = await _context.tb_Order
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == user.Id);

            if (order == null) return NotFound();

            // Chỉ cho phép xác nhận nếu đơn hàng chưa hoàn thành hoặc chưa bị hủy
            if (order.OrderStatus == "Processing")
            {
                order.OrderStatus = "Completed";
                order.PaymentStatus = "Paid"; // Đảm bảo đã thanh toán nếu là COD

                await _context.SaveChangesAsync();

                // Lưu log hành động
                LoggerHelper.WriteLog(_context, User, $"confirmed receipt for Order #{order.OrderId}");

                TempData["SuccessMsg"] = "Cofirmed order received successfully";
            }

            return RedirectToAction("Profile");
        }

        [Authorize]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var orders = await _context.tb_Order
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderHistoryVM
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    OrderStatus = o.OrderStatus,
                    PaymentStatus = o.PaymentStatus
                }).ToListAsync();

            return View(orders);
        }

        // 1. Hủy đơn hàng (Chỉ dành cho đơn đang Pending)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _context.tb_Order
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == user.Id);

            if (order == null) return NotFound();

            // Chỉ cho phép hủy khi đơn hàng chưa được xử lý (Pending)
            if (order.OrderStatus == "Pending")
            {
                order.OrderStatus = "Cancelled";
                order.PaymentStatus = "Failed"; // Hoặc "Refunded" nếu là MoMo
                await _context.SaveChangesAsync();

                LoggerHelper.WriteLog(_context, User, $"cancelled Order #{order.OrderId}");
                TempData["SuccessMsg"] = "Order cancelled.";
            }
            else
            {
                TempData["ErrorMsg"] = "A processing or delivering order cannot be cancelled.";
            }

            return RedirectToAction("Profile");
        }

        // 2. Xác nhận đã nhận hàng (Giữ lại hàm cũ nhưng sửa Redirect về Profile)
        
    }
}
