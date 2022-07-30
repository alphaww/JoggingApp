using JoggingApp.Core;
using JoggingApp.Core.Clock;
using JoggingApp.Core.Crypto;
using JoggingApp.Core.Email;
using JoggingApp.Core.Templating;
using JoggingApp.Services;
using JoggingApp.Users;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Setup
{
    public static class ServiceSetup
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenWriter, JwtSecurityTokenWriter>();
            services.AddScoped<IHashService, MD5HashService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ITemplateRenderer, RazorTemplateRenderer>();
            services.AddScoped<IClock, SystemClock>();
            services.AddScoped<UserRegisteredEmailTemplateRenderer>();         
        }
    }
}
