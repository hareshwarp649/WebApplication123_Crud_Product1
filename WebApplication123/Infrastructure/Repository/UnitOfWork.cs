using WebApplication123.Data;
using WebApplication123.Infrastructure.IRepository;

namespace WebApplication123.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private WebAppDbContext _context;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public UnitOfWork(WebAppDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
