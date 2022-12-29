using Microsoft.Extensions.Hosting;
using Server;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System.Reflection;

var host = SuperSocketHostBuilder.Create<StringPackageInfo, CommandLinePipelineFilter>()
    .UsePackageDecoder<CustomDecoder>()
    .UseSession<CustomSocketSession>()
    .UseCommand(opt => opt.AddCommandAssembly(Assembly.GetAssembly(typeof(LGN))))
    .ConfigureSuperSocket(options =>
    {
        options.Name = "Super Socket Server";
        options.Listeners = new List<ListenOptions>()
        {
            new ListenOptions(){Ip="Any",Port=6002}
        };
    })
.Build();


await host.RunAsync();


