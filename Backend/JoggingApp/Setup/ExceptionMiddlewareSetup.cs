using Microsoft.AspNetCore.Builder;

namespace JoggingApp.Setup
{
    public static class ExceptionMiddlewareSetup
    {
        public static void UseExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
