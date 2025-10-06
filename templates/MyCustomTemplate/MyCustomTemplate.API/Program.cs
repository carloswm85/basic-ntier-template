using Microsoft.EntityFrameworkCore;
using MyCustomTemplate.Data.Tools;
using MyCustomTemplate.Repository;
using MyCustomTemplate.Service.Services;
using MyCustomTemplate.Service.Services.Interfaces;

namespace MyCustomTemplate.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a WebApplication builder to configure services and middleware
            var builder = WebApplication.CreateBuilder(args);

            #region Service addition

            // Register DbContext with SQL Server as the database provider.
            // Connection string "DefaultConnection" is pulled from configuration (appsettings.json).
            //builder.Services.AddDbContext<MyCustomTemplateContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string" + "'DefaultConnection' not found.");

            builder.Services.AddSingleton<ConnectionResource>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                return new ConnectionResource(connectionString);
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register application services for dependency injection.
            // IBlogService (interface) will be resolved to BlogService (implementation).
            builder.Services.AddScoped<IBlogService, BlogService>();

            // Add controller support (enables MVC-style controllers).
            builder.Services.AddControllers();

            // Register services required for minimal APIs (endpoint metadata exploration).
            builder.Services.AddEndpointsApiExplorer();

            // Register Swagger generator for API documentation.
            builder.Services.AddSwaggerGen();

            #endregion

            // Build the application with the configured services.
            var app = builder.Build();

            #region Middleware addition

            // Configure the HTTP request pipeline (middleware execution order).
            if (app.Environment.IsDevelopment())
            {
                // Show detailed error pages during development.
                // app.UseDeveloperExceptionPage(); // Not required after NET6

                // Enable Swagger UI in development for API testing and documentation.
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Redirect all HTTP requests to HTTPS for security.
            app.UseHttpsRedirection();

            // Enable authorization middleware (validates user access).
            app.UseAuthorization();

            // Map controllers to endpoints (routes controllers’ actions to HTTP requests).
            app.MapControllers();

            #endregion

            // Run the application and start listening for incoming HTTP requests.
            app.Run();
        }
    }
}
