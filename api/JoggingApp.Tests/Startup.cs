using FluentValidation;
using JoggingApp.Core;
using JoggingApp.Core.Crypto;
using JoggingApp.Core.Email;
using JoggingApp.Core.Jogs;
using JoggingApp.Core.Templating;
using JoggingApp.Core.Users;
using JoggingApp.Core.Weather;
using JoggingApp.EntityFramework;
using JoggingApp.Jogs;
using JoggingApp.Services;
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
            services.AddTransient<IEmailSender, FakeEmailSender>();
            services.AddTransient<IWeatherService, FakeWeatherService>();
            services.AddTransient<ITemplateRenderer, FakeTemplateRenderer>();
            services.AddTransient<IUserStorage, UserStorage>();
            services.AddTransient<IJogStorage, JogStorage>();

            services.AddTransient<IValidator<UserRegisterRequest>, UserRegisterRequestValidator>();
            services.AddTransient<IValidator<UserAuthRequest>, UserAuthRequestValidator>();


            services.AddTransient<UserRegisteredEmailTemplateRenderer>();

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
