using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HungApple.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
        [System.ComponentModel.DisplayName("Họ")]
        public string? FirstName { get; set; }
        [System.ComponentModel.DisplayName("Tên")]
        public string? LastName { get; set; }
        [System.ComponentModel.DisplayName("Địa chỉ")]
        public string? Address { get; set; }
		[ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
		[ForeignKey("Delivery")]
        [System.ComponentModel.DisplayName("Phương thức thanh toán")]
        public int DeliveryId {  get; set; }
		public virtual Payment? Delivery { get; set; }
		[ForeignKey("Provinces")]
		[System.ComponentModel.DisplayName("Tỉnh/TP")]
		public int ProvinceId { get; set; } //tỉnh/tp
		public virtual Provinces Provinces { get; set; }
		[ForeignKey("Districts")]
		[System.ComponentModel.DisplayName("Quận/Huyện")]
		public int DistrictId { get; set; } //Quận/Huyện
		public virtual Districts Districts { get; set; }
		[ForeignKey("Wards")]
		[System.ComponentModel.DisplayName("Phường/Xã")]
		public int WardId { get; set; } //Phường/Xã
		public virtual Wards Wards { get; set; }
	}
}
