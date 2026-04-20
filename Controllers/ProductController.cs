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

    // ===== LIST + FILTER + SEARCH + PRICE FILTER =====
    public IActionResult Index(int? cateId, int? shopId, int? brandId, string keyword, string searchType,
                                decimal? minPrice, decimal? maxPrice)
    {
        var products = _context.tb_Product
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Shop)
            .AsQueryable();

        // ===== FILTER BY CATEGORY =====
        if (cateId.HasValue)
        {
            // L?y category con
            var childIds = _context.tb_ProductCategory
                .Where(c => c.ParentId == cateId)
                .Select(c => c.CateId)
                .ToList();

            // N?u là category cha ? l?y luôn con
            if (childIds.Any())
            {
                products = products.Where(p =>
                   p.CateId == cateId || (p.CateId.HasValue && childIds.Contains(p.CateId.Value)));
            }
            else
            {
                // N?u là category con ? l?c bình th??ng
                products = products.Where(p => p.CateId == cateId);
            }
        }

        // 2. NEW: FILTER BY BRAND
        if (brandId.HasValue)
        {
            products = products.Where(p => p.BrandId == brandId);
        }

        // 3. NEW: FILTER BY SHOP
        if (shopId.HasValue)
        {
            products = products.Where(p => p.ShopId == shopId);
        }

        // ===== SEARCH =====
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

        // ===== PRICE FILTER =====
      
        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue && maxPrice > 0)
        {
            products = products.Where(p => p.Price <= maxPrice.Value);
        }

        // ===== VIEWBAG FOR UI =====
        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Brands = _context.tb_Brand.ToList();
        ViewBag.Shops = _context.tb_Shop.ToList();

        ViewBag.Categories = _context.tb_ProductCategory.ToList();
        ViewBag.Keyword = keyword;
        ViewBag.SelectedCategory = cateId;
        ViewBag.SelectedBrand = brandId; // Gửi lại ID đã chọn để đánh dấu active trên UI
        ViewBag.SelectedShop = shopId;
        ViewBag.SearchType = searchType;
        ViewBag.MinPrice = minPrice ?? 0;
        ViewBag.MaxPrice = (maxPrice.HasValue && maxPrice > 0) ? maxPrice : 10000000;

        return View(products.ToList());
    }

    // ===== DETAILS =====
    public IActionResult Details(int id)
    {
        var product = _context.tb_Product
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Shop)
            .Include(p => p.ProductReviews)
                .ThenInclude(r => r.User)
            .FirstOrDefault(p => p.ProductId == id);

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
        var product = _context.tb_Product.FirstOrDefault(x => x.ProductId == id);

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
        var product = _context.tb_Product.FirstOrDefault(x => x.ProductId == model.ProductId);

        if (product == null)
            return NotFound();

        product.ProductName = model.ProductName;
        product.Price = model.Price;
        product.Quantity = model.Quantity;
        product.CateId = model.CateId;
        product.BrandId = model.BrandId;
        product.ShopId = model.ShopId;
        product.ProductDescription = model.ProductDescription;
        product.Image = model.Image;
        product.SeoTitle = model.SeoTitle;
        product.MetaKeywords = model.MetaKeywords;
        product.MetaDescription = model.MetaDescription;
        product.UpdatedDate = DateTime.Now;

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // ===== DELETE =====
    public IActionResult Delete(int id)
    {
        var product = _context.tb_Product.FirstOrDefault(x => x.ProductId == id);

        if (product != null)
        {
            _context.tb_Product.Remove(product);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}