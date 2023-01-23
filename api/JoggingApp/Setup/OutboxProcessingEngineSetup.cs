using JoggingApp.BackgroundJobs;
using JoggingApp.Core.Outbox;
using JoggingApp.OutboxPublishingAgent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Data;

namespace JoggingApp.Setup
{
    public static class OutboxProcessingEngineSetup
    {
        public static void AddOutboxProcessingEngine(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(builder.Configuration["ConnectionString:DefaultConnection"]));

            builder.Services.AddScoped<IOutboxStorage, OutboxStorage>();

            builder.Services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(BackgroundPublishingServiceAsync));

                configure
                    .AddJob<BackgroundPublishingServiceAsync>(jobKey)
                    .AddTrigger(
                        trigger =>
                            trigger.ForJob(jobKey)
                                .WithSimpleSchedule(
                                    schedule =>
                                        schedule.WithIntervalInSeconds(20)
                                            .RepeatForever()));

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            builder.Services.AddQuartzHostedService();
        }
    }
}
