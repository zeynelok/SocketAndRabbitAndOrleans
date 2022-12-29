using GrainInterfacesNet7;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Silo_Net7;

await Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
      siloBuilder
          .UseLocalhostClustering();
    })
    .ConfigureServices((hostContext, services) =>
    {
      services.AddSingleton<IRabbitMqConnectionManager, RabbitMQConnectionManager>();

    })
    .RunConsoleAsync();
