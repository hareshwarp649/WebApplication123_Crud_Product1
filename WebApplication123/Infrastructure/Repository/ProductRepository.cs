using WebApplication123.Data;
using WebApplication123.Infrastructure.IRepository;
using WebApplication123.Models;

namespace WebApplication123.Infrastructure.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private WebAppDbContext _context;
        public ProductRepository(WebAppDbContext context) : base(context)
        {
            _context = context;
        }


        public void Update(Product product)
        {
            var productDB = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (productDB != null)
            {
                productDB.Name = product.Name;
                productDB.Description = product.Description;
                productDB.Price = product.Price;    
                if(product.ImageUrl != null)
                {
                    productDB.ImageUrl = product.ImageUrl;
                }

            }
        }
    }
}
