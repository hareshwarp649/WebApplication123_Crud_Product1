using Microsoft.EntityFrameworkCore;
using WebApplication123.Models;

namespace WebApplication123.Data
{
    public class WebAppDbContext : DbContext
    {
        public WebAppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet <Category> Categories { get; set; }
        public DbSet <Product> Products { get; set; }
    }
}
