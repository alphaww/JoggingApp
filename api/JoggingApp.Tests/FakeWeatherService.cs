using JoggingApp.Core.Weather;

namespace JoggingApp.Tests
{
    public class FakeWeatherService : IWeatherService
    {
        public bool WasCalled { get; private set; }
        public Task<WeatherInfo> FetchWeatherInfo(Coordinates coordinates)
        {
            WasCalled = true;
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
