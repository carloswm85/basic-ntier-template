using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ComplexNtierTemplate.Data.Model
{
    public class ComplexNtierTemplateDbContextFactory
        : IDesignTimeDbContextFactory<ComplexNtierTemplateDbContext>
    {
        public ComplexNtierTemplateDbContext CreateDbContext(string[] args)
        {
            var connectionString =
                Environment.GetEnvironmentVariable("EF_CONNECTION")
                ?? "Server=.;Database=ComplexNtierTemplateDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<ComplexNtierTemplateDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ComplexNtierTemplateDbContext(optionsBuilder.Options);
        }
    }
}