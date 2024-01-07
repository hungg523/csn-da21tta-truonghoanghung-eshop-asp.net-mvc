using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HungApple.Models
{
    [Table("Provinces")]
    public class Provinces
    {
        [Key]
        public int province_id { get; set; }
        public string province_name { get; set; }
        public string type { get; set; }
    }
}
