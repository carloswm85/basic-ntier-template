using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class StoredProcedureRepository<TEntity> : IStoredProcedureRepository<TEntity>
    where TEntity : class
{
    private readonly BasicNtierTemplateDbContext _dbContext;

    public StoredProcedureRepository(BasicNtierTemplateDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<TEntity>> ExecuteAsync(
        string procedureName,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        ValidateProcedureName(procedureName);

        var (sql, sqlParams) = BuildSql(procedureName, parameters);

        return await _dbContext.Database
            .SqlQueryRaw<TEntity>(sql, sqlParams)
            .ToListAsync(cancellationToken);
    }

    private static void ValidateProcedureName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Procedure name cannot be null or empty", nameof(name));

        // Allow only alphanumeric, underscore, and dot (for schema.procname)
        if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[\w\.]+$"))
            throw new ArgumentException("Invalid procedure name format", nameof(name));
    }

    private static (string sql, SqlParameter[] sqlParams) BuildSql(
        string procName,
        Dictionary<string, object> parameters)
    {
        if (parameters == null || parameters.Count == 0)
            return ($"EXEC {procName}", Array.Empty<SqlParameter>());

        var sqlParams = new List<SqlParameter>();
        var placeholders = new List<string>();

        foreach (var kvp in parameters)
        {
            var paramName = kvp.Key.StartsWith("@") ? kvp.Key : $"@{kvp.Key}";
            placeholders.Add(paramName);
            sqlParams.Add(new SqlParameter(paramName, kvp.Value ?? DBNull.Value));
        }

        var joined = string.Join(", ", placeholders);
        return ($"EXEC {procName} {joined}", sqlParams.ToArray());
    }
}
