using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HungApple.Models
{
    [Table("Wards")]
    public class Wards
    {
        [Key]
        public int ward_id { get; set; }
        public int district_id { get; set; }
        public string ward_name { get; set; }
        public string type { get; set; }
    }
}
