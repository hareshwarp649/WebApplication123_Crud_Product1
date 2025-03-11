using WebApplication123.Models;

namespace WebApplication123.Infrastructure.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {

        void Update(Category category);
    }
}
