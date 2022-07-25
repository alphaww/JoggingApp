using FluentValidation;
using JoggingApp.Core;
using JoggingApp.Core.Crypto;
using JoggingApp.Core.Jogs;
using JoggingApp.Core.Users;
using JoggingApp.Core.Weather;
using JoggingApp.EntityFramework;
using JoggingApp.Infra;
using JoggingApp.Jogs;
using JoggingApp.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(x => BuildConfig());

            services.AddHttpClient();
            services.AddDbContext<JoggingAppDbContext>(options => options.UseInMemoryDatabase("testdb"));
            services.AddTransient<ITokenWriter, JwtSecurityTokenWriter>();
            services.AddTransient<IHashService, MD5HashService>();
            services.AddTransient<IUserStorage, UserStorage>();
            services.AddTransient<IJogStorage, JogStorage>();
            services.AddTransient<IWeatherService, OpenWeatherService>();

            services.AddScoped<IValidator<UserRegisterRequest>, UserRegisterRequestValidator>();
            services.AddScoped<IValidator<UserAuthRequest>, UserAuthRequestValidator>();

            services.AddTransient<UserController>();
            services.AddTransient<JogController>();
        }

        private IConfiguration BuildConfig()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }
    }
}
