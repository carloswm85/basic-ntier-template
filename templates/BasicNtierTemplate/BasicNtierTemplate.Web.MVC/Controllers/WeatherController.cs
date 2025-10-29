using BasicNtierTemplate.Web.MVC.Models;
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
        private readonly IWeatherServices _weatherService;

        public WeatherController(IWeatherServices weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var weatherDataList = await _weatherService.GetWeatherForecasts();
                var weatherViewModel = new WeatherViewModel
                {
                    WeatherList = weatherDataList
                };

                return View(weatherViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred in HomeController.Index");
                return View("Error");
            }
        }
    }
}