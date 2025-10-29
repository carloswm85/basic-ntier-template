using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Repository
{
    public class StoredProcedureRepository<T> : IStoredProcedureRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly string _query;

        public StoredProcedureRepository(DbContext context, string query)
        {
            _context = context;
            _query = query;
        }

        public StoredProcedureRepository()
        {
        }

        public IEnumerable<T> Exec(Dictionary<string, object> parameters)
        {
            var result = CreateCommandAndParameters(parameters);
            var command = $"{_query} {result.Command}";

            return _context.Database.SqlQueryRaw<T>(command, result.Parameters);
        }

        private class SQLQueryClass
        {
            public string Command { get; set; }

            public SqlParameter[] Parameters { get; set; }
        }

        private SQLQueryClass CreateCommandAndParameters(Dictionary<string, object> props)
        {
            var result = new SQLQueryClass();
            var lstSqlParameters = new List<SqlParameter>();
            var lstName = new List<string>();

            foreach (var key in props.Keys)
            {
                var propertyName = $"@{key}";
                var value = props[key];
                lstName.Add(propertyName);
                var sqlParameter = new SqlParameter(propertyName, value ?? DBNull.Value);
                lstSqlParameters.Add(sqlParameter);
            }

            result.Command = string.Join(", ", lstName);
            result.Parameters = lstSqlParameters.ToArray();
            return result;
        }
    }
}
