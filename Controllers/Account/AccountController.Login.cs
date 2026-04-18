using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    // BƯỚC 2: Ghi Log (Lúc này biến 'user' đã tồn tại)
                    var log = new SystemLog
                    {
                        // Nếu SystemLog.UserId là int, dùng user.Id
                        // Nếu SystemLog.UserId là string, dùng user.Id.ToString()
                        UserId = user.Id,
                        Action = "LOGIN",
                        Timestamp = DateTime.Now,
                        Details = $"User {model.Username} logged in successfully from IP: {HttpContext.Connection.RemoteIpAddress}"
                    };
                    _context.tb_SystemLog.Add(log);
                    await _context.SaveChangesAsync(); // Dùng SaveChangesAsync vì hàm đang là async
                }

                // BƯỚC 3: Điều hướng (Redirect)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }

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
                var user = await _userManager.FindByNameAsync(model.Username);
                var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);

                if (lockoutEndDate.HasValue)
                {
                    var remainingTime = lockoutEndDate.Value - DateTimeOffset.UtcNow;
                    // Lấy tổng số giây còn lại
                    var secondsLeft = (int)remainingTime.TotalSeconds;

                    ViewBag.SecondsLeft = secondsLeft;
                    ViewBag.Error = "Your account is locked due to multiple failed attempts.";
                }
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