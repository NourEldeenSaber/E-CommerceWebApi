using Domain.Entities;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        // Complete , SaveChangesAsync
        Task<int> SaveChangesAsync();

        //Return Obj of TEntity[Product,Order,...]
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
