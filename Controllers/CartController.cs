using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

public class CartController : Controller
{
    private readonly WebDbContext _context;

    public CartController(WebDbContext context)
    {
        _context = context;
    }

    // VIEW CART
    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    // ADD TO CART
    public IActionResult AddToCart(int productId)
    {
        var product = _context.tb_Product.FirstOrDefault(p => p.ProductID == productId);

        if (product == null) return NotFound();

        var cart = GetCart();

        var existingItem = cart.FirstOrDefault(x => x.ProductID == productId);

        if (existingItem == null)
        {
            cart.Add(new CartItems
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Image = product.Image,
                Price = product.Price,
                Quantity = 1
            });
        }
        else
        {
            existingItem.Quantity++;
        }

        SaveCart(cart);

        return RedirectToAction("Index");
    }

    // REMOVE
    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.ProductID == id);

        if (item != null)
        {
            cart.Remove(item);
        }

        SaveCart(cart);

        return RedirectToAction("Index");
    }

    // GET SESSION
    private List<CartItems> GetCart()
    {
        var session = HttpContext.Session.GetString("Cart");
        return session != null
            ? JsonConvert.DeserializeObject<List<CartItems>>(session)
            : new List<CartItems>();
    }

    // SAVE SESSION
    private void SaveCart(List<CartItems> cart)
    {
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
    }
}