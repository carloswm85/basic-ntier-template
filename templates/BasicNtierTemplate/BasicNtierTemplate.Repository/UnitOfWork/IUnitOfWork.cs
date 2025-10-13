using BasicNtierTemplate.Data.Entities;

namespace BasicNtierTemplate.Repository
{
    public interface IUnitOfWork
    {
        void Save();
        void Dispose();
        void CustomExec(string sqlQuery);

        IRepository<Blog> BlogRepository { get; }
        IRepository<Posteo> PostRepository { get; }

    }
}
