using WebApplication123.Data;
using WebApplication123.Infrastructure.IRepository;
using WebApplication123.Models;

namespace WebApplication123.Infrastructure.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private WebAppDbContext _context;
        public CategoryRepository(WebAppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            //_context.Categories.Update(category);
            var categoryDB = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (categoryDB != null)
            {
                categoryDB.Name = category.Name;

            }
        }
    }
}
