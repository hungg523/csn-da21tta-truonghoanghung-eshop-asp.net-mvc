using HungApple.Data;
using HungApple.Models;

namespace HungApple.Repositories
{
	public class CategoryRepository: ICategoryRepository
	{
		private readonly HungAppleContext _context;

		public CategoryRepository(HungAppleContext context)
		{
			_context = context;
		}

		public IEnumerable<Category> GetAllCategories()
		{
			var categories = _context.Category.ToList();

			foreach (var category in categories)
			{
				category.ProductCount = _context.Product.Count(p => p.CategoryId == category.Id);
			}

			return categories;
		}
	}
}
