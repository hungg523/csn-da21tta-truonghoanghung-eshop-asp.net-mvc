using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HungApple.Models
{
	public class Comment
	{
		[Key]
		public int Id { get; set; }
        [System.ComponentModel.DisplayName("Bình luận")]
        public string? Content { get; set; }
        [System.ComponentModel.DisplayName("Đánh giá (sao)")]
        public int Rating { get; set; }
        [System.ComponentModel.DisplayName("Ngày đánh giá")]
        public DateTime DateCreated { get; set; }
		[ForeignKey("User")]
		public int UserId { get; set; }
		public virtual User? User { get; set; }
		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public virtual Product? Product { get; set; }
		[NotMapped]
        [System.ComponentModel.DisplayName("Tài khoản")]
        public string UserName { get; set; }
		[NotMapped]
        [System.ComponentModel.DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
    }
}
