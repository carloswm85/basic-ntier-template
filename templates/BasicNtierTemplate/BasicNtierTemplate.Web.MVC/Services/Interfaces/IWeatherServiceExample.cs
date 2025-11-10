using BasicNtierTemplate.Web.MVC.Models.ViewModels.WeatherForecast;

namespace BasicNtierTemplate.Web.MVC.Services.Interfaces
{
    public interface IWeatherServiceExample
    {
        Task<List<WeatherForecast>?> GetWeatherForecastExample();
    }
}
