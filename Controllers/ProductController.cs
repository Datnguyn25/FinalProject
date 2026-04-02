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

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CateID == categoryId);
        }

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

                default:
                    products = products.Where(p =>
                        p.ProductName != null &&
                        p.ProductName.ToLower().Contains(keyword));
                    break;
            }
        }

        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Keyword = keyword;
        ViewBag.SelectedCategory = categoryId;
        ViewBag.SearchType = searchType;

        return View(products.ToList());
    }

    // ===== DETAILS =====
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
        var product = _context.tb_Product.FirstOrDefault(x => x.ProductID == id);

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
        var product = _context.tb_Product.FirstOrDefault(x => x.ProductID == model.ProductID);

        if (product == null)
            return NotFound();

        // ===== UPDATE FIELD (KHÔNG UPDATE NGUYÊN OBJECT) =====
        product.ProductName = model.ProductName;
        product.Price = model.Price;
        product.Quantity = model.Quantity;
        product.CateID = model.CateID;
        product.BrandID = model.BrandID;
        product.ShopID = model.ShopID;
        product.ProductDescription = model.ProductDescription;
        product.Image = model.Image;
        product.SeoTitle = model.SeoTitle;
        product.MetaKeywords = model.MetaKeywords;
        product.MetaDescription = model.MetaDescription;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // ===== DELETE =====
    public IActionResult Delete(int id)
    {
        var product = _context.tb_Product.FirstOrDefault(x => x.ProductID == id);

        if (product != null)
        {
            _context.tb_Product.Remove(product);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}
