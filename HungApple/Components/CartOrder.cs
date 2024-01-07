using HungApple.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HungApple.Models
{
	public class CartOrder : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View(HttpContext.Session.GetJson<Cart>("cart"));
		}
	}
}
