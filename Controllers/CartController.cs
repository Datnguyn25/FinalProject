using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CartController : Controller
{
    private readonly WebDbContext _context;

    public CartController(WebDbContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (userId == null)
        {
            return 0; // chưa login
        }
        return int.Parse(userId);
    }

    public IActionResult AddToCart(int productId)
    {
        int userId = GetUserId();
        if (userId == 0)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = _context.tb_Carts.FirstOrDefault(c => c.CustomerId == userId);

        if (cart == null)
        {
            cart = new Carts { CustomerId = userId };
            _context.tb_Carts.Add(cart);
            _context.SaveChanges();
        }

        var item = _context.tb_CartItems
            .FirstOrDefault(i => i.ProductID == productId && i.CartId == cart.CartId);

        if (item == null)
        {
            item = new CartItems
            {
                ProductID = productId,
                CartId = cart.CartId,
                Quantity = 1
            };
            _context.tb_CartItems.Add(item);
        }
        else
        {
            item.Quantity++;
        }

        _context.SaveChanges();

        return RedirectToAction("Cart");
    }

    public IActionResult Cart()
    {
        int userId = GetUserId();

        var cart = _context.tb_Carts
            .Include(c => c.CartItems)
            .ThenInclude(i => i.Product)
            .FirstOrDefault(c => c.CustomerId == userId);

        return View(cart);
    }

    public IActionResult Increase(int id)
    {
        var item = _context.tb_CartItems.Find(id);
        if (item != null)
        {
            item.Quantity++;
            _context.SaveChanges();
        }
        return RedirectToAction("Cart");
    }

    public IActionResult Decrease(int id)
    {
        var item = _context.tb_CartItems.Find(id);
        if (item != null)
        {
            item.Quantity--;
            if (item.Quantity <= 0)
            {
                _context.tb_CartItems.Remove(item);
            }
            _context.SaveChanges();
        }
        return RedirectToAction("Cart");
    }

    public IActionResult Remove(int id)
    {
        var item = _context.tb_CartItems.Find(id);
        if (item != null)
        {
            _context.tb_CartItems.Remove(item);
            _context.SaveChanges();
        }
        return RedirectToAction("Cart");
    }
}