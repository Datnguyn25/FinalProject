using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AdminController : Controller
    {
        public IActionResult Orders()
        {
            return View(_context.tb_Order.Include(o => o.User).ToList());
        }

        public IActionResult UpdateOrder(int id, string status)
        {
            var order = _context.tb_Order.Find(id);
            if (order == null) return NotFound();

            order.OrderStatus = status;
            if (status == "Completed")
            {
                order.Delivered = true;
                order.DeliveryDate = DateTime.Now;
            }
            
            WriteLog($"updated order: {order.OrderId} to status: {status}");
            _context.SaveChanges();
            return RedirectToAction("Orders");
        }

        public IActionResult OrderDetail(int id)
        {
            var order = _context.tb_Order
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return NotFound();
            return PartialView("_OrderDetailPartial", order);
        }

    }
}