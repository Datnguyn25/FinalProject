using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public IActionResult Products()
        {
            return View(_context.tb_Product.Include(p => p.Category).Include(p => p.Shop).ToList());
        }

        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product model)
        {
            _context.tb_Product.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.tb_Product.Find(id);
            if (product == null) return NotFound();
            ViewBag.Categories = _context.tb_ProductCategory.ToList();
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product model)
        {
            _context.tb_Product.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }

        public IActionResult Categories() => View(_context.tb_ProductCategory.ToList());

        public IActionResult DeleteCategory(int id)
        {
            var cate = _context.tb_ProductCategory.Find(id);
            if (cate != null) { _context.tb_ProductCategory.Remove(cate); _context.SaveChanges(); }
            return RedirectToAction("Categories");
        }
    }
}