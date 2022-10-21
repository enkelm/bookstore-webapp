using API.DataAccess.Repository.IRepository;

namespace API.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
    }
}
