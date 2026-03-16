using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly WebDbContext _context;

        public AccountController(WebDbContext context)
        {
            _context = context;
        }

        // LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN USERNAME PASSWORD
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.tb_Users
                .FirstOrDefault(u => u.UserName == username && u.PasswordHash == password);

            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai username hoặc password";
            return View();
        }

        // REGISTER PAGE
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER USERNAME PASSWORD
        [HttpPost]
        public IActionResult Register(User user)
        {
            var checkUser = _context.tb_Users
                .FirstOrDefault(u => u.UserName == user.UserName);

            if (checkUser != null)
            {
                ViewBag.Error = "Username đã tồn tại";
                return View();
            }

            _context.tb_Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // LOGIN GOOGLE
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // GOOGLE RESPONSE
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if(!result.Succeeded)
                return RedirectToAction("Login");

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            var user = _context.tb_Users
                .Include(u => u.Role)  // Đảm bảo load luôn Role để lấy RoleId
                .FirstOrDefault(u => u.Email == email);

            // nếu chưa có user thì tạo mới
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    UserName = name,
                    PasswordHash = null,      // Bạn đã sửa file Migration thành nullable: true rồi nên OK
                    RoleId = 2,               // <--- QUAN TRỌNG: Gán ID của Role "Customer"
                    CreatedAt = DateTime.Now,  // Các trường bắt buộc khác (nếu có)
                    IsActive = true,
                    EmailVerified = true
                };

                _context.tb_Users.Add(user);
                await _context.SaveChangesAsync();
                user = _context.tb_Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.UserID == user.UserID); // Lấy lại user sau khi đã có ID
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName ?? "User") // Lấy RoleName từ Role đã load
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        // LOGOUT
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}