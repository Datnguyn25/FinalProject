using FinalProject.Data;
using FinalProject.Models;
using FinalProject.Services.Momo;
using FinalProject.Services.Zalo;
using FinalProject.Services.ZaloPay;
using FinalProject.ViewModels;
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

    // VIEW CART
    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    // ADD TO CART
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(int productId, int quantity = 1, string actionType = "add")
    {
        // 🔥 FIX 1: CHECK LOGIN TRƯỚC
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
        var existingItem = cart.FirstOrDefault(x => x.ProductID == productId);

        if (existingItem == null)
        {
            cart.Add(new CartItems
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Image = product.Image,
                Price = product.Price,
                Quantity = quantity
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

    // REMOVE
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

    // GET SESSION
    private List<CartItems> GetCart()
    {
        var session = HttpContext.Session.GetString("Cart");
        if (string.IsNullOrEmpty(session)) return new List<CartItems>();
        return JsonConvert.DeserializeObject<List<CartItems>>(session);
    }

    // SAVE SESSION
    private void SaveCart(List<CartItems> cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        // 🔥 FIX 2: CHẶN CHƯA LOGIN
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
        // 🔥 FIX 3: CHẶN CHƯA LOGIN
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = GetCart();

        if (SingleProductId != null)
        {
            cart = cart.Where(x => x.ProductID == SingleProductId).ToList();
        }

        ViewBag.Cart = cart;
        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        double totalAmount = (double)cart.Sum(x => (double)x.Price * x.Quantity);

        if (model.PaymentMethod == "Momo")
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

            ModelState.AddModelError("", "Hệ thống Momo đang bận, vui lòng thử lại sau.");
            return View(model);
        }

        if (model.PaymentMethod == "ZaloPay")
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

            ModelState.AddModelError("", "Không thể khởi tạo thanh toán ZaloPay.");
            return View(model);
        }

        HttpContext.Session.Remove("Cart");
        return RedirectToAction("Success", new { method = "COD" });
    }

    [HttpGet]
    [Route("Cart/ZaloPayCallBack")]
    public IActionResult ZaloPayCallBack()
    {
        var status = Request.Query["status"];

        if (status == "1")
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "ZaloPay" });
        }

        TempData["Error"] = "Transaction was done unsuccessfully";
        return RedirectToAction("Checkout");
    }

    [HttpGet]
    [Route("Checkout/PaymentCallBack")]
    public IActionResult PaymentCallBack()
    {
        var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
        string errorCode = HttpContext.Request.Query["errorCode"];

        if (errorCode == "0")
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "Momo" });
        }

        TempData["Error"] = "Transaction failed or canceled";
        return RedirectToAction("Checkout");
    }

    public IActionResult Success(string method)
    {
        ViewBag.Method = method;
        return View();
    }

    public IActionResult GetCartCount()
    {
        var cart = GetCart();
        return Json(new { count = cart.Sum(x => x.Quantity) });
    }
}