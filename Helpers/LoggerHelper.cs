using FinalProject.Data;
using FinalProject.Models;
using System.Security.Claims;

namespace FinalProject.Helpers
{
    public static class LoggerHelper
    {
        // Phải truyền context và user vào đây
        public static void WriteLog(WebDbContext context, ClaimsPrincipal user, string fullSentence, string technicalDetails = "")
        {
            // 1. Chặn các log không cần thiết
            if (string.IsNullOrWhiteSpace(fullSentence)) return;

            string actionLower = fullSentence.ToLower();
            if (actionLower.Contains("view") ||
                actionLower.Contains("xem") ||
                actionLower.Contains("details"))
            {
                return;
            }

            // 2. Lấy UserId trực tiếp từ Claims (Không cần UserManager)
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return;

            // 3. Tạo Log
            var log = new SystemLog
            {
                UserId = int.Parse(userIdString),
                Action = fullSentence,
                Details = technicalDetails,
                Timestamp = DateTime.Now
            };

            // 4. Lưu vào DB
            context.tb_SystemLog.Add(log);
            context.SaveChanges();
        }
    }
}