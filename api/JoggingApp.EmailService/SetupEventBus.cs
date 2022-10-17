using JoggingApp.BuildingBlocks.EventBus;
using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.BuildingBlocks.EventBusRabbitMQ;
using JoggingApp.Users.IntegrationEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace JoggingApp.EemailService
{
    public static class EventBusSetup
    {
        public static void AddEventBus(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                var factory = new ConnectionFactory()
                {
                    HostName = builder.Configuration["EventBus:Connection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(builder.Configuration["EventBus:UserName"]))
                {
                    factory.UserName = builder.Configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrEmpty(builder.Configuration["EventBus:Password"]))
                {
                    factory.Password = builder.Configuration["EventBus:Password"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(builder.Configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(builder.Configuration["EventBus:RetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = builder.Configuration["EventBus:SubscriptionClientName"];
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<IServiceScopeFactory>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(builder.Configuration["EventBus:RetryCount"]))
                {
                    retryCount = int.Parse(builder.Configuration["EventBus:RetryCount"]);
                }

                var eventBus = new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope,
                    eventBusSubcriptionsManager, subscriptionClientName, retryCount);

                return eventBus;
            });
        }
    }
}
