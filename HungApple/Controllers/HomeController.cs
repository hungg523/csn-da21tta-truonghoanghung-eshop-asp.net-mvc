using HungApple.Data;
using HungApple.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HungApple.Controllers
{
    public class HomeController : Controller
    {
		private readonly HungAppleContext _context;

		public HomeController(HungAppleContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            return View(_context.Product.Include(p => p.Category).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AdminHome()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}