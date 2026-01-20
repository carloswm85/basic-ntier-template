using BasicNtierTemplate.Data.Datum;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Mappings.ContosoUniversity;
using BasicNtierTemplate.Service.Services;
using BasicNtierTemplate.Service.Services.Interfaces;
using BasicNtierTemplate.Web.MVC.Services;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Web.MVC
{
    public class StartUp
    {
        public StartUp(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Services Configuration

            // Connection string "BasicNtierTemplateConnection" is pulled from configuration (appsettings.json).
            var connectionString = Configuration.GetConnectionString("BasicNtierTemplateConnection")
                ?? throw new InvalidOperationException("Connection string 'BasicNtierTemplateConnection' not found.");

            // Register DbContext with SQL Server as the database provider.
            services.AddDbContext<BasicNtierTemplateDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure HttpClient with base address from configuration
            services.AddHttpClient("ApiClient", (provider, client) =>
            {
                var apiBaseUrl = Configuration["ApiBaseUrl"];
                if (string.IsNullOrWhiteSpace(apiBaseUrl))
                {
                    throw new InvalidOperationException("ApiBaseUrl configuration is missing or empty.");
                }
                client.BaseAddress = new Uri(apiBaseUrl);
            });


            services.AddRazorPages();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services
                .AddControllersWithViews()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization()
                .AddXmlDataContractSerializerFormatters();

            services.AddScoped<IUnitOfWork, UnitOfWorkEF>();

            services.AddAutoMapper(
                cfg => { },
                typeof(StudentProfile).Assembly,
                typeof(CourseProfile).Assembly,
                typeof(EnrollmentProfile).Assembly
            );

            // Application services
            services.AddScoped<IContosoUniversityService, ContosoUniversityService>();

            // MVC Services
            services.AddScoped<IWeatherForecastService, WeatherForectastService>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            #region Middleware Configuration

            if (env.IsDevelopment())
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            // Add Contoso University test data to the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<StartUp>>();
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
        }
    }
}