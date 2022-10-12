using JoggingApp.BackgroundJobs;
using JoggingApp.Core.Outbox;
using JoggingApp.OutboxPublishingAgent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace JoggingApp.Setup
{
    public static class OutboxProcessingEngineSetup
    {
        public static void AddOutboxProcessingEngine(this WebApplicationBuilder app)
        {
            app.Services.AddScoped<IOutboxStorage, DapperOutboxStorage>();

            app.Services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(BackgroundPublishingService));

                configure
                    .AddJob<BackgroundPublishingService>(jobKey)
                    .AddTrigger(
                        trigger =>
                            trigger.ForJob(jobKey)
                                .WithSimpleSchedule(
                                    schedule =>
                                        schedule.WithIntervalInSeconds(10)
                                            .RepeatForever()));

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            app.Services.AddQuartzHostedService();
        }
    }
}
