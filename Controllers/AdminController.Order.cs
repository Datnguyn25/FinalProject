using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public partial class AdminController
    {
        public IActionResult Orders()
        {
            return View(_context.tb_Order.Include(o => o.Customer).Include(o => o.OrderDetails).ToList());
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
            _context.SaveChanges();
            return RedirectToAction("Orders");
        }

        public IActionResult Revenue()
        {
            ViewBag.TotalRevenue = _context.tb_OrderDetails.Sum(d => d.Price * d.Quantity);
            return View();
        }
    }
}