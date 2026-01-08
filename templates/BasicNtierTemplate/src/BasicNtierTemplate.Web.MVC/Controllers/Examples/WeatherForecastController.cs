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

        public WeatherForecastController(
            IWeatherForecastService weatherForecastService,
            ILogger<WeatherForecastController> logger
        )
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

        public IActionResult ApiInstructions()
        {
            var instructions = @"
                == Weather Forecast API Instructions ==

                1. Run the BasicNtierTemplace.API project using your favorite method. 
                    • In Visual Studio IDE, you can set both projects to run simultaneously at 'Configure Startup Projects' in the 'Search Feature' box.
                2. Run the BasicNtierTemplace.Web.MVC at the same time.
                3. You should be able to see the forecast data when both are running.
                ";
            return Ok(instructions);
        }
    }
}