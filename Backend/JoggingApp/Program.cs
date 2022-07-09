using JoggingApp.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddStorage();
builder.Services.AddServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddJwtAuthentication();
builder.Services.AddSwaggerSetup();

var app = builder.Build();

//app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseCustomSwaggerConfig();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
