using Domain.Contracts;
using Domain.Entities;
using Presistence.Data;
using System.Collections.Concurrent;

namespace Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private ConcurrentDictionary<string,object> _repositories ;
        public UnitOfWork(StoreDbContext dbContext)
        {
           _dbContext = dbContext;
            _repositories = new();
        }

        // Get or create cached repository instance for the entity.
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
            => (IGenericRepository<TEntity, TKey>) _repositories.
            GetOrAdd(typeof(TEntity).Name, (_) => new GenericRepository<TEntity, TKey>(_dbContext));

        public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
    }
}
