using SimpleMonolithTemplate.Module.ViewModels.WeatherForecast;

namespace SimpleMonolithTemplate.Module.Services.Interfaces.ExampleInterfaces;

public interface IWeatherForecastService
{
    Task<List<WeatherForecast>?> GetWeatherForecastExample();
}
