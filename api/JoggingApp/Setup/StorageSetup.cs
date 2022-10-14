using JoggingApp.Core;
using JoggingApp.Core.Jogs;
using JoggingApp.Core.Users;
using JoggingApp.EntityFramework;
using JoggingApp.EntityFramework.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Setup
{
    public static class StorageSetup
    {
        public static void AddStorage(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<DomainEventsHandlerInterceptor>();

            builder.Services.AddDbContext<JoggingAppDbContext>(
                (sp, optionsBuilder) =>
                {
                    var inteceptor = sp.GetService<DomainEventsHandlerInterceptor>();

                    optionsBuilder.UseSqlServer(builder.Configuration["ConnectionString:DefaultConnection"])
                        .AddInterceptors(inteceptor);
                });

            builder.Services.AddScoped<IUserStorage, UserStorage>();
            builder.Services.AddScoped<IJogStorage, JogStorage>();
        }
    }
}
