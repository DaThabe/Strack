using Common;
using Common.Extension;
using IGPSport;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Strack;
using Strack.Factory;
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
            var igp = app.Services.GetIGPSportClientProvider().Sessions.First().Client;
            var xz = app.Services.GetXingZheClientProvider().Sessions.First().Client;

            var syncService = app.Services.GetSyncService();


            var igpTask = syncService.AddRangeAsync(syncService
                    .GetNotSyncFromIGPSportAsync(igp.GetActivitySummaryAsync())
                    .SelectAwait(async x => await ActivityEntityFactory.FromIGPSportAsync(igp, x.Id, x.FitFileUrl)));

            var xzTask = syncService.AddRangeAsync(syncService
                    .GetNotSyncFromXingZheAsync(xz.GetWorkoutSummaryAsync())
                    .SelectAwait(async x => await ActivityEntityFactory.FromXingZheAsync(xz, x.Id)));


            await Task.WhenAll(igpTask, xzTask);
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