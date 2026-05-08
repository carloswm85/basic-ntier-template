using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BasicNLayerTemplate.Data.Model
{
    public class BasicNLayerTemplateDbContextFactory
        : IDesignTimeDbContextFactory<BasicNLayerTemplateDbContext>
    {
        public BasicNLayerTemplateDbContext CreateDbContext(string[] args)
        {
            var connectionString =
                Environment.GetEnvironmentVariable("EF_CONNECTION")
                ?? "Server=.;Database=BasicNLayerTemplateDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<BasicNLayerTemplateDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BasicNLayerTemplateDbContext(optionsBuilder.Options);
        }
    }
}