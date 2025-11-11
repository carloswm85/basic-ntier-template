using BasicNtierTemplate.Web.MVC.Models.ViewModels.WeatherForecast;

namespace BasicNtierTemplate.Web.MVC.Services.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<List<WeatherForecast>?> GetWeatherForecastExample();
    }
}
