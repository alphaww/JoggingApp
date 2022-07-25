using JoggingApp.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddStorage();
builder.Services.AddWeatherService();
builder.Services.AddHttpClient();
builder.Services.AddServices();
builder.Services.AddControllersWithViews();
builder.Services.AddValidators();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddJwtAuthentication();
builder.Services.AddSwaggerSetup();
builder.Services.AddCors();

var app = builder.Build();

//app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseCustomSwaggerConfig();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
