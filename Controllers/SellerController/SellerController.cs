using FinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class SellerController : Controller
{
    private readonly WebDbContext _context;

    public SellerController(WebDbContext context)
    {
        _context = context;
    }

    // 🔥 DASHBOARD
    public IActionResult Dashboard(DateTime? fromDate, DateTime? toDate)
    {
        var query = _context.tb_Order.AsQueryable();

        // FILTER NGÀY
        if (fromDate.HasValue)
            query = query.Where(o => o.CreatedDate >= fromDate);

        if (toDate.HasValue)
            query = query.Where(o => o.CreatedDate <= toDate);

        // 🔥 TỔNG DOANH THU
        var revenue = query.Sum(o => o.TotalPrice);

        // 🔥 SỐ ĐƠN
        var totalOrders = query.Count();

        // 🔥 ĐƠN ĐÃ GIAO
        var deliveredOrders = query.Count(o => o.Delivered == true);

        // 🔥 TOP SẢN PHẨM
        var topProducts = _context.tb_OrderDetails
            .Include(x => x.Product)
            .GroupBy(x => x.Product.ProductName)
            .Select(g => new
            {
                ProductName = g.Key,
                TotalSold = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.TotalSold)
            .Take(5)
            .ToList();

        ViewBag.Revenue = revenue;
        ViewBag.TotalOrders = totalOrders;
        ViewBag.DeliveredOrders = deliveredOrders;
        ViewBag.TopProducts = topProducts;

        return View();
    }
}