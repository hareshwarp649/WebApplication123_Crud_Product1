using WebApplication123.Models;

namespace WebApplication123.Infrastructure.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {

        void Update(Product product);
    }
}
