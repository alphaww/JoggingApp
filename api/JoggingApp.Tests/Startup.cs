using FluentValidation;
using JoggingApp.Core;
using JoggingApp.Core.Clock;
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
            services.AddScoped(x => BuildConfig());

            services.AddHttpClient();
            services.AddScoped(x => BuildDbContextWithTestData(x));
            services.AddScoped<ITokenWriter, JwtSecurityTokenWriter>();
            services.AddScoped<IHashService, MD5HashService>();
            services.AddScoped<IEmailSender, FakeEmailSender>();
            services.AddScoped<IWeatherService, FakeWeatherService>();
            services.AddScoped<ITemplateRenderer, FakeTemplateRenderer>();
            services.AddScoped<IClock>(x => new FakeClock(DateTime.UtcNow));
            services.AddScoped<IUserStorage, UserStorage>();
            services.AddScoped<IJogStorage, JogStorage>();

            services.AddScoped<IValidator<UserRegisterRequest>, UserRegisterRequestValidator>();
            services.AddScoped<IValidator<UserAuthRequest>, UserAuthRequestValidator>();
            services.AddScoped<IValidator<JogInsertRequest>, JogInsertRequestValidator>();
            services.AddScoped<IValidator<JogUpdateRequest>, JogUpdateRequestValidator>();

            services.AddScoped<UserRegisteredEmailTemplateRenderer>();

            services.AddScoped<UserController>();
            services.AddScoped<JogController>();
        }

        private IConfiguration BuildConfig()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }

        private JoggingAppDbContext BuildDbContextWithTestData(IServiceProvider sp)
        {
             var options = new DbContextOptionsBuilder<JoggingAppDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
             var context = new JoggingAppDbContext(options);

             var hashService = sp.GetService<IHashService>();
             var clock = sp.GetService<IClock>();

             var user1 = User.Create("testuser1@test.com", "?Testuser123.1", hashService, clock);
             var user2 = User.Create("testuser2@test.com", "?Testuser123.2", hashService, clock);

             context.Users.Add(user1);
             context.Users.Add(user2);


             context.Add(Jog.Create(user1.Id, 2000, new TimeSpan(10, 0, 0), null,
                new FakeClock(new DateTime(2022, 1, 1))));
             context.Add(Jog.Create(user1.Id, 1000, new TimeSpan(11, 15, 0), null,
                new FakeClock(new DateTime(2022, 2, 15))));
             context.Add(Jog.Create(user1.Id, 13000, new TimeSpan(15, 2, 24), null,
                new FakeClock(new DateTime(2022, 3, 20))));
             context.Add(Jog.Create(user1.Id, 3000, new TimeSpan(15, 2, 24), null,
                new FakeClock(new DateTime(2022, 6, 17))));

             context.Add(Jog.Create(user2.Id, 2000, new TimeSpan(10, 0, 0), null,
                new FakeClock(new DateTime(2022, 1, 1))));
             context.Add(Jog.Create(user2.Id, 1000, new TimeSpan(11, 15, 0), null,
                new FakeClock(new DateTime(2022, 2, 15))));
             context.Add(Jog.Create(user2.Id, 13000, new TimeSpan(15, 2, 24), null,
                new FakeClock(new DateTime(2022, 3, 20))));

            context.SaveChanges();

            return context;
        }
    }
}
