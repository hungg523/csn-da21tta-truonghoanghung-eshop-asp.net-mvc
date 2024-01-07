using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HungApple.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [System.ComponentModel.DisplayName("Tên đăng nhập")]
        public string? Username { get; set; }

        [Required]
        [MaxLength(50)]
        [System.ComponentModel.DisplayName("Mật khẩu")]
        public string? Password { get; set; }
		
        [NotMapped]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [System.ComponentModel.DisplayName("Xác nhận mật khẩu")]
        public string? ConfirmPassword { get; set; }

		[MaxLength(100)]
        [System.ComponentModel.DisplayName("Địa chỉ Email")]
        public string? Email { get; set; }

        [MaxLength(10)]
        [System.ComponentModel.DisplayName("Số điện thoại")]
        public string? Phone { get; set; }

        [MaxLength(20)]
        [System.ComponentModel.DisplayName("Quyền")]
        public string? Role { get; set; }

		public string? ResetPasswordToken { get; set; }
		public DateTime? ResetPasswordExpiration { get; set; }
        public int ProvinceId { get; set; } //tỉnh/tp
		public int DistrictId { get; set; } //Quận/Huyện
		public int WardId { get; set; } //Phường/Xã
        public string? ImagePath { get; set; }
        [NotMapped]
        public string? ProvinceName { get; set; }
        [NotMapped]
        public string? DistrictName { get; set; }
        [NotMapped]
        public string? WardName { get; set; }

    }
}
