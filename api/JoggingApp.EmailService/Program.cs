using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.EemailService;
using JoggingApp.Users.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);
builder.AddEventBus();
var app = builder.Build();

var eventBus = app.Services.GetService<IEventBus>();

eventBus.Subscribe<UserRegisteredIntegrationEvent, UserRegisteredIntegrationEventHandler>();

app.Run();
