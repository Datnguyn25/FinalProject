using FinalProject.Data;
using FinalProject.Hubs;
using FinalProject.Models;
using FinalProject.Services.Momo;
using FinalProject.Services.Zalo;
using FinalProject.Helpers;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

public class CartController : Controller
{
    private readonly WebDbContext _context;
    private readonly IMomoService _momoService;
    private readonly IZaloPayService _zaloPayService;
    private readonly IHubContext<OrderHub> _hub;

    public CartController(
        WebDbContext context,
        IMomoService momoService,
        IZaloPayService zaloPayService,
        IHubContext<OrderHub> hub)
    {
        _context = context;
        _momoService = momoService;
        _zaloPayService = zaloPayService;
        _hub = hub;
    }

    // ================= CART =================
    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(int productId, int quantity = 1, string actionType = "add", string size = "M")
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var product = _context.tb_Product.FirstOrDefault(p => p.ProductId == productId);
        if (product == null) return NotFound();

        var cart = GetCart();

        var item = cart.FirstOrDefault(x => x.ProductId == productId && x.Size == size);

        if (item == null)
        {
            cart.Add(new CartItems
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Image = product.Image,
                Price = product.Price,
                Quantity = quantity,
                Size = size
            });
        }
        else
        {
            item.Quantity += quantity;
        }

        SaveCart(cart);
        LoggerHelper.WriteLog(_context, User, $"Added {quantity} product(s) '{product.ProductName}' (Size: {size}) to the cart");

        if (actionType == "buyNow")
        {
            return RedirectToAction("Checkout");
        }

        return RedirectToAction("Index");
    }

    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id);

        if (item != null)
        {
            LoggerHelper.WriteLog(_context, User, $"Removed product '{item.ProductName}' (Size: {item.Size}) from the cart");
            cart.Remove(item);
        }

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    private List<CartItems> GetCart()
    {
        var session = HttpContext.Session.GetString("Cart");
        if (string.IsNullOrEmpty(session)) return new List<CartItems>();
        return JsonConvert.DeserializeObject<List<CartItems>>(session);
    }

    private void SaveCart(List<CartItems> cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }

    // ================= CHECKOUT =================
    [HttpGet]
    public IActionResult Checkout()
    {
        var cart = GetCart();
        if (cart.Count == 0) return RedirectToAction("Index", "Product");

        ViewBag.Cart = cart;
        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutVM model)
    {
        var cart = GetCart();

        if (cart.Count == 0)
            return RedirectToAction("Index", "Product");

        double totalAmount = (double)cart.Sum(x => (double)x.Price * x.Quantity);

        // ================= MOMO =================
        if (model.PaymentMethod?.ToLower() == "momo")
        {
            HttpContext.Session.SetString("CartBackup", JsonConvert.SerializeObject(cart));

            var orderInfo = new OrderInfoModel
            {
                FullName = model.FullName,
                Amount = totalAmount,
                OrderInfo = "Thanh toán đơn hàng",
                OrderId = DateTime.Now.Ticks.ToString()
            };

            var response = await _momoService.CreatePaymentMomo(orderInfo);

            if (response == null || string.IsNullOrEmpty(response.PayUrl))
            {
                return Content("Lỗi MoMo: không lấy được link thanh toán");
            }

            return Redirect(response.PayUrl);
        }

        // ================= COD =================
        return await SaveOrder(cart, "COD", "Unpaid");
    }

    // ================= MOMO CALLBACK =================
    [AllowAnonymous]
    [HttpGet]
    [Route("Checkout/PaymentCallBack")]
    public async Task<IActionResult> PaymentCallBack()
    {
        var resultCode = HttpContext.Request.Query["resultCode"].ToString();

        if (resultCode != "0")
        {
            return Content("Thanh toán thất bại");
        }

        var session = HttpContext.Session.GetString("CartBackup");

        if (string.IsNullOrEmpty(session))
        {
            return Content("Mất session giỏ hàng");
        }

        var cart = JsonConvert.DeserializeObject<List<CartItems>>(session);

        return await SaveOrder(cart, "MoMo", "Paid");
    }

    // ================= SAVE ORDER (CHUNG) =================
    private async Task<IActionResult> SaveOrder(List<CartItems> cart, string method, string paymentStatus)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var order = new Order
        {
            UserId = userId,
            CreatedDate = DateTime.Now,
            OrderDate = DateTime.Now,
            OrderStatus = "Pending",
            PaymentStatus = paymentStatus,
            TotalPrice = cart.Sum(x => x.Price * x.Quantity),

            // 🔥 FIX NULL ERROR
            ReceiverPhone = "0123456789",
            ShippingAddress = "Chưa cập nhật",
            CustomerName = User.Identity.Name
        };

        _context.tb_Order.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in cart)
        {
            _context.tb_OrderDetails.Add(new OrderDetails
            {
                OrderId = order.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,
                Size = item.Size
            });
        }

        await _context.SaveChangesAsync();
        LoggerHelper.WriteLog(_context, User, $"Created order #{order.OrderId} with {cart.Count} item(s) using {method}");

        // 🔥 REALTIME: ĐƠN MỚI
        await _hub.Clients.All.SendAsync("NewOrder", new
        {
            orderId=order.OrderId,
            total = order.TotalPrice,
            order.OrderStatus
        });

        HttpContext.Session.Remove("Cart");
        HttpContext.Session.Remove("CartBackup");

        return RedirectToAction("Invoice", new { id = order.OrderId });
    }

    // ================= INVOICE =================
    public IActionResult Invoice(int id)
    {
        var order = _context.tb_Order
            .Include(o => o.OrderDetails)
            .ThenInclude(d => d.Product)
            .FirstOrDefault(o => o.OrderId == id);

        if (order == null) return NotFound();

        return View(order);
    }
}