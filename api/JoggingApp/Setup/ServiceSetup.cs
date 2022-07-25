using JoggingApp.Core;
using JoggingApp.Core.Crypto;
using JoggingApp.Core.Email;
using JoggingApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Setup
{
    public static class ServiceSetup
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenWriter, JwtSecurityTokenWriter>();
            services.AddTransient<IHashService, MD5HashService>();
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
