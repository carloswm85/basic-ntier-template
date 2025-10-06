using MyCustomTemplate.Data.Entities;

namespace MyCustomTemplate.Repository
{
    public interface IUnitOfWork
    {
        void Save();
        void Dispose();
        void CustomExec(string sqlQuery);

        IRepository<Blog> BlogRepository { get; }
        IRepository<Post> PostRepository { get; }

    }
}
