namespace BasicNtierTemplate.Web.MVC.Models
{
    public class WeatherViewModel
    {
        public List<WeatherForecast> WeatherList { get; set; } = [];
    }
    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string? Summary { get; set; }
    }
}
