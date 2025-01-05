using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication123.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Category")]
        public string Name { get; set; }
    }
}
