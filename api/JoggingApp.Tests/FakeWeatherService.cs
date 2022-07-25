using JoggingApp.Core.Weather;

namespace JoggingApp.Tests
{
    public class FakeWeatherService : IWeatherService
    {
        public Task<WeatherInfo> FetchWeatherInfo(Coordinates coordinates)
        {
            return Task.FromResult(new WeatherInfo
            {
                Description = "test",
                FeelsLikeTemperature = 1.0f,
                Humidity = 1,
                Temperature = 1.0f,
                Location = "test",
                Pressure = 1
            });
        }
    }
}
