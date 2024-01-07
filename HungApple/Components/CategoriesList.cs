using HungApple.Data;
using Microsoft.AspNetCore.Mvc;

namespace HungApple.Models
{
	public class CategoriesList: ViewComponent
	{
		private readonly HungAppleContext _context;

		public CategoriesList(HungAppleContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			return View(_context.Category.ToList());
		}
	}
}
