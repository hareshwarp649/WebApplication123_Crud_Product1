using Microsoft.EntityFrameworkCore;
using WebApplication123.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApplication123.Data
{
    public class WebAppDbContext : IdentityDbContext
    {
        public WebAppDbContext(DbContextOptions<WebAppDbContext> options) : base(options)
        {
        }
        public DbSet <Category> Categories { get; set; }
        public DbSet <Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
