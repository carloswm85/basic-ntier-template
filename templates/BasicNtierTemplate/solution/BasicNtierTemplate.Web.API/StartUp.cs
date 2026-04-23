using System.Reflection;
using Asp.Versioning;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Mappings.ContosoUniversity;
using BasicNtierTemplate.Service.Services.ExampleServices;
using BasicNtierTemplate.Service.Services.ExampleServices.Interfaces;
using BasicNtierTemplate.Web.API.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace BasicNtierTemplate.Web.API;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // ------------------------------------------------------------
    // Service registration
    // ------------------------------------------------------------
    public void ConfigureServices(IServiceCollection services)
    {
        // Connection string validation (fail fast)
        var connectionString = Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'.");

        // DbContext
        services.AddDbContext<BasicNtierTemplateDbContext>(options =>
            options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));

        // Caching (optional, but can improve performance for certain scenarios)
        services.AddDistributedMemoryCache();

        services.AddResponseCaching(options =>
        {
            options.MaximumBodySize = 1024 * 1024; // 1 MiB
            options.UseCaseSensitivePaths = true;
        });

        #region Application Services

        // Services Layer
        services.AddScoped<IUnitOfWork, UnitOfWorkEF>();
        services.AddScoped<IContosoUniversityService, ContosoUniversityService>();

        #endregion

        // AutoMapper (assemblies only)
        services.AddAutoMapper(
            cfg => { },
            typeof(StudentProfile).Assembly,
            typeof(CourseProfile).Assembly,
            typeof(EnrollmentProfile).Assembly
        );

        // Controllers
        services.AddControllers(options =>
        {
            options.CacheProfiles.Add(CacheProfiles.Default10Sec, CacheProfiles.Profile10);
            options.CacheProfiles.Add(CacheProfiles.Default60Sec, CacheProfiles.Profile60);
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

        // OpenAPI
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/docs/configure-and-customize-swaggergen.md
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "BasicNtierTemplate API V1",
                Description = "An ASP.NET Core Web API for your resourses.",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example Use License",
                    Url = new Uri("https://example.com/license")
                }
            });

            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "BasicNtierTemplate API V2",
                Description = "An ASP.NET Core Web API for your resourses.",
                TermsOfService = new Uri("https://example.com/terms")
            });

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.CustomSchemaIds(t => t.FullName!.Replace("+", "."));


        });

        var apiVersioningBuilder = services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;

            // Enabling multiple versioning methods (optional, choose based on your needs)
            /* In this case: With query string versioning results:
             *      ?api-version=1.0 //IN QUERY STRING
             *      v{version} //IN PARAMETER PATH
             */
            //options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"));
        });
        apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV"; // e.g., "v1", "v2", etc.
            options.SubstituteApiVersionInUrl = true; // api/v{version}/resource

        });

        services.AddCors(options =>
        {
            options.AddPolicy(
                PolicyNames.AllowSpecificOrigin,
                builder =>
                {
                    builder.WithOrigins("*")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    // ------------------------------------------------------------
    // Middleware pipeline configuration
    // ------------------------------------------------------------
    public void ConfigurePipeline(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseStaticFiles();

        // Swagger
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");

                // Also, versions can be separated in folders by controller grouping
                /*  Controllers/
                 *    ├── V1/
                 *    │   └── ProductsController.cs
                 *    ├── V2/
                 *    │   └── ProductsController.cs
                 *   etc.
                 */
            });

            app.UseDeveloperExceptionPage();
        }
        else
        {
            // Optionally still expose JSON for machine clients
            app.UseSwagger();
            app.UseExceptionHandler();
        }

        app.UseRouting();

        // UseCors must be called before UseResponseCaching
        app.UseCors(PolicyNames.AllowSpecificOrigin);

        app.UseResponseCaching();

        app.Use(async (context, next) =>
        {
            context.Response.GetTypedHeaders().CacheControl =
                new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(10)
                };

            context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                new string[] { "Accept-Encoding" };

            await next();
        });

        app.UseEndpoints(endpoints =>
        {
            // Standard MVC controller routing (if you have API controllers)
            endpoints.MapControllers();

            // Fallback for unmatched endpoints
            endpoints.MapFallback(() => Results.NotFound("Endpoint not found."));
        });

    }
}