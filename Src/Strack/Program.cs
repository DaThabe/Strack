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
            .UseStrack()
            .Build();

        await app.StartAsync();



        var gpxSyncService = app.Services.GetGpxSyncService();

        //var xingZheTask = gpxSyncService.FromXingZheAsync();
        //var igpsportTask = gpxSyncService.FromIGPSportAsync();

        //await Task.WhenAll(xingZheTask, igpsportTask);

        await gpxSyncService.CombineAsync();


        await app.StopAsync();
    }
}