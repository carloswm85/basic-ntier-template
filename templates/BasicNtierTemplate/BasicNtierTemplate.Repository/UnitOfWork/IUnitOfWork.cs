using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        // === Persist all changes (SaveChanges)
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        // === EF transaction helpers
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        #region Contoso University Example

        IRepository<Student> StudentRepository { get; }
        IRepository<Course> CourseRepository { get; }
        IRepository<Enrollment> EnrollmentRepository { get; }

        #endregion
    }
}
