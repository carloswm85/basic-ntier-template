using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NLayerTemplate.Data.Model
{
    public class NLayerTemplateDbContextFactory
        : IDesignTimeDbContextFactory<NLayerTemplateDbContext>
    {
        public NLayerTemplateDbContext CreateDbContext(string[] args)
        {
            var connectionString =
                Environment.GetEnvironmentVariable("EF_CONNECTION")
                ?? "Server=.;Database=NLayerTemplateDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<NLayerTemplateDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new NLayerTemplateDbContext(optionsBuilder.Options);
        }
    }
}
