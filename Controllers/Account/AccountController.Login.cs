using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FinalProject.ViewModels;

namespace FinalProject.Controllers
{
    public partial class AccountController
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.Error = null;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model, string? returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            
            var result = await _signInManager.PasswordSignInAsync(
                model.Username, 
                model.Password, 
                model.RememberMe, 
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // ƯU TIÊN 1: Nếu có returnUrl hợp lệ, quay lại đó ngay (ví dụ: quay lại Cart hoặc Checkout)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

                // ƯU TIÊN 2: Nếu không có returnUrl, mới phân quyền để về trang Dashboard tương ứng
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Shop")) return RedirectToAction("Index", "SellerShop");
                    if (roles.Contains("Admin")) return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ViewBag.Error = "Your account is temporarily locked. Please try again later.";
            }
            else
            {
                ViewBag.Error = "Invalid username or password.";
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}