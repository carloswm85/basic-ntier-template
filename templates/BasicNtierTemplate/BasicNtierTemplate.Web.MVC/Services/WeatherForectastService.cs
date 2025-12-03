using System.Net.Sockets;
using System.Text.Json;
using BasicNtierTemplate.Web.MVC.Models.ViewModels.WeatherForecast;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;

namespace BasicNtierTemplate.Web.MVC.Services
{

    public class WeatherForectastService(IHttpClientFactory httpClientFactory, ILogger<WeatherForectastService> logger) : IWeatherForecastService
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient("ApiClient");
        private readonly ILogger<WeatherForectastService> _logger = logger;

        public async Task<List<WeatherForecast>?> GetWeatherForecastExample()
        {
            try
            {
                var response = await _client.GetAsync("api/WeatherForecasts/data");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogDebug("API response received successfully");

                // Deserialize to a typed model
                var watherDataList = JsonSerializer.Deserialize<List<WeatherForecast>>
                    (content, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return watherDataList ?? [];

            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException sock &&
                                       sock.SocketErrorCode == SocketError.ConnectionRefused)
            {
                Console.WriteLine($"Socket error: {ex.InnerException} - {ex.Message}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Socket error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred in WeatherServiceExample.GetWeatherForecastExample");
                throw;
            }
        }
    }
}
