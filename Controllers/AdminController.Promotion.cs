using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq;

namespace FinalProject.Controllers
{
    // Make sure the namespace and class name match your main AdminController
    public partial class AdminController : Controller
    {
        // 1. GET: /Admin/Promotion
        [Route("Admin/Promotions")]
        public IActionResult Promotion()
        {
            var promotions = _context.tb_Promotion
                .OrderByDescending(p => p.StartDate)
                .ToList();

            // Add the "s" here to match your file name Promotions.cshtml
            return View("Promotions", promotions);
        }

        // 2. GET: /Admin/CreatePromotion
        [HttpGet]
        public IActionResult CreatePromotion()
        {
            // Set default dates to avoid validation errors on empty forms
            var model = new Promotion
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };
            return View(model);
        }

        // 3. POST: /Admin/CreatePromotion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePromotion(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                // Logic: Check if PromotionCode already exists
                bool codeExists = _context.tb_Promotion.Any(p => p.PromotionCode == promotion.PromotionCode);
                if (codeExists)
                {
                    ModelState.AddModelError("PromotionCode", "This Promotion Code already exists.");
                    return View(promotion);
                }

                _context.tb_Promotion.Add(promotion);
                _context.SaveChanges();
                WriteLog($"added a new promotion: {promotion.Name}", $"Value: {promotion.DiscountPercent}%");
                return RedirectToAction(nameof(Promotion));
            }

            // If we reach here, there was an error
            return View(promotion);
        }

        // 4. POST: /Admin/DeletePromotion/5
        [HttpPost]
        public IActionResult DeletePromotion(int id)
        {
            var promotion = _context.tb_Promotion.Find(id);
            if (promotion != null)
            {
                WriteLog($"deleted promotion: {promotion.Name}");
                _context.tb_Promotion.Remove(promotion);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Promotion));
        }
    }
}