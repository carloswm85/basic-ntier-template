using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleMonolithTemplate.Monolith.Data.Persistence
{
    public class SimpleMonolithTemplateDbContextFactory
        : IDesignTimeDbContextFactory<SimpleMonolithTemplateDbContext>
    {
        public SimpleMonolithTemplateDbContext CreateDbContext(string[] args)
        {
            var connectionString =
                Environment.GetEnvironmentVariable("EF_CONNECTION")
                ?? "Server=.;Database=SimpleMonolithTemplateDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<SimpleMonolithTemplateDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SimpleMonolithTemplateDbContext(optionsBuilder.Options);
        }
    }
}
