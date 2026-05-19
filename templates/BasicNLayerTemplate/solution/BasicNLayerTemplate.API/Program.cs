using BasicNLayerTemplate.Data.Datum;
using BasicNLayerTemplate.Data.Model;
using BasicNLayerTemplate.Web.API;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------
// Logging
// ------------------------------------------------------------
var logger = LoggerFactory
    .Create(logging =>
    {
        logging.AddConsole();
        logging.AddDebug();
    })
    .CreateLogger("Startup");

logger.LogInformation("API starting...");

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
        var db = serviceProvider.GetRequiredService<BasicNLayerTemplateDbContext>();
        await DbInitializer.Initialize(serviceProvider);

        logger.LogInformation("Database successfully initialized (Web API).");

        if (app.Environment.IsDevelopment())
        {
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var server = app.Services.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;

                foreach (var address in addresses ?? [])
                {
                    logger.LogInformation(
                        "Web API running at: {Address}/swagger/index.html",
                        address
                    );
                }
            });
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database initialization failed.");
    }
}

// ------------------------------------------------------------
app.Run();
