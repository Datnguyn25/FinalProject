using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FinalProject.Filters
{
    public class SystemLogFilter : IActionFilter
    {
        private readonly WebDbContext _context;

        public SystemLogFilter(WebDbContext context)
        {
            _context = context;
        }

        // 👉 Lưu id tạm
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey("id"))
            {
                context.HttpContext.Items["Action_Id"] =
                    context.ActionArguments["id"]?.ToString();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                var controller = context.ActionDescriptor.RouteValues["controller"];
                var action = context.ActionDescriptor.RouteValues["action"];

                // ❗ chỉ log admin
                if (!controller.ToLower().Contains("admin"))
                    return;

                // 👉 lấy userId
                int userId = 0;

                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var claim = context.HttpContext.User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                    if (claim != null)
                        int.TryParse(claim.Value, out userId);
                }

                // 👉 lấy id đã lưu
                var id = context.HttpContext.Items["Action_Id"]?.ToString() ?? "";

                // 👉 phân loại action
                string actionType = "VIEW";
                var actionLower = action.ToLower();

                if (actionLower.Contains("create")) actionType = "CREATE";
                else if (actionLower.Contains("edit") || actionLower.Contains("update")) actionType = "UPDATE";
                else if (actionLower.Contains("delete")) actionType = "DELETE";

                var log = new SystemLog
                {
                    UserId = userId,
                    Action = actionType,
                    Details = $"{controller}.{action} (ID={id})",
                    Timestamp = DateTime.Now
                };

                _context.tb_SystemLog.Add(log);
                _context.SaveChanges();
            }
            catch
            {
                // tránh crash
            }
        }
    }
}