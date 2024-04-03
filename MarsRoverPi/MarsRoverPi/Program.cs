using NServiceBus;
using System.Device.Gpio;

namespace Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x => x.AddJsonFile("appsettings-connectionstrings.json")) // Add your AzureServiceBus connection string in this file.
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));

                    logging.AddConsole();
                })
                .UseConsoleLifetime()
                .UseNServiceBus(context =>
                {
                    var endpointConfiguration = new EndpointConfiguration("roverrequest");
                    
                    var connectionString = context.Configuration.GetConnectionString("AzureServiceBusConnectionString");
                    var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>().ConnectionString(connectionString);
                    
                    endpointConfiguration.AuditProcessedMessagesTo("audit");

                    // Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
                    endpointConfiguration.EnableInstallers();

                    return endpointConfiguration;
                })
                .Build();

            await host.RunAsync();
        }
    }
}