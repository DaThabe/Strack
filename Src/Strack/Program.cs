using Common;
using IGPSport;
using Microsoft.Extensions.Hosting;
using Strack;
using XingZhe;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.Title = "Strack.Console";

        var app = Host.CreateDefaultBuilder(args)
            .UseCommon()
            .UseXingZhe()
            .UseIGPSport()
            .UseStrack();

        await app.RunConsoleAsync();
    }
}