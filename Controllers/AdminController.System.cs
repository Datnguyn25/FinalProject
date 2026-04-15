using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public IActionResult Promotions() => View(_context.tb_Promotion.ToList());

        [HttpPost]
        public IActionResult CreatePromotion(Promotion promo)
        {
            _context.tb_Promotion.Add(promo);
            _context.SaveChanges();
            return RedirectToAction("Promotions");
        }

        public IActionResult Settings()
        {
            return View(_context.tb_SystemSetting.FirstOrDefault());
        }

        [HttpPost]
        public IActionResult Settings(SystemSetting model)
        {
            _context.tb_SystemSetting.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Settings");
        }
    }
}