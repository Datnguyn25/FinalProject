using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductController : Controller
{
    private readonly WebDbContext _context;

    public ProductController(WebDbContext context)
    {
        _context = context;
    }

    // ===== LIST + FILTER + SEARCH =====
    public IActionResult Index(int? categoryId, string keyword, string searchType)
    {
        var products = _context.tb_Product
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Shop)
            .AsQueryable();

        // ===== FILTER CATEGORY =====
        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CateID == categoryId);
        }

        // ===== SEARCH (DROPDOWN) =====
        if (!string.IsNullOrEmpty(keyword))
        {
            keyword = keyword.ToLower();

            switch (searchType)
            {
                case "seo":
                    products = products.Where(p =>
                        p.SeoTitle != null &&
                        p.SeoTitle.ToLower().Contains(keyword));
                    break;

                case "keyword":
                    products = products.Where(p =>
                        p.MetaKeywords != null &&
                        p.MetaKeywords.ToLower().Contains(keyword));
                    break;

                case "desc":
                    products = products.Where(p =>
                        p.MetaDescription != null &&
                        p.MetaDescription.ToLower().Contains(keyword));
                    break;

                default: // name
                    products = products.Where(p =>
                        p.ProductName != null &&
                        p.ProductName.ToLower().Contains(keyword));
                    break;
            }
        }

        // ===== VIEWBAG =====
        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Keyword = keyword;
        ViewBag.SelectedCategory = categoryId;
        ViewBag.SearchType = searchType;

        return View(products.ToList());
    }

    // ===== DETAIL =====
    public IActionResult Details(int id)
    {
        var product = _context.tb_Product
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Shop)
            .FirstOrDefault(p => p.ProductID == id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    // ===== CREATE =====
    public IActionResult Create()
    {
        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Brands = _context.tb_Brand.ToList();
        ViewBag.Shops = _context.tb_Shop.ToList();

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

        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Brands = _context.tb_Brand.ToList();
        ViewBag.Shops = _context.tb_Shop.ToList();

        return View(model);
    }


    // ===== EDIT =====
    public IActionResult Edit(int id)
    {
        var product = _context.tb_Product.Find(id);

        if (product == null)
            return NotFound();

        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Brands = _context.tb_Brand.ToList();
        ViewBag.Shops = _context.tb_Shop.ToList();

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

        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Brands = _context.tb_Brand.ToList();
        ViewBag.Shops = _context.tb_Shop.ToList();

        return View(model);
    }


    // ===== DELETE =====
    public IActionResult Delete(int id)
    {
        var product = _context.tb_Product.Find(id);

        if (product != null)
        {
            _context.tb_Product.Remove(product);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }   
}