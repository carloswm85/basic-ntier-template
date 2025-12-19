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
using Microsoft.AspNetCore.Authentication.Cookies;
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

                // === EF CORE CONFIGURATION ===
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

                // === IDENTITY API CONFIGURATION ===
                // Configure Identity services (IdentityOptions) and Entity Framework stores.
                // Calls: AddIdentity, AddDefaultUI,AddDefaultTokenProviders
                builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    // Default Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Default SignIn settings
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    // Default Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // Default User settings
                    options.User.AllowedUserNameCharacters =
                            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = false;

                })
                    .AddEntityFrameworkStores<BasicNtierTemplateDbContext>();

                // Cookie settings
                builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.Cookie.Name = "YourAppCookieName";
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.LoginPath = "/Identity/Account/Login";
                    // ReturnUrlParameter requires 
                    //using Microsoft.AspNetCore.Authentication.Cookies;
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                    options.SlidingExpiration = true;
                });

                // Force Identity's security stamp to be validated every minute.
                builder.Services.Configure<SecurityStampValidatorOptions>(o =>
                                   o.ValidationInterval = TimeSpan.FromMinutes(1));


                // === HTTP CLIENT CONFIGURATION ===
                // Configure HttpClient with base address from configuration setting
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

                // Services from external project
                builder.Services.AddScoped<IEmailService, EmailService>();
                builder.Services.AddScoped<IRegistrationService, RegistrationService>();
                builder.Services.AddScoped<IUserService, UserService>();

                builder.Services.AddAutoMapper(
                    cfg => { },
                    typeof(ApplicationUserProfile).Assembly,
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

                app.UseAuthentication();
                app.UseAuthorization();
                app.UseRequestLocalization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                app.MapRazorPages();

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
                // Use proper logging
                var logger = LoggerFactory.Create(config =>
                {
                    config.AddConsole();
                }).CreateLogger<Program>();

                logger.LogCritical(ex, "MVC: Application failed to start");
                throw;
            }
        }
    }
}
