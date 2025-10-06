using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyCustomTemplate.Data.Model
{
    /// <summary>
    /// Factory class used by Entity Framework Core at design time to create an instance 
    /// of <see cref="MyCustomTemplateContext"/>.
    /// </summary>
    /// <remarks>
    /// This class is typically used for design-time operations such as running EF Core migrations.
    /// </remarks>
    public class MyCustomTemplateContextFactory : IDesignTimeDbContextFactory<MyCustomTemplateContext>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MyCustomTemplateContext"/> with configuration 
        /// loaded from the API project's appsettings.json file.
        /// </summary>
        /// <param name="args">Command-line arguments (not used in this implementation).</param>
        /// <returns>A configured <see cref="MyCustomTemplateContext"/> instance.</returns>
        public MyCustomTemplateContext CreateDbContext(string[] args)
        {
            // Build the configuration by locating the appsettings.json file
            // in the API project directory (1 directory level up from the current directory).
            var path = Path.Combine(Directory.GetCurrentDirectory(), "../MyCustomTemplate.API");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // Create the DbContext options builder
            var optionsBuilder = new DbContextOptionsBuilder<MyCustomTemplateContext>();

            // Configure the DbContext to use SQL Server with the "DefaultConnection" connection string
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            // Return a new instance of the context with the configured options
            return new MyCustomTemplateContext(optionsBuilder.Options);
        }
    }
}
