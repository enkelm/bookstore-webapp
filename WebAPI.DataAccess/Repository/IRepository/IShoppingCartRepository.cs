using API.Models;

namespace API.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        Task<List<ShoppingCart>> GetByUser(string userId);
        void Update(ShoppingCart obj);
    }
}
