using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public partial class AccountController
    {
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user, string password)
        {
            user.UserName = user.Email;
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            user.PhoneNumber = Guid.NewGuid().ToString();

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                TempData["SuccessMsg"] = "Your account has been successfully registered";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                ViewBag.Error = error.Description;
            }
            return View(user);
        }
    }
}