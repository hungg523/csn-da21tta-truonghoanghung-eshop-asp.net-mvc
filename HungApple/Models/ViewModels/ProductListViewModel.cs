namespace HungApple.Models.ViewModels
{
	public class ProductListViewModel
	{
        public IEnumerable<Product> Product { get; set; }
		public PagingInfo PagingInfo { get; set; } = new PagingInfo();
	}
}
