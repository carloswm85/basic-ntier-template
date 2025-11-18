using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Repository
{
    public interface IUnitOfWork
    {
        void Save();
        Task<int> SaveChangesAsync();
        void Dispose();
        void CustomExec(string sqlQuery);

        #region Contoso University Example

        IRepository<Student> StudentRepository { get; }
        IRepository<Course> CourseRepository { get; }
        IRepository<Enrollment> EnrollmentRepository { get; }

        #endregion
    }
}
