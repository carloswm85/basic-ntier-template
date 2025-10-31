using BasicNtierTemplate.Web.MVC.Models;

namespace BasicNtierTemplate.Web.MVC.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<List<WeatherForecast>> GetWeatherForecasts();
    }
}
