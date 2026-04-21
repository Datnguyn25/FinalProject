using FinalProject.Helpers;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AccountController
    {
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new UserProfileVM
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                AvatarUrl = user.AvatarUrl,
                City = user.City,
                District = user.District,
                Ward = user.Ward
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UserProfileVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Lưu vết thay đổi để ghi log
            string changes = "";
            if (user.FullName != model.FullName) changes += $"Name: {user.FullName} -> {model.FullName}, ";
            if (user.PhoneNumber != model.PhoneNumber) changes += $"Phone: {user.PhoneNumber} -> {model.PhoneNumber}, ";

            // Xử lý Upload ảnh (nếu có)
            if (model.AvatarFile != null)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.AvatarFile.FileName);
                var path = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.AvatarFile.CopyToAsync(stream);
                }
                user.AvatarUrl = "/uploads/avatars/" + fileName;
                changes += "Changed Avatar, ";
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.City = model.City;
            user.District = model.District;
            user.Ward = model.Ward;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // 🔥 GHI LOG: User tự cập nhật profile
                if (!string.IsNullOrEmpty(changes))
                {
                    LoggerHelper.WriteLog(_context, User, "Updated personal profile", changes.TrimEnd(' ', ','));
                }
                TempData["Success"] = "Profile updated successfully!";
            }

            return RedirectToAction("Profile");
        }
    }
}
