using BasicNtierTemplate.Data;
using BasicNtierTemplate.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Repository
{
    //The reading is: It is a class, that takes a generic type, and implements an interface, AND this generic type is limited to be any class + entities interface.
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        #region Private fields

        private readonly BasicNtierTemplateDbContext _context;
        private readonly DbSet<TEntity> _entities;

        #endregion

        #region Constructors

        // TODO should DbContext be changed to BasicNtierTemplateDbContext?
        public Repository(BasicNtierTemplateDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        #endregion

        #region Public Methods: GetAll, GetById, Add, Delete, Delete, Update

        // TODO should I improve these methods?

        public virtual IQueryable<TEntity> GetAll(bool cache = true)
        {
            if (!cache)
            {
                return _entities.AsNoTracking<TEntity>();
            }

            return _entities;
        }

        public virtual TEntity GetById(object id, bool cache = true)
        {
            if (!cache)
            {
                _entities.AsNoTracking<TEntity>();
            }

            if (id.GetType().IsGenericType)
            {
                var arrayKeys = new List<object>();

                for (int i = 0; i < id.GetType().GetProperties().Length; i++)
                {
                    arrayKeys.Add(id.GetType().GetProperties()[i].GetValue(id));
                }

                return _entities.Find(arrayKeys.ToArray());
            }

            return _entities.Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id, bool cache = true)
        {
            if (!cache)
            {
                _entities.AsNoTracking<TEntity>();
            }

            if (id.GetType().IsGenericType)
            {
                var arrayKeys = new List<object>();

                for (int i = 0; i < id.GetType().GetProperties().Length; i++)
                {
                    arrayKeys.Add(id.GetType().GetProperties()[i].GetValue(id));
                }

                return _entities.Find(arrayKeys.ToArray());
            }

            return _entities.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            _entities.Add(entity);
        }
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public virtual void Delete(int id)
        {
            TEntity entity = _entities.Find(id);

            Delete(entity);
        }

        public virtual void Delete(Guid id)
        {
            TEntity entity = _entities.Find(id);

            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _entities.Attach(entity);
            }

            _entities.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            var key = entity.ID;

            if (entry.State == EntityState.Detached)
            {
                var currentEntry = _entities.Find(key);
                if (currentEntry != null)
                {
                    var attachedEntry = _context.Entry(currentEntry);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    _entities.Attach(entity);
                    entry.State = EntityState.Modified;
                }
            }
        }


        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            entities = entities.Where(item => item.ID != null);

            _entities.RemoveRange(entities);
        }

        public void DeleteRangeByIds(IQueryable<int> entityIds)
        {
            if (entityIds == null || !entityIds.Any())
                return;

            var idList = entityIds.ToList();

            // Get name of 'entityIds' table
            var tableName = _context.Entry(Activator.CreateInstance<TEntity>()).Entity.GetType().Name;

            // Create comma-separated list of IDs
            var idString = string.Join(",", idList.Select(id => id.ToString()));

            var sql = $"DELETE FROM {tableName} WHERE id IN ({idString})";
            _context.Database.ExecuteSqlRaw(sql);
        }

        #endregion
    }
}
