using JoggingApp.Core.Weather;
using JoggingApp.Infra;
using Microsoft.AspNetCore.Builder;
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
