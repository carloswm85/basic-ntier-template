using BasicNLayerTemplate.Web.Models.ViewModels.WeatherForecast;

namespace BasicNLayerTemplate.Web.Services.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<List<WeatherForecast>?> GetWeatherForecastExample();
    }
}
