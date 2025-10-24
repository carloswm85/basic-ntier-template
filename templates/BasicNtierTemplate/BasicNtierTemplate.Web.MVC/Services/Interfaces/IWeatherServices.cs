using BasicNtierTemplate.Web.MVC.Models;

namespace BasicNtierTemplate.Web.MVC.Services.Interfaces
{
    public interface IWeatherServices
    {
        Task<List<WeatherForecast>> GetWeatherForecasts();
    }
}
