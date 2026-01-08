namespace BasicNtierTemplate.Repository
{
    public interface IStoredProcedureRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> ExecuteAsync(
            string procedureName,
            Dictionary<string, object> parameters,
            CancellationToken cancellationToken = default);
    }
}
