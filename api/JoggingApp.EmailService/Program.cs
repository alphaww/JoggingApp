using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.EemailService;
using JoggingApp.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);
builder.AddEventBus();
var app = builder.Build();

var eventBus = app.Services.GetService<IEventBus>();

eventBus.Subscribe<SendEmailIntegrationEvent, SendEmailIntegrationEventHandler>();

app.Run();
