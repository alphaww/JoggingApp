using JoggingApp.Core.Weather;

namespace JoggingApp.Tests
{
    public class FakeWeatherService : IWeatherService
    {
        public bool WasCalled { get; private set; }
        public Task<WeatherInfo> FetchWeatherInfoAsync(Coordinates coordinates, CancellationToken cancellation = default)
        {
            WasCalled = true;
            if (coordinates is null 
                || (coordinates.Latitude == 0)
                || (coordinates.Longitude == 0))
            {
                throw new Exception("Unable to fetch weather info.");
            }
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
