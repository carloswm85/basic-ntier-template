using System.Diagnostics;

namespace SimpleMonolithTemplate.Module.Module;

public partial class Program
{
    protected Program() { }

    public static void Main(string[] args)
    {
        try
        {
            Debug.WriteLine("Starting application...");

            var host = CreateHostBuilder(args).Build();
            host.Run();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex, "Stopped application because of exception");
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
