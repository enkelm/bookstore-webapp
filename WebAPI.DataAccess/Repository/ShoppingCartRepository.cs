using API.DataAccess.Repository.IRepository;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ApplicationDbContext _db { get; set; }
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart obj)
        {
            _db.ShoppingCarts.Update(obj);
        }

        public Task<List<ShoppingCart>> GetByUser(string userId)
        {
            //IQueryable<ShoppingCart> query = 
            return _db.ShoppingCarts.Where(cartItem => cartItem.ApplicationUserId == userId).ToListAsync();
        }
    }
}
