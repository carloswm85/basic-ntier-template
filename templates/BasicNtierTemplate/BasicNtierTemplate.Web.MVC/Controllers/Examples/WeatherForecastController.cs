using BasicNtierTemplate.Web.MVC.Models.ViewModels.WeatherForecast;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.Web.MVC.Controllers.Examples
{
    /// <summary>
    /// Using primary constructor syntax for dependency injection
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public class WeatherForecastController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(IWeatherForecastService weatherForecastService, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        public async Task<IActionResult> Index()
        {
            var weatherViewModel = new WeatherViewModel();

            var weatherDataList = await _weatherForecastService.GetWeatherForecastExample();
            _logger.LogInformation("Fetched {Count} weather forecast entries.", weatherDataList?.Count ?? 0);
            weatherViewModel.WeatherList = weatherDataList;

            return View(weatherViewModel);
        }
    }
}