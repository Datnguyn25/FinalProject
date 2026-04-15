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
    [HttpPost] // Nên dùng HttpPost để bảo mật dữ liệu giỏ hàng
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(int productId, int quantity = 1, string actionType = "add")
    {
        var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == productId);
        if (product == null) return NotFound();

        var cart = GetCart();
        var existingItem = cart.FirstOrDefault(x => x.ProductID == productId);

        // 1. Logic thêm/cập nhật giỏ hàng phải chạy TRƯỚC
        if (existingItem == null)
        {
            cart.Add(new CartItems
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Image = product.Image,
                Price = product.Price,
                Quantity = quantity // Dùng tham số quantity truyền vào
            });
        }
        else
        {
            existingItem.Quantity += quantity;
        }

        // 2. Lưu giỏ hàng mới vào Session/Cookie
        SaveCart(cart);
        // 👉 THÊM ĐOẠN NÀY
        int count = cart.Sum(x => x.Quantity);

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new
            {
                count = cart.Sum(x => x.Quantity)
            });
        }

        // 3. Kiểm tra xem người dùng nhấn nút nào để điều hướng
        if (!User.Identity.IsAuthenticated)
        {
            // Nếu chưa đăng nhập, dù bấm nút nào cũng phải qua trang Login
            // Lưu ý: returnUrl nên trỏ về trang thanh toán nếu là Buy Now, hoặc trang giỏ hàng nếu là Add
            string returnUrl = actionType == "buyNow" ? "/Cart/Checkout" : "/Cart";
            return RedirectToAction("Login", "Account", new { returnUrl = returnUrl });
        }

        // 4. Nếu đã đăng nhập, phân loại điều hướng theo actionType
        if (actionType == "buyNow")
        {
            return RedirectToAction("Checkout", "Cart");
        }

        // Mặc định cho Add to Cart là về trang giỏ hàng
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
        var cart = GetCart();
        if (cart.Count == 0) return RedirectToAction("Index", "Product");

        ViewBag.Cart = cart;
        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);

        return View();
    }

    // POST: /Cart/Checkout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutVM model)
    {
        var cart = GetCart();
        ViewBag.Cart = cart;
        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
        if (!ModelState.IsValid)
        {
            ViewBag.Cart = cart;
            ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
            return View(model);
        }

        double totalAmount = (double)cart.Sum(x => (double)x.Price * x.Quantity);

        if (model.PaymentMethod == "Momo")
        {
            // Tạo model thông tin đơn hàng gửi sang Momo
            var orderInfo = new OrderInfoModel
            {
                FullName = model.FullName,
                Amount = totalAmount,
                OrderInfo = "Thanh toán đơn hàng Fashion Store"
            };

            // Gọi service tạo link thanh toán
            var response = await _momoService.CreatePaymentMomo(orderInfo);

            // Chuyển hướng người dùng sang trang thanh toán của Momo
            if (response != null && !string.IsNullOrEmpty(response.PayUrl))
            {
                return Redirect(response.PayUrl);
            }

            ModelState.AddModelError("", "Hệ thống Momo đang bận, vui lòng thử lại sau.");
            ViewBag.Cart = cart;
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

            // Gọi service lấy link thanh toán (có chứa mã QR)
            var payUrl = await _zaloPayService.CreatePaymentUrl(orderInfo);

            if (!string.IsNullOrEmpty(payUrl))
            {
                return Redirect(payUrl); // Chuyển hướng đến trang chứa mã QR của ZaloPay
            }

            ModelState.AddModelError("", "Không thể khởi tạo thanh toán ZaloPay.");
            return View(model);
        }

        // Nếu là COD
        HttpContext.Session.Remove("Cart");
        return RedirectToAction("Success", new { method = "COD" });
    }

    [HttpGet]
    [Route("Cart/ZaloPayCallBack")]
    public IActionResult ZaloPayCallBack()
    {
        // ZaloPay Sandbox trả về status hoặc các tham số trong Query
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
    [Route("Checkout/PaymentCallBack")] // Ép đường dẫn khớp với appsettings.json
    public IActionResult PaymentCallBack()
    {
        // 1. Lấy thông tin từ URL mà Momo trả về
        var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);

        // 2. Kiểm tra mã lỗi (errorCode = 0 là thành công)
        string errorCode = HttpContext.Request.Query["errorCode"];

        if (errorCode == "0")
        {
            // Thanh toán thành công: Xóa giỏ hàng và sang trang Success
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success", new { method = "Momo" });
        }

        // Thanh toán thất bại: Quay lại trang Checkout và báo lỗi
        TempData["Error"] = "Transaction failed or canceled";
        return RedirectToAction("Checkout");
    }
    public IActionResult Success(string method)
    {
        ViewBag.Method = method;
        return View();
    }
}
