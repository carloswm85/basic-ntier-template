using System.Net.Http;
using System.Text.Json;
using BasicNtierTemplate.Web.Razor.Models;
using BasicNtierTemplate.Web.Razor.Services.Interfaces;
using NLog;

namespace BasicNtierTemplate.Web.Razor.Services
{

    public class WeatherServices(IHttpClientFactory httpClientFactory) : IWeatherServices
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient("ApiClient");
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public async Task<List<WeatherForecast>> GetWeatherForecasts()
        {
            var response = await _client.GetAsync("weatherforecast");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            _logger.Debug("API response received successfully");

            // Deserialize to a typed model
            var watherDataList = JsonSerializer.Deserialize<List<WeatherForecast>>
                (content, options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return watherDataList ?? [];
        }
    }
}
