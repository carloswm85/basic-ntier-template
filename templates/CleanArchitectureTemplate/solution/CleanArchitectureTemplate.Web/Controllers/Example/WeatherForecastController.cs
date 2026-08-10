using CleanArchitectureTemplate.Web.Models.ViewModels.WeatherForecast;
using CleanArchitectureTemplate.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Web.Controllers.Example;

/// <summary>
/// Using primary constructor syntax for dependency injection
/// </summary>
/// <param name="httpClientFactory"></param>
[AllowAnonymous]
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
        var viewModel = new WeatherViewModel();

        var weatherDataList = await _weatherForecastService.GetWeatherForecastExample();
        _logger.LogInformation(
            "Fetched {Count} weather forecast entries.",
            weatherDataList?.Count ?? 0
        );
        viewModel.WeatherList = weatherDataList;

        return View(viewModel);
    }

    public IActionResult ApiInstructions()
    {
        var instructions =
            @"
                == Weather Forecast API Instructions ==

                1. Run the BasicNLayerTemplace.API project using your favorite method. 
                    • In Visual Studio IDE, you can set both projects to run simultaneously at 'Configure Startup Projects' in the 'Search Feature' box.
                2. Run the BasicNLayerTemplace.Web at the same time.
                3. You should be able to see the forecast data when both are running.
                ";
        return Ok(instructions);
    }
}
