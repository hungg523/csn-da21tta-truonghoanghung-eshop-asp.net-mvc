using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HungApple.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [System.ComponentModel.DisplayName("Tên danh mục")]
        public string? Name { get; set; }
        
        [NotMapped]        
        public int ProductCount { get; set; }
    }
}
