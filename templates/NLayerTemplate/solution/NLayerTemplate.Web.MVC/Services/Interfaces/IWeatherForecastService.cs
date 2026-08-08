using NLayerTemplate.Web.MVC.Models.ViewModels.WeatherForecast;

namespace NLayerTemplate.Web.MVC.Services.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<List<WeatherForecast>?> GetWeatherForecastExample();
    }
}
