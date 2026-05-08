using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ComplexNLayerTemplate.Data.Model
{
    public class ComplexNLayerTemplateDbContextFactory
        : IDesignTimeDbContextFactory<ComplexNLayerTemplateDbContext>
    {
        public ComplexNLayerTemplateDbContext CreateDbContext(string[] args)
        {
            var connectionString =
                Environment.GetEnvironmentVariable("EF_CONNECTION")
                ?? "Server=.;Database=ComplexNLayerTemplateDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<ComplexNLayerTemplateDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ComplexNLayerTemplateDbContext(optionsBuilder.Options);
        }
    }
}