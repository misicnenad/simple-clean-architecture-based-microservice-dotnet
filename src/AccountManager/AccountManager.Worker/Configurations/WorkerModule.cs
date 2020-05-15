using AccountManager.Worker.Handlers;
using Autofac;

using Microsoft.Extensions.Configuration;

using Rebus.Config;
using Rebus.Retry.Simple;

namespace AccountManager.Worker.Configurations
{
    public class WorkerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterRebus((configurer, context) =>
            {
                var config = context.Resolve<IConfiguration>();
                var connectionString = config.GetConnectionString("ServiceBusConnectionString");
                var queueName = config.GetValue<string>("AccountManagerConfiguration:ServiceBusQueueName");

                return configurer
                        .Logging(l => l.Serilog())
                        .Transport(t => t.UseAzureServiceBus(connectionString, queueName))
                        .Options(o =>
                        {
                            o.SimpleRetryStrategy(secondLevelRetriesEnabled: true);
                        });
            });

            builder.RegisterHandler<CreateAccountMessageHandler>();
        }
    }
}
