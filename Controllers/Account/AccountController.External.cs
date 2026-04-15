using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public partial class AccountController
    {
        public IActionResult GoogleLogin(bool rememberMe = false)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", "Account"),
                IsPersistent = rememberMe
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded) return RedirectToAction("Login");

            bool isPersistent = result.Properties?.IsPersistent ?? false;
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
                new Claim("FullName", user.FullName ?? googleName ?? user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName ?? googleName ?? user.UserName)
            };

            await _signInManager.SignInWithClaimsAsync(user, isPersistent: isPersistent, claims);
            return RedirectToAction("Index", "Home");
        }
    }
}