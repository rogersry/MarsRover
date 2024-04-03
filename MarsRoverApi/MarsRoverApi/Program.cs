using MarsRoverApi.Core.AutoMapper;
using MarsRoverApi.Core.Handlers.CQRS.Command;
using MarsRoverMessages;
using NServiceBus;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add your connection strings to this file
        builder.Configuration.AddJsonFile("appsettings-connectionstrings.json");

        // Add services to the container.
        builder.Host.UseNServiceBus(context =>
        {
            var endpointConfiguration = new EndpointConfiguration("roverrequest");

            var connectionString = context.Configuration.GetConnectionString("AzureServiceBusConnectionString");
            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>().ConnectionString(connectionString);
            transport.Routing().RouteToEndpoint(
                assembly: typeof(MoveRoverMessage).Assembly,
                destination: "roverrequest");
            endpointConfiguration.SendOnly();
            return endpointConfiguration;
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MoveRoverHandler>());

        builder.Services.AddAutoMapper(typeof(MoveRoverProfile));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}