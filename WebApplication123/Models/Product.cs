using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication123.Models
{
    public class Product
    {
     
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public double Price { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
       

        //[ForeignKey("Categories")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public virtual Category Category { get; set; }

    }
}
