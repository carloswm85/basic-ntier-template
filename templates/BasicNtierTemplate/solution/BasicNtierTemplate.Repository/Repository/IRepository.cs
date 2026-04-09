namespace BasicNtierTemplate.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // === Query root – exposes IQueryable for complex LINQ queries
        IQueryable<TEntity> Query();

        // === CRUD OPERATIONS ===
        Task<TEntity?> GetByIdAsync(params object[] keyValues);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
