using HungApple.Data;
using Microsoft.AspNetCore.Mvc;

namespace HungApple.Models
{
	public class Navbar: ViewComponent
	{
		private readonly HungAppleContext _context;

		public Navbar(HungAppleContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			return View(_context.Category.ToList());
		}
	}
}
