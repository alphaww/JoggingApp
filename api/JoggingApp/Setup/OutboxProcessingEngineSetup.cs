using JoggingApp.BackgroundJobs;
using JoggingApp.Core.Outbox;
using JoggingApp.OutboxPublishingAgent;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace JoggingApp.Setup
{
    public static class OutboxProcessingEngineSetup
    {
        public static void AddOutboxProcessingEngine(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<QueryFactory>((e) =>
            {
                var connection = new SqlConnection(builder.Configuration["ConnectionString:DefaultConnection"]);
                var compiler = new SqlServerCompiler();
                return new QueryFactory(connection, compiler);
            });

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
