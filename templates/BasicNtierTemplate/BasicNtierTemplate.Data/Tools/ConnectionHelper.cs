using System.Diagnostics;
using BasicNtierTemplate.Data.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Data.Tools
{
    /// <summary>
    /// Provides utility methods for creating database connections and connection strings.
    /// </summary>
    public class ConnectionHelper
    {
        /// <summary>
        /// Builds a connection string from the provided <see cref="ConnectionResource"/> details.
        /// </summary>
        /// <param name="connectionResource">
        /// An object containing connection information such as server, database, user credentials, 
        /// and connection string (if already defined).
        /// </param>
        /// <returns>
        /// A valid SQL Server connection string that can be used by Entity Framework Core.
        /// </returns>
        public static string? CreateConnectionString(ConnectionResource connectionResource, bool isManualAssemble = false)
        {
            if (connectionResource == null)
            {
                throw new ArgumentNullException(nameof(connectionResource));
            }

            // Optionally, you could use SqlConnectionStringBuilder to manually assemble the string:
            if (isManualAssemble)
            {
                const string appName = "EntityFrameworkCore";
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = connectionResource.DataSource,
                    InitialCatalog = connectionResource.InitialCatalog,
                    MultipleActiveResultSets = true,
                    IntegratedSecurity = false,
                    ApplicationName = appName,
                    UserID = connectionResource.UserId,
                    Password = connectionResource.Password
                };

                return sqlBuilder.ConnectionString;
            }

            // For simplicity, we return the provided connection string directly:
            return connectionResource.ConnectionString;
        }

        /// <summary>
        /// Creates a new instance of <see cref="BasicNtierTemplateDbContext"/> using the connection 
        /// information from the provided <see cref="ConnectionResource"/>.
        /// </summary>
        /// <param name="connectionResource">
        /// An object containing connection configuration details, including the connection string.
        /// </param>
        /// <returns>
        /// A fully configured <see cref="BasicNtierTemplateDbContext"/> ready for database operations.
        /// </returns>
        public static BasicNtierTemplateDbContext CreateConnection(ConnectionResource connectionResource)
        {
            // Generate the connection string from the given resource.
            string connectionString = CreateConnectionString(connectionResource)
                ?? throw new InvalidOperationException("Connection string cannot be null or empty.");

            // Log the connection string for debugging (avoid in production).
            Debug.WriteLine($"Connection String: {connectionString}");

            // Configure the EF Core DbContext options to use SQL Server.
            var optionsBuilder = new DbContextOptionsBuilder<BasicNtierTemplateDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Return a new context instance configured with the specified options.
            return new BasicNtierTemplateDbContext(optionsBuilder.Options);
        }
    }
}