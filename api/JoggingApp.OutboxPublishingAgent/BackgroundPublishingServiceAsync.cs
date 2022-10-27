using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.Core;
using JoggingApp.Core.Outbox;
using JoggingApp.Core.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Quartz;

namespace JoggingApp.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class BackgroundPublishingServiceAsync : IJob
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BackgroundPublishingServiceAsync> _logger;

        private static int _executionId = 0;
        private static DateTime _lastFiredTime = DateTime.MinValue;

        public BackgroundPublishingServiceAsync(IServiceScopeFactory scopeFactory, ILogger<BackgroundPublishingServiceAsync> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _executionId++;

                if (_lastFiredTime.AddMilliseconds(250) > DateTime.UtcNow)
                {
                    _logger.LogInformation($"Dequeueing {_executionId}");
                    return;
                }

                _logger.LogInformation($"{_executionId} :: Processing Outbox {DateTime.Now}...");

           
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IOutboxStorage>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<BackgroundPublishingServiceAsync>>(); 

                var processableOutboxEvents = await repository.MarkAsTransitAndFetchReadyOutboxEventsAsync();
                
                foreach (var @event in processableOutboxEvents)
                {
                    HandleEvent(@event, context.CancellationToken)
                        .SafeFireAndForget(onException: (ex) => _logger.LogError(ex.ToString()) );
                }

                _logger.LogInformation($"  {_executionId} :: Process Outbox Completed {DateTime.Now}");
                _lastFiredTime = DateTime.UtcNow;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public async Task HandleEvent(OutboxMessage message, CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            var repository = scope.ServiceProvider.GetRequiredService<IOutboxStorage>();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var @event = JsonConvert.DeserializeObject<IEvent>(message.Content, JsonSerializerSettings);

            if (@event is null)
            {
                return;
            }

            var retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3,
                attempt => TimeSpan.FromMilliseconds(50 * attempt));

            var publishResult = await retryPolicy.ExecuteAndCaptureAsync(() =>
            {
                return @event switch
                {
                    DomainEventBase e => publisher.Publish(e, cancellationToken),
                    IntegrationEventBase e => eventBus.Publish(e, cancellationToken),
                    _ => throw new Exception($"Unsupported event type {@event.GetType()}")
                };
            });

            message.SetEventState(publishResult.Outcome == OutcomeType.Successful
                ? OutboxMessageState.Done
                : OutboxMessageState.Fail);

            await repository.UpdateOutboxEventAsync(message);
        }
    }
}