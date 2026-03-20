using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller
{
    private readonly WebDbContext _context;

    public OrderController(WebDbContext context)
    {
        _context = context;
    }

    public IActionResult Checkout()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Checkout(string name, string phone, string address)
    {
        var order = new Order
        {
            OrderDate = DateTime.Now,
            OrderStatus = "Pending",
            PaymentStatus = "Unpaid",
            Delivered = false,
            Discount = 0
            // Set other required properties as needed
        };

        _context.tb_Order.Add(order);
        _context.SaveChanges();

        HttpContext.Session.Remove("Cart");

        return RedirectToAction("Success");
    }

    public IActionResult Success()
    {
        return View();
    }
}