using Common;
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

        var logger = app.Services.GetLogger<Program>();

        try
        {
            var igs = app.Services.GetIGPSportClientProvider().Sessions.First().Client;
            var xz = app.Services.GetXingZheClientProvider().Sessions.First().Client;

            var syncService = app.Services.GetSyncService();

            var igsTask = syncService.FromClient(igs);
            var xzTask = syncService.FromClient(xz);

            await Task.WhenAll(igsTask, xzTask);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "执行失败");
        }
       

        //await foreach (var i in client.GetWorkoutListAsync())
        //{
        //    var data = await client.GetWorkoutDataAsync(i.Id);
        //    var gpx = await client.GetWorkoutTrackAsync(i.Id);
        //    var records = await client.GetWorkoutRecordPointAsync(i.Id);

        //    Console.WriteLine("Fuck");
        //}


        await app.StopAsync();
    }
}