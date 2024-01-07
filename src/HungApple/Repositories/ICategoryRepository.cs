using HungApple.Models;

namespace HungApple.Repositories
{
	public interface ICategoryRepository
	{
		IEnumerable<Category> GetAllCategories();
	}
}