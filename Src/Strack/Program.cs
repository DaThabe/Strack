using Common;
using Common.Model.File.Gpx;
using Common.Service.File;
using IGPSport;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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