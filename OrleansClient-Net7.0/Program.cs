using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrleansClientNet7;

using var host = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(clientBuilder =>
    {
      clientBuilder.UseLocalhostClustering();
    })
    .Build();
await host.StartAsync();



var client = host.Services.GetRequiredService<IClusterClient>();


var subs = new Worker(client);
subs.Subscribe();

Console.ReadLine();
