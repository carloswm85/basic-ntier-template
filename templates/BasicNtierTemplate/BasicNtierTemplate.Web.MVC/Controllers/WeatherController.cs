using BasicNtierTemplate.Web.MVC.Models.ViewModels.WeatherForecast;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.Web.MVC.Controllers
{
    /// <summary>
    /// Using primary constructor syntax for dependency injection
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public class WeatherController : Controller
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly IWeatherServiceExample _weatherService;

        public WeatherController(IWeatherServiceExample weatherService, ILogger<WeatherController> logger)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            var weatherViewModel = new WeatherViewModel();

            var weatherDataList = await _weatherService.GetWeatherForecastExample();
            weatherViewModel.WeatherList = weatherDataList;

            return View(weatherViewModel);
        }
    }
}