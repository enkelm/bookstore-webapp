using API.Models;

namespace API.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public void Update(Category category);
    }
}
