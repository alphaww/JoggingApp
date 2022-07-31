using JoggingApp.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Setup
{
    public static class MigrateDatabaseSetup
    {
        public static void MigrateDatabase(this WebApplication app)
        {
            using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<JoggingAppDbContext>();
            context.Database.Migrate();
        }
    }
}
