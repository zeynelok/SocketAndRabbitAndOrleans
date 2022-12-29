
using GrainInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Silo;
using Orleans.Hosting;
using Orleans.Statistics;
using Orleans.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Grains;

internal class Program
{
  static void Main(string[] args)
  {
    try
    {
      var host = StartSiloAsync().Result;
      Console.WriteLine("\n\n Press Enter to terminate.... \n\n");
      Console.ReadLine();

      host.StopAsync().Wait();
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      Console.ReadLine();
    }
  }
  static async Task<IHost> StartSiloAsync()
  {
    var builder = new HostBuilder()
    
        .UseOrleans(c =>
        {
          c.UseLocalhostClustering()
                    .Configure<ClusterOptions>(options =>
                    {
                      options.ClusterId = "deneme_orleans_cluster";
                      options.ServiceId = "deneme_orleans_cluster";
                    })
                      //.ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
                      .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(AnchorGrain).Assembly).WithReferences())
                    .ConfigureLogging(logging => logging.AddConsole());

        })
        
        .ConfigureServices((hostContext, services) =>
        {
          services.AddSingleton<IRabbitMqConnectionManager, RabbitMQConnectionManager>();

        });

    var host = builder.Build();
    await host.StartAsync();
    return host;
  }
}