using System.Text.Json;
using BasicNtierTemplate.Web.MVC.Models;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;

namespace BasicNtierTemplate.Web.MVC.Services
{

    public class WeatherServices(IHttpClientFactory httpClientFactory, ILogger<WeatherServices> logger) : IWeatherServices
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient("ApiClient");
        private readonly ILogger<WeatherServices> _logger = logger;

        public async Task<List<WeatherForecast>> GetWeatherForecasts()
        {
            var response = await _client.GetAsync("weatherforecast");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            _logger.LogDebug("API response received successfully");

            // Deserialize to a typed model
            var watherDataList = JsonSerializer.Deserialize<List<WeatherForecast>>
                (content, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return watherDataList ?? [];
        }
    }
}
