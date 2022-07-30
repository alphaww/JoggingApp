namespace JoggingApp.Core.Weather
{
    public interface IWeatherService
    {
        Task<WeatherInfo> FetchWeatherInfoAsync(Coordinates coordinates, CancellationToken cancellation = default);
    }
}
