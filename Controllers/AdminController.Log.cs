using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        private void WriteLog(string fullSentence, string technicalDetails = "")
        {
            if (string.IsNullOrWhiteSpace(fullSentence) ||
                fullSentence.Equals("VIEW", StringComparison.OrdinalIgnoreCase) ||
                fullSentence.ToLower().Contains("view"))
            {
                return;
            }
            string actionLower = fullSentence.ToLower();
            if (actionLower.Contains("view") ||
                actionLower.Contains("xem") ||
                actionLower.Contains("details"))
            {
                return;
            }

            var userIdString = _userManager.GetUserId(User);
            if (userIdString == null) return;

            var log = new SystemLog
            {
                UserId = int.Parse(userIdString),
                Action = fullSentence, // Generate sentence: "Admin updated product A: Price from 10 to 20, Name from X to Y"
                Details = technicalDetails,
                Timestamp = DateTime.Now
            };
            _context.tb_SystemLog.Add(log);
            _context.SaveChanges();
        }

        // Tạo view để xem lịch sử Log
        public IActionResult Logs()
        {
            var logs = _context.tb_SystemLog
                       .Include(l => l.User)
                       .OrderByDescending(l => l.Timestamp)
                       .Take(200)
                       .ToList();
            return View(logs);
        }


    }
}
