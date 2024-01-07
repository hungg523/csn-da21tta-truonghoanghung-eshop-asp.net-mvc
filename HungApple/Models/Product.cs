using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HungApple.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		[System.ComponentModel.DisplayName("Tên sản phẩm")]
		public string? Name { get; set; }

		[MaxLength(500)]
        [System.ComponentModel.DisplayName("Mô tả")]
        public string? Description { get; set; }

        [System.ComponentModel.DisplayName("Chi tiết sản phẩm")]
        public string? Detail { get; set; }

		[Required]
        [System.ComponentModel.DisplayName("Giá")]
        public decimal Price { get; set; }

		[Required]
		[System.ComponentModel.DisplayName("Giá giảm")]
        public decimal PriceDiscount { get; set; }
		[MaxLength(255)]
        [System.ComponentModel.DisplayName("Hình ảnh")]
        public string? Image { get; set; }

		[MaxLength(50)]
        [System.ComponentModel.DisplayName("Màu sắc")]
        public string? Color { get; set; }

        [System.ComponentModel.DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [System.ComponentModel.DisplayName("Sản phẩm mới")]
        public bool IsNew { get; set; }

        [System.ComponentModel.DisplayName("Sản phẩm được bán nhiều nhất")]
        public bool IsBestSeller { get; set; }

		[ForeignKey("Category")]
        public int CategoryId { get; set; }

        [System.ComponentModel.DisplayName("Loại sản phẩm")]
        public virtual Category? Category { get; set; }

        [System.ComponentModel.DisplayName("Sản phẩm được giảm giá")]
        public bool IsSale { get; set; }

    }
}
