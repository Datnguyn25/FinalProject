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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(WebDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN USERNAME PASSWORD
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please do not leave your username and password empty";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ViewBag.Error = "Your account has been temporarily locked due to multiple error inputs. Please try again later";
            }
            else
            {
                ViewBag.Error = "Invalid username or password";
            }

            return View();
        }

        // REGISTER PAGE
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER USERNAME PASSWORD
        [HttpPost]
        public async Task<IActionResult> Register(User user, string password)
        {
            user.UserName = user.Email;
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;

            user.PhoneNumber = Guid.NewGuid().ToString();
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // gan quyen mac dinh
                await _userManager.AddToRoleAsync(user, "User");

                TempData["SuccessMsg"] = "Your account has been successfully registered";
                return RedirectToAction("Login");
            }

            // neu loi, nap loi vao modelstate de hien thi ra view
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                ViewBag.Error = error.Description; // display alert
            }
            return View(user);
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
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded) return RedirectToAction("Login");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var googleName = result.Principal.FindFirstValue(ClaimTypes.Name) ?? result.Principal.FindFirstValue("name");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    EmailVerified = true,
                    FullName = googleName,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            var claims = new List<Claim>
            {
                new Claim("FullName", googleName ?? user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

            return RedirectToAction("Index", "Home");
        }
        

        // LOGOUT
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}