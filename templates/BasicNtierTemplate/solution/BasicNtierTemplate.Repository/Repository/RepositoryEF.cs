using BasicNtierTemplate.Data;
using BasicNtierTemplate.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Repository
{
    //The reading is: It is a class, that takes a generic type, and implements an interface, AND this generic type is limited to be any class + entities interface.
    public class RepositoryEF<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        #region Private fields

        private readonly BasicNtierTemplateDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        #endregion

        #region Constructors

        public RepositoryEF(BasicNtierTemplateDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        #endregion

        #region Public Methods

        public IQueryable<TEntity> Query() => _dbSet.AsQueryable();

        public async Task<TEntity?> GetByIdAsync(params object[] keyValues)
            => await _dbSet.FindAsync(keyValues);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task AddAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
            => await _dbSet.AddRangeAsync(entities);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities)
            => _dbSet.UpdateRange(entities);

        public void Remove(TEntity entity)
            => _dbSet.Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities)
            => _dbSet.RemoveRange(entities);

        #endregion
    }
}
