using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ProductController : Controller
{
    private readonly WebDbContext _context;

    public ProductController(WebDbContext context)
    {
        _context = context;
    }

    // ===== DANH SÁCH + FILTER =====
    public IActionResult Index(int? categoryId, int? brandId)
    {
        var products = _context.tb_Product
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Shop)
            .AsQueryable();

        if (categoryId.HasValue)
            products = products.Where(p => p.CateID == categoryId);

        if (brandId.HasValue)
            products = products.Where(p => p.BrandID == brandId);

        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Brands = _context.tb_Brand.ToList();

        return View(products.ToList());
    }

    // ===== CHI TIẾT =====
    public IActionResult Details(int id)
    {
        var product = _context.tb_Product
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Shop)
            .FirstOrDefault(p => p.ProductID == id);

        if (product == null)
            return NotFound();

        return View(product);
    }

    // ===== TẠO =====
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.tb_ProductCategory, "CategoryId", "CategoryName");
        ViewBag.Brands = new SelectList(_context.tb_Brand, "BrandID", "Name");
        ViewBag.Shops = new SelectList(_context.tb_Shop, "ShopID", "ShopName");

        return View();
    }

    [HttpPost]
    public IActionResult Create(Product model)
    {
        if (ModelState.IsValid)
        {
            _context.tb_Product.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // reload dropdown nếu lỗi
        ViewBag.Categories = new SelectList(_context.tb_ProductCategory, "CategoryId", "CategoryName", model.CateID);
        ViewBag.Brands = new SelectList(_context.tb_Brand, "BrandID", "Name", model.BrandID);
        ViewBag.Shops = new SelectList(_context.tb_Shop, "ShopID", "ShopName", model.ShopID);

        return View(model);
    }

    // ===== EDIT =====
    public IActionResult Edit(int id)
    {
        var product = _context.tb_Product.Find(id);
        if (product == null) return NotFound();

        ViewBag.Categories = new SelectList(_context.tb_ProductCategory, "CategoryId", "CategoryName", product.CateID);
        ViewBag.Brands = new SelectList(_context.tb_Brand, "BrandID", "Name", product.BrandID);
        ViewBag.Shops = new SelectList(_context.tb_Shop, "ShopID", "ShopName", product.ShopID);

        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Product model)
    {
        if (ModelState.IsValid)
        {
            _context.tb_Product.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewBag.Categories = new SelectList(_context.tb_ProductCategory, "CategoryId", "CategoryName", model.CateID);
        ViewBag.Brands = new SelectList(_context.tb_Brand, "BrandID", "Name", model.BrandID);
        ViewBag.Shops = new SelectList(_context.tb_Shop, "ShopID", "ShopName", model.ShopID);

        return View(model);
    }

    // ===== DELETE =====
    public IActionResult Delete(int id)
    {
        var product = _context.tb_Product.Find(id);
        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirm(int id)
    {
        var product = _context.tb_Product.Find(id);

        if (product != null)
        {
            _context.tb_Product.Remove(product);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    // ===== SEARCH =====
    public IActionResult Search(string keyword)
    {
        var products = _context.tb_Product
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Shop)
            .Where(p => string.IsNullOrEmpty(keyword) || p.ProductName.Contains(keyword))
            .ToList();

        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Brands = _context.tb_Brand.ToList();

        return View("Index", products);
    }
}