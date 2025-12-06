using System.Diagnostics;
using BasicNtierTemplate.Data.Datum;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Mappings;
using BasicNtierTemplate.Service.Services;
using BasicNtierTemplate.Service.Services.Interfaces;
using BasicNtierTemplate.Web.MVC.Mappings;
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

                #region Services Configuration

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
                builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 3;
                    options.SignIn.RequireConfirmedEmail = true;
                    //options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                    // https://youtu.be/jHRWR36UC2s?list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
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

                builder.Services.AddRazorPages();

                builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

                builder.Services
                    .AddControllersWithViews()
                    .AddViewLocalization()
                    .AddDataAnnotationsLocalization()
                    .AddXmlDataContractSerializerFormatters();

                builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEF>();

                builder.Services.AddAutoMapper(
                    cfg => { },
                    typeof(ApplicationUserProfile).Assembly
                );

                // Services from current project
                builder.Services.AddScoped<IWeatherServiceExample, WeatherServiceExample>();

                // Services from external project
                builder.Services.AddScoped<IEmailService, EmailService>();
                builder.Services.AddScoped<IRegistrationService, RegistrationService>();
                builder.Services.AddScoped<IUserService, UserService>();

                builder.Services.AddAutoMapper(
                    cfg => { },
                    typeof(UserProfile).Assembly,
                    typeof(CourseProfile).Assembly,
                    typeof(EnrollmentProfile).Assembly
                );

                // Application services
                builder.Services.AddScoped<IContosoUniversityService, ContosoUniversityService>();

                // MVC Services
                builder.Services.AddScoped<IWeatherForecastService, WeatherForectastService>();

                #endregion

                var app = builder.Build();

                #region Middleware Configuration

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

                app.UseAuthorization();
                app.UseRequestLocalization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                // Add Contoso University test data to the database
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    try
                    {
                        var context = services.GetRequiredService<BasicNtierTemplateDbContext>();
                        DbInitializer.Initialize(context);
                        logger.LogDebug("DB successfully initialized from the MVC layer.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while seeding the database from the MVC layer.");
                    }
                }

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
