using JoggingApp.Core.Weather;
using JoggingApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Setup
{
    public static class WeatherSerrviceSetup
    {
        public static void AddWeatherService(this IServiceCollection services)
        {
            services.AddTransient<IWeatherService, OpenWeatherService>();
        }
    }
}
