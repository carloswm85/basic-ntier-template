namespace BasicNtierTemplate.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool asNoTracking = true);
        TEntity GetById(object id, bool asNoTracking = false);
        Task<TEntity> GetByIdAsync(object id, bool asNoTracking = false);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entity);
        void Update(TEntity entity);
        void Delete(int id);
        void Delete(Guid id);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entity);
        void DeleteRangeByIds(IQueryable<int> entityIds);
    }
}
