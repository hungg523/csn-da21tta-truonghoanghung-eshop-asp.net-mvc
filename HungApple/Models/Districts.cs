using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HungApple.Models
{
    [Table("Districts")]
    public class Districts
    {
        [Key]
        public int district_id { get; set; }
        public int province_id { get; set; }
        public string district_name { get; set; }
        public string type { get; set; }
    }
}
