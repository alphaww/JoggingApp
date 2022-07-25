using JoggingApp.Core.Weather;
using JoggingApp.Infra;
using Xunit;

namespace JoggingApp.Tests
{
    public class WeatherServiceTest
    {
        private readonly IWeatherService _weatherService;

        public WeatherServiceTest(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [Fact]
        public async void WeatherService_Should_Return_Weather_Info_For_Zagreb()
        {
            var weatherInfo = await _weatherService.FetchWeatherInfo(new Coordinates { Latitude = 45.815399, Longitude = 15.966568 });
            Assert.NotNull(weatherInfo);
        }
    }
}
