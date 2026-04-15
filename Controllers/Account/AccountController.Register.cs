using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public partial class AccountController
    {
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CreatedAt = DateTime.Now,
                IsActive = true,
                PhoneNumberConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                TempData["RegisterSuccess"] = "Your account has been created successfully!";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult RegisterBusiness()
        {
            var defaultModel = new Shop
            {
                ShopDescription = "male shop",
                City = "Default City",
                Country = "Vietnam"
            };
            return View(defaultModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterBusiness(Shop model)
        {
            // 1. Lấy user hiện tại
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            model.OwnerId = user.Id;
            model.RoleId = 3;
            model.CreatedAt = DateTime.Now;
            model.IsActive = true;
            model.RatingAverage = 0; // default 0
            model.TotalProducts = 0;

            model.ShopDescription = string.IsNullOrEmpty(model.ShopDescription) ? "Blank" : model.ShopDescription;
            model.City = string.IsNullOrEmpty(model.City) ? "Unknown" : model.City;
            model.Country = string.IsNullOrEmpty(model.Country) ? "Unknown" : model.Country;
            model.CoverImageUrl = "default-cover.jpg";
            model.LogoUrl = "default-logo.png";

            ModelState.Remove("City");
            ModelState.Remove("Country");
            ModelState.Remove("LogoUrl");
            ModelState.Remove("CoverImageUrl");
            ModelState.Remove("Owner");
            ModelState.Remove("Role");
            ModelState.Remove("OwnerId");



            if (ModelState.IsValid)
            { 
                var existingShop = await _context.tb_Shop.AnyAsync(s => s.OwnerId == user.Id);
                if (existingShop)
                {
                    TempData["ErrorMsg"] = "You already own a shop.";
                    return RedirectToAction("Index", "Home");
                }

                _context.tb_Shop.Add(model);
                await _context.SaveChangesAsync(); // Lưu shop trước

                // 4. Cập nhật Role cho User thành Shop
                var roleResult = await _userManager.AddToRoleAsync(user, "Shop");
                if (roleResult.Succeeded)
                {
                    // 5. Làm mới Identity Cookie để nhận Role mới
                    await _signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("Index", "SellerShop");
                }

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
    }
}