using System.Diagnostics;
using System.Reflection;
using BasicNtierTemplate.Data.Datum;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Data.Model.Identity;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Mappings;
using BasicNtierTemplate.Service.Services;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Debug.WriteLine(">>> API: Starting application...");

                // Create a WebApplication builder to configure services and middleware
                var builder = WebApplication.CreateBuilder(args);

                #region Service addition

                // Connection string "BasicNtierTemplateConnection" is pulled from configuration (appsettings.json).
                var connectionString = builder.Configuration.GetConnectionString("BasicNtierTemplateConnection")
                ?? throw new InvalidOperationException("Connection string" + "'BasicNtierTemplateConnection' not found.");

                // Register DbContext with SQL Server as the database provider.
                builder.Services.AddDbContext<BasicNtierTemplateDbContext>(options =>
                    options
                    //.UseLazyLoadingProxies() // Enable lazy loading of navigation properties
                    .UseSqlServer(connectionString));

                // TODO https://youtu.be/kC9qrUcy2Js?list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&t=199
                builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
                {
                    // Password policy
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 2;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;

                    // Lockout
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.AllowedForNewUsers = true;

                    // User / Sign-in
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                    .AddEntityFrameworkStores<BasicNtierTemplateDbContext>()
                    .AddDefaultTokenProviders();

                // CORS Configuration to allow requests from frontend applications (for production, configure appropriately).
                /* Not required when running frontend and backend in the same solution with proper proxy setup
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowFrontend", policy =>
                    {
                        policy.WithOrigins("http://localhost:3000", "http://localhost:4200") // Your frontend URLs
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Important for cookies/auth
                    });
                });
                */

                builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEF>();

                builder.Services.AddAutoMapper(
                    cfg => { },
                    typeof(StudentProfile).Assembly,
                    typeof(CourseProfile).Assembly,
                    typeof(EnrollmentProfile).Assembly
                );

                // Register application services for dependency injection.
                builder.Services.AddScoped<IContosoUniversityService, ContosoUniversityService>(); // SAMPLE

                builder.Services.AddScoped<IEmailService, EmailService>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IRegistrationService, RegistrationService>();

                // Add controller support (enables MVC-style controllers).
                builder.Services.AddControllers().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

                // Register services required for minimal APIs (endpoint metadata exploration).
                builder.Services.AddEndpointsApiExplorer();

                // Register Swagger generator for API documentation.
                builder.Services.AddSwaggerGen(options =>
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    options.IncludeXmlComments(xmlPath);
                    options.CustomSchemaIds(type => type.FullName!.Replace("+", "."));
                });

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
                else
                {
                    app.UseExceptionHandler("/error");
                    app.UseStatusCodePagesWithReExecute("/error/{0}");
                }

                // Redirect all HTTP requests to HTTPS for security.
                app.UseHttpsRedirection();

                // Enable CORS using the defined policy to allow frontend access.
                // Not required when running frontend and backend in the same solution with proper proxy setup
                // app.UseCors("AllowFrontend");

                // Enable authorization middleware (validates user access).
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapIdentityApi<ApplicationUser>();

                // Map controllers to endpoints (routes controllers’ actions to HTTP requests).
                app.MapControllers();

                app.MapFallback(() => Results.NotFound("Endpoint not found."));

                // Add Contoso University test data to the database
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    try
                    {
                        var context = services.GetRequiredService<BasicNtierTemplateDbContext>();
                        DbInitializer.Initialize(context);
                        logger.LogDebug("DB successfully initialized from the API layer.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while seeding the database from the API layer.");
                    }
                }

                #endregion

                // Run the application and start listening for incoming HTTP requests.
                app.Run();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "API: Stopped program because of exception");
                throw;
            }
        }
    }
}
