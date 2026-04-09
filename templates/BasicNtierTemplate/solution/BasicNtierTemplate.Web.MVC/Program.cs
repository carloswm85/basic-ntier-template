using System.Diagnostics;

namespace BasicNtierTemplate.Web.MVC;

public partial class Program
{
    protected Program() { }

    public static void Main(string[] args)
    {
        try
        {
            Debug.WriteLine("MVC: Starting application...");

            var host = CreateHostBuilder(args).Build();
            host.Run();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex, "MVC: Stopped program because of exception");
            throw;
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<StartUp>();
            });
}
