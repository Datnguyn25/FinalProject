using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly WebDbContext _context;

        public ProductCategoryController(WebDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Load danh mục kèm theo thông tin danh mục cha của nó
            var categories = await _context.tb_ProductCategory
                                           .Include(c => c.ParentCategory)
                                           .ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            // Lấy danh sách danh mục để chọn ParentID
            ViewBag.ParentList = _context.tb_ProductCategory.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}
