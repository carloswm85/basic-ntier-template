using ComplexNtierTemplate.Web.MVC.Models.ViewModels.WeatherForecast;

namespace ComplexNtierTemplate.Web.MVC.Services.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<List<WeatherForecast>?> GetWeatherForecastExample();
    }
}
