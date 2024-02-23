using EasyNetQ;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MPStore.Core.Utils;
using MPStore.WebAPI.Core.Database;
using NetDevPack.Utilities;
using RabbitMQ.Client;
using static MPStore.WebAPI.Core.Database.ProviderConfiguration;

namespace MPStore.WebAPI.Core.Configuration
{
    public static class GenericHelthCheck
    {
        public static IHealthChecksBuilder AddDefaultHealthCheck(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var (database, connString) = DetectDatabase(configuration);
            var checkBuilder = services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "api" });

            var rabbitConnStr = configuration.GetMessageQueueConnection("MessageBus");

            if (rabbitConnStr.IsPresent())
                checkBuilder.AddEasyNetQRabbitHealthCheck(rabbitConnStr);


            return database switch
            {
                DatabaseType.SqlServer => checkBuilder.AddSqlServer(connString, name: "SqlServer", tags: new[] { "infra" }),
                _ => checkBuilder
            };
        }

        private static RabbitMQ.Client.IConnectionFactory CreateConnectionFactory(ConnectionConfiguration configuration)
        {
            var connectionFactory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = false,
                VirtualHost = configuration.VirtualHost,
                UserName = configuration.UserName,
                Password = configuration.Password,
                Port = configuration.Port,
                RequestedHeartbeat = configuration.RequestedHeartbeat,
                ClientProperties = configuration.ClientProperties,
                AuthMechanisms = configuration.AuthMechanisms,
                ClientProvidedName = configuration.Name,
                NetworkRecoveryInterval = configuration.ConnectIntervalAttempt,
                ContinuationTimeout = configuration.Timeout,
                DispatchConsumersAsync = true,
                ConsumerDispatchConcurrency = configuration.PrefetchCount,
            };

            if (configuration.Hosts.Count > 0)
                connectionFactory.HostName = configuration.Hosts[0].Host;

            return connectionFactory;
        }

        public static IApplicationBuilder UseDefaultHealthcheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("api"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecks("/healthz-infra", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("infra"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return app;
        }
    }
}
