using System.Diagnostics;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Web.MVC.Services;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Web.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Debug.WriteLine("MVC: Starting application...");

                var builder = WebApplication.CreateBuilder(args);

                #region Services Addition

                // Build the configuration by locating the appsettings.json file
                // in the API project directory (1 directory level up from the current directory).
                var path = Path.Combine(Directory.GetCurrentDirectory(), "../BasicNtierTemplate.API");
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                // Connection string "BasicNtierTemplateConnection" is pulled from configuration (appsettings.json).
                var connectionString = configuration.GetConnectionString("BasicNtierTemplateConnection")
                ?? throw new InvalidOperationException("Connection string" + "'BasicNtierTemplateConnection' not found.");

                // Register DbContext with SQL Server as the database provider.
                builder.Services.AddDbContext<BasicNtierTemplateDbContext>(options =>
                    options.UseSqlServer(connectionString));

                // Configure HttpClient with base address from configuration
                builder.Services.AddHttpClient("ApiClient", client =>
                {
                    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
                    if (string.IsNullOrWhiteSpace(apiBaseUrl))
                    {
                        throw new InvalidOperationException("ApiBaseUrl configuration is missing or empty.");
                    }
                    client.BaseAddress = new Uri(apiBaseUrl);
                });

                builder.Services.AddRazorPages();

                builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

                builder.Services
                    .AddControllersWithViews()
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization()
                    .AddXmlDataContractSerializerFormatters();

                builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEF>();

                // Services from current project
                builder.Services.AddScoped<IWeatherServiceExample, WeatherServiceExample>();

                #endregion

                var app = builder.Build();

                #region Middleware Addition

                if (app.Environment.IsDevelopment())
                {
                    // Enable the Developer Exception Page in the development environment.
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    // Use a custom error handling page for production.
                    // app.UseExceptionHandler("/Home/Error");
                    app.UseExceptionHandler("/Error");

                    // REMEMBER THIS WORKS ONLY WITH:
                    // "ASPNETCORE_ENVIRONMENT": "Production"

                    // Returns error as plain text
                    // app.UseStatusCodePages(); // (1)

                    // Intersect error and return a view
                    // {0} is a placeholder for the status code
                    // app.UseStatusCodePagesWithRedirects("/Error/{0}"); // (2) Redirect to the string controller
                    app.UseStatusCodePagesWithReExecute("/Error/{0}"); // (3) Re-executes the pipeline


                    // Enable HTTP Strict Transport Security (HSTS) for enhanced security in production.
                    // The default duration is 30 days; you can adjust this value based on your requirements.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseRequestLocalization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                #endregion

                app.Run();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "MVC: Stopped program because of exception");
                throw;
            }
        }
    }
}
