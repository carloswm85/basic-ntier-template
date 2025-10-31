using System.Diagnostics;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Services;
using BasicNtierTemplate.Service.Services.Interfaces;
using BasicNtierTemplate.Web.MVC.Services;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
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

                // Configure Identity services
                builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<BasicNtierTemplateDbContext>()
                    .AddDefaultTokenProviders();

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

                // Add services to the container.
                builder.Services.AddRazorPages();

                builder.Services
                    .AddControllersWithViews()
                    .AddXmlDataContractSerializerFormatters();

                builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEF>();

                // Services from current project
                builder.Services.AddScoped<IWeatherService, WeatherService>();

                // Services from external project
                builder.Services.AddScoped<IEmailService, EmailService>();
                builder.Services.AddScoped<IRegistrationService, RegistrationService>();

                #endregion

                var app = builder.Build();

                #region Middleware Addition

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

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
