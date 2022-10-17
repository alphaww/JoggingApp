using JoggingApp.EemailService;

var builder = WebApplication.CreateBuilder(args);
builder.AddEventBus();
var app = builder.Build();

app.Run();
