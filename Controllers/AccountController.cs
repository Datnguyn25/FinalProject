using FinalProject.Data;
using FinalProject.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
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
       .FirstOrDefault(u => u.UserName == username);

            if (user != null)
            {
                var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (result == PasswordVerificationResult.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // If login fails, return the Login view (optionally with an error message)
            ViewBag.Error = "Invalid username or password";
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

            // Kiểm tra mật khẩu mạnh
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$";

            if (!Regex.IsMatch(user.PasswordHash, pattern))
            {
                ViewBag.Error = "Mật khẩu phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt.";
                return View();
            }

            // Hash password
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);

            _context.tb_Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // LOGIN GOOGLE
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", "Account")
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // GOOGLE RESPONSE
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return RedirectToAction("Login");

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            if (email == null)
                return RedirectToAction("Login");

            var user = _context.tb_Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    UserName = name,
                    RoleId = 2,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    EmailVerified = true
                };

                _context.tb_Users.Add(user);
                await _context.SaveChangesAsync();

                user = _context.tb_Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.UserID == user.UserID);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User")
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