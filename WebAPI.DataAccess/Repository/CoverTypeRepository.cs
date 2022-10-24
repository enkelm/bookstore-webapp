using API.DataAccess.Repository.IRepository;
using API.Models;

namespace API.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CoverType entity)
        {
            dbSet.Update(entity);
        }
    }
}
