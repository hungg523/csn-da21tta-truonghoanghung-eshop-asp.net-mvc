using HungApple.Data;
using HungApple.Infrastructure;
using HungApple.Models;
using HungApple.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using NuGet.Protocol.Core.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HungApple.Controllers
{
    public class CartController : Controller
    {
        public Cart? Cart { get; set; }
        private readonly HungAppleContext _context;

        public CartController(HungAppleContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Cart", HttpContext.Session.GetJson<Cart>("cart"));
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            Product? product = _context.Product
                .FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, quantity);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }
        public IActionResult AddToPayment(int id, int quantity = 1)
        {
            Product? product = _context.Product
                .FirstOrDefault(p => p.Id == id);
            var cart = new PaymentModel();
            if (product != null)
            {
                cart.ProductId = product.Id;
                cart.Quantity = quantity;
            }
            return RedirectToAction("Index", "Orders", cart);
        }
        public IActionResult DeFromCart(int id)
        {
            Product? product = _context.Product
                .FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, -1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }

        public IActionResult RemoveFromCart(int id)
        {
            Product? product = _context.Product
                .FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart");
                Cart.RemoveLine(product);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }
    }
}
