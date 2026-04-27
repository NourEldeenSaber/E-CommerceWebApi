using Domain.Contracts;
using Domain.Entities;
using Presistence.Data;

namespace Presistence.Repositories
{
    public class GenericRepository<TEntity,TKey>
        : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        #region Constructor
        
        private readonly StoreDbContext _dbContext;
        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        } 
        
        #endregion

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
        => asNoTracking ? await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync() 
            : await _dbContext.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(TKey id)
        => await _dbContext.Set<TEntity>().FindAsync(id);
        public async Task AddAsync(TEntity entity)
        => await _dbContext.Set<TEntity>().AddAsync(entity);
        public void Update(TEntity entity)
        => _dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity)
        => _dbContext.Set<TEntity>().Remove(entity);



        #region Specifications

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications)
        =>  await SpecificationEvluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).ToListAsync();

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> specifications)
        => await SpecificationEvluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> specifications)
        => await SpecificationEvluator.CreateQuery(_dbContext.Set<TEntity>(), specifications).CountAsync();
        

        #endregion
    }
}
