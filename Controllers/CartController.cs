using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services.Momo;
using FinalProject.Services.Zalo;
using FinalProject.Services.ZaloPay;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

public class CartController : Controller
{
    private readonly WebDbContext _context;
    private readonly IMomoService _momoService;
    private readonly IZaloPayService _zaloPayService;

    public CartController(WebDbContext context, IMomoService momoService, IZaloPayService zaloPayService)
    {
        _context = context;
        _momoService = momoService;
        _zaloPayService = zaloPayService;
    }

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
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { redirect = Url.Action("Login", "Account") });
            }

            return RedirectToAction("Login", "Account");
        }

        var product = _context.tb_Product.FirstOrDefault(p => p.ProductId == productId);
        if (product == null) return NotFound();

        var cart = GetCart();


        var existingItem = cart.FirstOrDefault(x => x.ProductId == productId && x.Size == size);


        if (existingItem == null)
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
            existingItem.Quantity += quantity;
        }

        SaveCart(cart);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new
            {
                count = cart.Sum(x => x.Quantity)
            });
        }

        if (actionType == "buyNow")
        {
            return RedirectToAction("Checkout", "Cart");
        }

        return RedirectToAction("Index", "Cart");
    }

    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id);

        if (item != null)
        {
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

    [HttpGet]
    public IActionResult Checkout()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = GetCart();
        if (cart.Count == 0) return RedirectToAction("Index", "Product");

        ViewBag.Cart = cart;
        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutVM model, int? SingleProductId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = GetCart();

        ViewBag.Cart = cart;
        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        double totalAmount = (double)cart.Sum(x => (double)x.Price * x.Quantity);

        // ================= MOMO =================
        if (model.PaymentMethod?.ToLower() == "momo")
        {
            // 🔥 FIX SESSION MẤT
            HttpContext.Session.SetString("CartBackup", JsonConvert.SerializeObject(cart));
            HttpContext.Session.SetString("TempCart", JsonConvert.SerializeObject(cart));

            var orderInfo = new OrderInfoModel
            {
                FullName = model.FullName,
                Amount = totalAmount,
                OrderInfo = "Thanh toán đơn hàng"
            };

            var response = await _momoService.CreatePaymentMomo(orderInfo);

            if (response == null)
            {
                ModelState.AddModelError("", "MoMo không trả về dữ liệu");
                return View(model);
            }

            if (string.IsNullOrEmpty(response.PayUrl))
            {
                ModelState.AddModelError("", "MoMo lỗi: " + response.Message);
                return View(model);
            }

            return Redirect(response.PayUrl);
        }

        // ================= ZALOPAY =================
        if (model.PaymentMethod?.ToLower() == "zalopay")
        {
            var orderInfo = new OrderInfoModel
            {
                FullName = model.FullName,
                Amount = totalAmount,
                OrderInfo = "Thanh toán đơn hàng"
            };

            var payUrl = await _zaloPayService.CreatePaymentUrl(orderInfo);

            if (!string.IsNullOrEmpty(payUrl))
            {
                return Redirect(payUrl);
            }

            ModelState.AddModelError("", "Lỗi ZaloPay");
            return View(model);
        }

        // ================= COD =================
        HttpContext.Session.Remove("Cart");
        return RedirectToAction("Success", new { method = "COD" });
    }

    // ================= MOMO CALLBACK =================
    [AllowAnonymous]
    [HttpGet]
    [Route("Checkout/PaymentCallBack")] // 🔥 FIX 404
    public IActionResult PaymentCallBack()
    {
        var resultCode = HttpContext.Request.Query["resultCode"].ToString();

        if (resultCode == "0")
        {
            // 🔥 FIX SESSION
            var session = HttpContext.Session.GetString("CartBackup");

            if (string.IsNullOrEmpty(session))
            {
                session = HttpContext.Session.GetString("TempCart");
            }

            if (string.IsNullOrEmpty(session))
            {
                return Content("Mất session → không có giỏ hàng");
            }

            var cart = JsonConvert.DeserializeObject<List<CartItems>>(session);

            // 🔥 FIX USER ID
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                OrderStatus = "Completed",
                PaymentStatus = "Paid",
                TotalPrice = cart.Sum(x => x.Price * x.Quantity)
            };

            _context.tb_Order.Add(order);
            _context.SaveChanges();

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

            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("CartBackup");
            HttpContext.Session.Remove("TempCart");

            return RedirectToAction("Invoice", new { id = order.OrderId });
        }

        return Content("Thanh toán thất bại");
    }

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