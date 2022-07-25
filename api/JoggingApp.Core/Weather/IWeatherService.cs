namespace JoggingApp.Core.Weather
{
    public interface IWeatherService
    {
        Task<WeatherInfo> FetchWeatherInfo(Coordinates coordinates);
    }
}
