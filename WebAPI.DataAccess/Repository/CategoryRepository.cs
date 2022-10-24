using API.DataAccess.Repository.IRepository;
using API.Models;

namespace API.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category entity)
        {
            dbSet.Update(entity);
        }
    }
}
