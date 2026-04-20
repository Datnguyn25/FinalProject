using FinalProject.Data;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class SellerDashboardController : Controller
{
    private readonly WebDbContext _context;

    public SellerDashboardController(WebDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }

    private int? GetShopId()
    {
        return _context.tb_Shop
            .Where(s => s.OwnerId == GetUserId())
            .Select(s => s.ShopId)
            .FirstOrDefault();
    }

    public IActionResult Index(DateTime? fromDate, DateTime? toDate)
    {
        var shopId = GetShopId();
        if (shopId == null) return Content("Chưa có shop");

        var orders = _context.tb_Order
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .Where(o => o.PaymentStatus == "Paid")
            .Where(o => o.OrderDetails.Any(od => od.Product.ShopId == shopId));

        if (fromDate.HasValue)
            orders = orders.Where(o => o.CreatedDate >= fromDate);

        if (toDate.HasValue)
            orders = orders.Where(o => o.CreatedDate <= toDate);

        var list = orders.ToList();

        var revenue = list.Sum(o =>
            o.OrderDetails
             .Where(od => od.Product.ShopId == shopId)
             .Sum(od => od.Price * od.Quantity));

        var top = list
            .SelectMany(o => o.OrderDetails)
            .Where(od => od.Product.ShopId == shopId)
            .GroupBy(x => x.Product.ProductName)
            .Select(g => new TopProductVM
            {
                ProductName = g.Key,
                TotalSold = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.TotalSold)
            .Take(5)
            .ToList();

        var vm = new SellerDashboardVM
        {
            TotalRevenue = revenue,
            TotalOrders = list.Count,
            TotalProductsSold = top.Sum(x => x.TotalSold),
            TopProducts = top
        };

        return View(vm);
    }

    // ================= CHART API =================
    [HttpGet]
    public IActionResult GetChartData(int days = 7)
    {
        var shopId = GetShopId();
        if (shopId == null) return Json(new List<object>());

        var rawData = _context.tb_Order
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
            .Where(o => o.PaymentStatus == "Paid")
            .Where(o => o.OrderDetails.Any(od => od.Product.ShopId == shopId))
            .SelectMany(o => o.OrderDetails
                .Where(od => od.Product.ShopId == shopId)
                .Select(od => new
                {
                    Date = o.CreatedDate.Date,
                    Revenue = od.Price * od.Quantity,
                    Profit = (od.Price * od.Quantity) * 0.2m
                }))
            .ToList();

        var dateRange = Enumerable.Range(0, days)
            .Select(i => DateTime.Today.AddDays(-i))
            .OrderBy(d => d)
            .ToList();

        var result = dateRange.Select(day =>
        {
            var dayData = rawData.Where(x => x.Date == day);

            return new
            {
                date = day.ToString("dd/MM"),
                revenue = dayData.Sum(x => x.Revenue),
                profit = dayData.Sum(x => x.Profit)
            };
        }).ToList();

        return Json(result);
    }
}

