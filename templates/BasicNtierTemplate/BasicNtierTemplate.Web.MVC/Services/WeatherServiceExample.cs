using System.Text.Json;
using BasicNtierTemplate.Web.MVC.Models.ViewModels.WeatherForecast;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;

namespace BasicNtierTemplate.Web.MVC.Services
{

    public class WeatherServiceExample(IHttpClientFactory httpClientFactory, ILogger<WeatherServiceExample> logger) : IWeatherServiceExample
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient("ApiClient");
        private readonly ILogger<WeatherServiceExample> _logger = logger;

        public async Task<List<WeatherForecast>> GetWeatherForecastExample()
        {
            var response = await _client.GetAsync("weatherforecasts");
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
