using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> 342cecc507d78faff00b79ec29079f2828c0259e

namespace FinalProject.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly WebDbContext _context;

        public ProductCategoryController(WebDbContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
        // LIST
        public IActionResult Index()
        {
            var data = _context.tb_ProductCategory.ToList();
            return View(data);
        }

        // CREATE
        public IActionResult Create()
        {
=======
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
>>>>>>> 342cecc507d78faff00b79ec29079f2828c0259e
            return View();
        }

        [HttpPost]
<<<<<<< HEAD
        public IActionResult Create(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                _context.tb_ProductCategory.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // EDIT
        public IActionResult Edit(int id)
        {
            var data = _context.tb_ProductCategory.Find(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(ProductCategory model)
        {
            _context.tb_ProductCategory.Update(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var data = _context.tb_ProductCategory.Find(id);

            if (data != null)
            {
                _context.tb_ProductCategory.Remove(data);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
=======
        public async Task<IActionResult> Create(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
>>>>>>> 342cecc507d78faff00b79ec29079f2828c0259e
        }
    }
}