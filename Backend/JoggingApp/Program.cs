using FluentValidation;
using JoggingApp.Setup;
using JoggingApp.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.AddStorage();
builder.Services.AddServices();
builder.Services.AddControllers();
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
