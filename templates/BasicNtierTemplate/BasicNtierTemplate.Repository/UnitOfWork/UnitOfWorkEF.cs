using BasicNtierTemplate.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Repository
{
    public sealed class UnitOfWorkEF : IDisposable, IUnitOfWork
    {
        #region Private Fields

        private readonly BasicNtierTemplateDbContext _dbContext;
        private bool _disposed = false;

        private IRepository<Student>? _studentRepository;
        private IRepository<Course>? _courseRepository;
        private IRepository<Enrollment>? _enrollmentRepository;

        #endregion

        public UnitOfWorkEF(BasicNtierTemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Contoso University Example

        public IRepository<Student> StudentRepository => _studentRepository ?? (_studentRepository = new Repository<Student>(_dbContext));
        public IRepository<Course> CourseRepository => _courseRepository ?? (_courseRepository = new Repository<Course>(_dbContext));
        public IRepository<Enrollment> EnrollmentRepository => _enrollmentRepository ?? (_enrollmentRepository = new Repository<Enrollment>(_dbContext));

        #endregion

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
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
