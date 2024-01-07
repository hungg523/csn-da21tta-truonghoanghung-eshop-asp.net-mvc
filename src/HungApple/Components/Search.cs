using HungApple.Data;
using Microsoft.AspNetCore.Mvc;

namespace HungApple.Models
{
	public class Search: ViewComponent
	{
		private readonly HungAppleContext _context;

		public Search(HungAppleContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			return View(_context.Category.ToList());
		}
	}
}
