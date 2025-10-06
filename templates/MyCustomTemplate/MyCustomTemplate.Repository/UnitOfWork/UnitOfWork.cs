using Microsoft.EntityFrameworkCore;
using MyCustomTemplate.Data.Entities;
using MyCustomTemplate.Data.Tools;

namespace MyCustomTemplate.Repository
{
    public sealed class UnitOfWork : IDisposable, IUnitOfWork
    {
        #region Private Fields

        private readonly MyCustomTemplateContext _context;

        private bool _disposed = false;

        private IRepository<Blog> _blogRepository;
        private IRepository<Post> _postRepository;


        #endregion

        #region Dependency (Unity) Injection Constructor

        public UnitOfWork(ConnectionResource connection)
        {
            _context = ConnectionHelper.CreateConnection(connection);

            // Default command timeout within the EF connection string
            //_context.Database.CommandTimeout = 250;

            // This might speed up things a little aswell. Default is true
            //_context.Configuration.AutoDetectChangesEnabled = false;

            // As might this (if applicable for your situation). Default is true
            //_context.Configuration.ValidateOnSaveEnabled = false;
        }
        #endregion

        #region Public Methods

        public IRepository<Blog> BlogRepository => _blogRepository ?? (_blogRepository = new Repository<Blog>(_context));
        public IRepository<Post> PostRepository => _postRepository ?? (_postRepository = new Repository<Post>(_context));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void CustomExec(string sqlQuery)
        {
            _context.Database.ExecuteSqlRaw(sqlQuery);
        }

        #endregion

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}
