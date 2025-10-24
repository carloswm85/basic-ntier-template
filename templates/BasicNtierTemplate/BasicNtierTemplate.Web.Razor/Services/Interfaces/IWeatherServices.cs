using BasicNtierTemplate.Web.Razor.Models;

namespace BasicNtierTemplate.Web.Razor.Services.Interfaces
{
    public interface IWeatherServices
    {
        Task<List<WeatherForecast>> GetWeatherForecasts();
    }
}
