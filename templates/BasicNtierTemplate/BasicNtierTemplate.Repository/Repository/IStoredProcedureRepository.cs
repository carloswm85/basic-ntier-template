namespace BasicNtierTemplate.Repository
{
    public interface IStoredProcedureRepository<T> where T : class
    {
        IEnumerable<T> Exec(Dictionary<string, object> parameters);
    }
}
