using FluentValidation;
using JoggingApp.Jogs;
using JoggingApp.Users;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingApp.Setup
{
    public static class ValidatorsSetup
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserRegisterRequest>, UserRegisterRequestValidator>();
            services.AddScoped<IValidator<UserAuthRequest>, UserAuthRequestValidator>();
            services.AddScoped<IValidator<JogUpdateRequest>, JogUpdateRequestValidator>();
            services.AddScoped<IValidator<JogInsertRequest>, JogInsertRequestValidator>();
        }
    }
}
