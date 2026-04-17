using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services.Momo;
using FinalProject.Services.Zalo;
using FinalProject.Services.ZaloPay;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

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

        var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == productId);
        if (product == null) return NotFound();

        var cart = GetCart();

        // 🔥 FIX: phân biệt theo size
        var existingItem = cart.FirstOrDefault(x => x.ProductID == productId && x.Size == size);

        if (existingItem == null)
        {
            cart.Add(new CartItems
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Image = product.Image,
                Price = product.Price,
                Quantity = quantity,
                Size = size // 🔥 LƯU SIZE
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
        var item = cart.FirstOrDefault(x => x.ProductID == id);

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

        // ✅ MOMO
        if (model.PaymentMethod?.ToLower() == "momo")
        {
            var orderInfo = new OrderInfoModel
            {
                FullName = model.FullName,
                Amount = totalAmount,
                OrderInfo = "Thanh toán đơn hàng Fashion Store"
            };

            var response = await _momoService.CreatePaymentMomo(orderInfo);

            if (response != null && !string.IsNullOrEmpty(response.PayUrl))
            {
                return Redirect(response.PayUrl);
            }

            ModelState.AddModelError("", "Lỗi kết nối MoMo");
            return View(model);
        }

        // ✅ ZALOPAY
        if (model.PaymentMethod?.ToLower() == "zalopay")
        {
            var orderInfo = new OrderInfoModel
            {
                FullName = model.FullName,
                Amount = totalAmount,
                OrderInfo = "Thanh toán đơn hàng Fashion Store"
            };

            var payUrl = await _zaloPayService.CreatePaymentUrl(orderInfo);

            if (!string.IsNullOrEmpty(payUrl))
            {
                return Redirect(payUrl);
            }

            ModelState.AddModelError("", "Lỗi ZaloPay");
            return View(model);
        }

        // ✅ COD
        HttpContext.Session.Remove("Cart");
        return RedirectToAction("Success", new { method = "COD" });
    }
    // 🔥 MOMO CALLBACK
    [AllowAnonymous]
    [HttpGet]
    [Route("Checkout/PaymentCallBack")]
     // 🔥 QUAN TRỌNG
    public IActionResult PaymentCallBack()
    {
        var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
        string errorCode = HttpContext.Request.Query["errorCode"];

        if (errorCode == "0")
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "Momo" });
        }

        TempData["Error"] = "Thanh toán thất bại";
        return RedirectToAction("Checkout");
    }
   
}
