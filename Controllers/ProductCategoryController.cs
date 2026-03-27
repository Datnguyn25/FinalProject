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


        // LIST
        public IActionResult Index()
        {
            var data = _context.tb_ProductCategory.ToList();
            return View(data);
        }

        // CREATE
        public IActionResult Create()
        {
            // Lấy danh sách danh mục để chọn ParentID
            ViewBag.ParentList = _context.tb_ProductCategory.ToList();

            return View();
        }

        [HttpPost]
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
        }
    }
}
