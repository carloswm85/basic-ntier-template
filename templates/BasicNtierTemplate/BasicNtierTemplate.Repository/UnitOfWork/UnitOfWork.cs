using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Data.Tools;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Repository
{
    public sealed class UnitOfWork : IDisposable, IUnitOfWork
    {
        #region Private Fields

        private readonly BasicNtierTemplateDbContext _dbContext;

        private bool _disposed = false;

        private IRepository<Blog>? _blogRepository;
        private IRepository<Posteo>? _postRepository;


        #endregion

        #region Dependency (Unity) Injection Constructor

        public UnitOfWork(ConnectionResource connection)
        {
            _dbContext = ConnectionHelper.CreateConnection(connection);
        }
        #endregion

        #region Public Methods

        public IRepository<Blog> BlogRepository => _blogRepository ?? (_blogRepository = new Repository<Blog>(_dbContext));
        public IRepository<Posteo> PostRepository => _postRepository ?? (_postRepository = new Repository<Posteo>(_dbContext));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void CustomExec(string sqlQuery)
        {
            _dbContext.Database.ExecuteSqlRaw(sqlQuery);
        }

        #endregion

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}
