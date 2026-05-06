using ComplexNLayerTemplate.Web.MVC.Models.ViewModels.WeatherForecast;

namespace ComplexNLayerTemplate.Web.MVC.Services.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<List<WeatherForecast>?> GetWeatherForecastExample();
    }
}
