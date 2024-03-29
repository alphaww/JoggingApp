﻿using JoggingApp.Core.Weather;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Services
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public OpenWeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<WeatherInfo> FetchWeatherInfoAsync(Coordinates coordinates, CancellationToken cancellation = default)
        {
            var appId = _configuration["OpenWeather:AppId"];
            var url = _configuration["OpenWeather:Url"];
            var response = await _httpClient.GetAsync($"{url}?lat={coordinates.Latitude}&AppId={appId}&lon={coordinates.Longitude}&units=metric", cancellation);
            var content = await response.Content.ReadAsStringAsync(cancellation);
            var obj = JsonConvert.DeserializeObject<JObject>(content);

            return new WeatherInfo
            {
                Description = obj.SelectToken("weather[0].description").Value<string>(),
                Location = obj.SelectToken("name").Value<string>(),
                Temperature = obj.SelectToken("main.temp").Value<float>(),
                FeelsLikeTemperature = obj.SelectToken("main.feels_like").Value<float>()
            };
        }
    }
}
