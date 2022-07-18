using JoggingApp.Core;
using JoggingApp.Core.Crypto;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Setup
{
    public static class ServiceSetup
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenWriter, JwtSecurityTokenWriter>();
            services.AddTransient<IHashService, MD5HashService>();
        }
    }
}
