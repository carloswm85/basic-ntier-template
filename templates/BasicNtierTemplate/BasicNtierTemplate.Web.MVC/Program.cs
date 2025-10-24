using BasicNtierTemplate.Web.MVC.Services;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;
using NLog;

namespace BasicNtierTemplate.Web.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
            logger.Debug("init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);

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

                builder.Services.AddScoped<IWeatherServices, WeatherServices>();

                var app = builder.Build();

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

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();

            }
        }
    }
}
