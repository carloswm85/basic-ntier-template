namespace BasicNtierTemplate.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(bool cache = true);
        T GetById(object id, bool cache = true);
        void Add(T entity);
        void AddRange(IEnumerable<T> entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(Guid id);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
        void DeleteRangeByIds(IQueryable<int> entityIds);
    }
}
