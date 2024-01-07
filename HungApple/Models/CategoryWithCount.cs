namespace HungApple.Models
{
    public class CategoryWithCount
    {
        public Category? Category { get; set; }
        public int ProductCount { get; set; }
        public Product? Product { get; set; }
    }
}
