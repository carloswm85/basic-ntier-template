using BasicNtierTemplate.Data.Constants;
using BasicNtierTemplate.Data.Datum;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Web.API;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------
// Logging
// ------------------------------------------------------------
var startupLogger = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
}).CreateLogger("Startup");

startupLogger.LogInformation("API starting...");

// ------------------------------------------------------------
// Build app & middleware
// ------------------------------------------------------------

// Configure services
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure middleware pipeline
startup.ConfigurePipeline(app, app.Environment);

// ------------------------------------------------------------
// Database seeding (with optional migrations)
// ------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    try
    {
        var db = serviceProvider.GetRequiredService<BasicNtierTemplateDbContext>();

        // Set password with the Secret Manager tool.
        //  `dotnet user-secrets set "SeedUserPW" "!Abc123Antartica" --project .\src\BasicNtierTemplate.Web.MVC\`
        var testUserPw = app.Configuration.GetValue<string>("SeedUserPW") ?? ApplicationConstants.TestPassword;
        await DbInitializer.Initialize(serviceProvider, testUserPw);

        startupLogger.LogInformation("Database successfully initialized.");
    }
    catch (Exception ex)
    {
        startupLogger.LogError(ex, "Database initialization failed.");
    }
}

// ------------------------------------------------------------
app.Run();