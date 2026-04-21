using Azure;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Models.Momo;
using FinalProject.Services.Momo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Helpers;
using Microsoft.AspNetCore.Identity;
public class CheckoutController : Controller
{
    private readonly IMomoService _momoService;
    private readonly WebDbContext _context;
    private readonly UserManager<User> _userManager;

    public CheckoutController(IMomoService momoService, WebDbContext context, UserManager<User> userManager)
    {
        _momoService = momoService;
        _context = context;
        _userManager = userManager;
    }

    // 👉 BẤM THANH TOÁN
    [HttpPost]
    public async Task<IActionResult> Checkout(string paymentMethod, string PromotionCode)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var cart = HttpContext.Session.Get<List<CartItems>>("Cart");
        if (cart == null || !cart.Any()) return RedirectToAction("Index", "Home");

        decimal originalTotal = 55000; // You should calculate this from Session/DB cart items
        decimal finalTotal = originalTotal;
        int discountPercent = 0;

        if (!string.IsNullOrEmpty(PromotionCode))
        {
            var now = DateTime.Now;
            var promo = _context.tb_Promotion.FirstOrDefault(p =>
                p.PromotionCode == PromotionCode &&
                p.Status == true &&
                now >= p.StartDate &&
                now <= p.EndDate);

            if (promo != null)
            {
                discountPercent = promo.DiscountPercent;
                finalTotal = originalTotal * (100 - discountPercent) / 100;
            }
        }



        // 1. Tạo đơn hàng
        var order = new Order
        {
            UserId = 1, // Should get from User.Identity
            OrderStatus = "Pending",
            PaymentStatus = "Unpaid",
            OrderDate = DateTime.Now,
            TotalPrice = finalTotal, // Store the final price!
            ShippingAddress = $"{user.Address}, {user.Ward}, {user.District}, {user.City}",
            ReceiverPhone = user.PhoneNumber,
            OrderDetails = cart.Select(item => new OrderDetails
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
                Size = item.Size
            }).ToList()
        };

        _context.tb_Order.Add(order);
        await _context.SaveChangesAsync();
        LoggerHelper.WriteLog(_context, User, $"User {User.Identity.Name} initiated checkout for order {order.OrderId} via {paymentMethod}. Total: {finalTotal} (Discount: {discountPercent}%)");

        // 2. COD → xong luôn
        if (paymentMethod == "COD")
        {
            order.OrderStatus = "Completed";
            order.PaymentStatus = "Paid";
            await _context.SaveChangesAsync();

            LoggerHelper.WriteLog(_context, User, $"User {User.Identity.Name} completed COD Order {order.OrderId}. Total: {finalTotal} (Discount: {discountPercent}%)");
            return RedirectToAction("Result", "Checkout", new { id = order.OrderId });
        }

        // 3. MOMO
        var momoModel = new OrderInfoModel
        {
            FullName = user.FullName,
            Amount = (double)finalTotal,

            // 🔥 FIX Ở ĐÂY
            OrderInfo = "Order Payment " + order.OrderId + (discountPercent > 0 ? $" (Discount {discountPercent}%)" : "")
        };
        var response = await _momoService.CreatePaymentMomo(momoModel);

        if (response != null && !string.IsNullOrEmpty(response.PayUrl))
        {
            // Đây là lệnh quan trọng để mở trang quét mã QR MoMo
            return Redirect(response.PayUrl);
        }

        // 👉 in ra để xem lỗi thật
        return Content(
            "Message: " + response.Message +
            "\nPayUrl: " + response.PayUrl
        );
    }

    // 👉 MOMO TRẢ VỀ
    public async Task<IActionResult> MomoReturn()
    {
        try
        {
            var query = HttpContext.Request.Query;

            var orderInfo = query["orderInfo"].ToString();
            var resultCode = query["resultCode"].ToString();

            var orderIdFromMomo = query["orderId"].ToString();
            // 🔥 FIX: không parse bừa nữa
            if (int.TryParse(orderIdFromMomo, out int orderId))
            {
                var order = _context.tb_Order
                    .Include(o => o.OrderDetails) // OrderDetails
                    .ThenInclude(d => d.Product)
                    .FirstOrDefault(o => o.OrderId == orderId);

                if (order != null)
                {
                    if (resultCode == "0")
                    {
                        LoggerHelper.WriteLog(_context, User, $"User {User.Identity.Name} completed MoMo Order {order.OrderId}. Total: {order.TotalPrice} (OrderInfo: {orderInfo})");

                        order.OrderStatus = "Completed";
                        order.PaymentStatus = "Paid";
                        ViewBag.Message = "Transaction Successful";
                    }
                    else
                    {
                        LoggerHelper.WriteLog(_context, User, $"User {User.Identity.Name} failed MoMo Order {order.OrderId}. Total: {order.TotalPrice} (OrderInfo: {orderInfo}, ResultCode: {resultCode})");
                        order.OrderStatus = "Cancelled";
                        order.PaymentStatus = "Failed";
                        ViewBag.Message = "Transaction Failed";
                    }

                    await _context.SaveChangesAsync();
                    return View("~/Views/Cart/Invoice.cshtml", order);
                }
            }

            return View("~/Views/Cart/Invoice.cshtml");
        }
        catch (Exception ex)
        {
            return Content("ERROR: " + ex.Message);
        }
    }
    public IActionResult Result(int id)
    {
        var order = _context.tb_Order
            .Include(o => o.OrderDetails)
            .ThenInclude(d => d.Product)
            .FirstOrDefault(o => o.OrderId == id);
        return View("~/Views/Cart/Invoice.cshtml", order);
    }
}