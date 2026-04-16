using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public IActionResult Promotions() => View(_context.tb_Promotion.ToList());

       

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