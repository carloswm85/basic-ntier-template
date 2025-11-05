using BasicNtierTemplate.Web.MVC.Models.ViewModels;

namespace BasicNtierTemplate.Web.MVC.Services.Interfaces
{
    public interface IWeatherServiceExample
    {
        Task<List<WeatherForecast>> GetWeatherForecastExample();
    }
}
