using JoggingApp.BackgroundJobs;
using Microsoft.AspNetCore.Builder;
using Quartz;

namespace JoggingApp.Setup
{
    public static class OutboxProcessingEngineSetup
    {
        public static void AddOutboxProcessingEngine(this WebApplicationBuilder app)
        {
            app.Services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

                configure
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
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
