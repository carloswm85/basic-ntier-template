using System.Text.Json;
using BasicNtierTemplate.Web.MVC.Models;
using BasicNtierTemplate.Web.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace BasicNtierTemplate.Web.MVC.Controllers
{
    /// <summary>
    /// Using primary constructor syntax for dependency injection
    /// </summary>
    /// <param name="httpClientFactory"></param>
    public class WeatherController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
                _logger.Error(ex, "Unexpected error occurred in HomeController.Index");
                return View("Error");
            }
        }
    }
}