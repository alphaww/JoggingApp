using JoggingApp;
using JoggingApp.Setup;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(AssemblyReference.Assembly);
builder.AddStorage();
builder.AddEventBus();
builder.AddOutboxProcessingEngine();
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

//This is not good security. This is for demo purposes since this is a poc.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase();

app.Run();


