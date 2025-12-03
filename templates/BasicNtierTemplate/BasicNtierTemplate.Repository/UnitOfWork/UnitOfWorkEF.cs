using BasicNtierTemplate.Data.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace BasicNtierTemplate.Repository
{
    public sealed class UnitOfWorkEF : IDisposable, IUnitOfWork
    {
        #region Private Fields

        private readonly BasicNtierTemplateDbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;

        private IRepository<Student>? _studentRepository;
        private IRepository<Course>? _courseRepository;
        private IRepository<Enrollment>? _enrollmentRepository;

        #endregion

        public UnitOfWorkEF(BasicNtierTemplateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Contoso University Example

        public IRepository<Student> StudentRepository => _studentRepository ?? (_studentRepository = new RepositoryEF<Student>(_dbContext));
        public IRepository<Course> CourseRepository => _courseRepository ?? (_courseRepository = new RepositoryEF<Course>(_dbContext));
        public IRepository<Enrollment> EnrollmentRepository => _enrollmentRepository ?? (_enrollmentRepository = new RepositoryEF<Enrollment>(_dbContext));

        #endregion


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
