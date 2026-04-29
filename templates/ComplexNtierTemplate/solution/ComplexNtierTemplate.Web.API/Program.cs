using ComplexNtierTemplate.Data.Datum;
using ComplexNtierTemplate.Data.Model;
using ComplexNtierTemplate.Web.API;

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
        var db = serviceProvider.GetRequiredService<ComplexNtierTemplateDbContext>();
        await DbInitializer.Initialize(serviceProvider);

        startupLogger.LogInformation("Database successfully initialized.");
    }
    catch (Exception ex)
    {
        startupLogger.LogError(ex, "Database initialization failed.");
    }
}

// ------------------------------------------------------------
app.Run();