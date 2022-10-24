using API.Models;

namespace API.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        public void Update(CoverType entity);
    }
}
