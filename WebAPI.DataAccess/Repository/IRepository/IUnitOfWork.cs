namespace API.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }

        Task<bool> SaveChangesAsync();
    }
}
