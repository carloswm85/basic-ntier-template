using BasicNtierTemplate.Data.Model;

namespace BasicNtierTemplate.Repository
{
    public interface IUnitOfWork
    {
        void Save();
        Task<int> SaveAsync();
        void Dispose();
        void CustomExec(string sqlQuery);

        #region Examples

        IRepository<Blog> BlogRepository { get; }
        IRepository<Posteo> PostRepository { get; }

        IRepository<Student> StudentRepository { get; }
        IRepository<Course> CourseRepository { get; }
        IRepository<Enrollment> EnrollmentRepository { get; }

        #endregion
    }
}
