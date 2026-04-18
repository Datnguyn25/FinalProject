using FinalProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ShopController : Controller
{
    private readonly WebDbContext _context;

    public ShopController(WebDbContext context)
    {
        _context = context;
    }

    // SHOP LIST
    public IActionResult Index()
    {
        var shops = _context.tb_Shop.ToList();
        return View(shops);
    }

    // PRODUCT BY SHOP
    public IActionResult Details(int id)
    {
        var products = _context.tb_Product
            .Include(p => p.Shop)
            .Where(p => p.ShopId == id)
            .ToList();

        ViewBag.Shop = _context.tb_Shop.Find(id);

        return View(products);
    }
}