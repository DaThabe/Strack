using Common;
using IGPSport;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Strack;
using Strack.Model.Sync;
using Strack.Service.Import;
using Strack.Service.Sync;
using Strack.Service.Sync.Activity;
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

        var syncFactory = app.Services.GetSyncFactoryService();
        var xingzheSync =  syncFactory.CreateXingZheSyncService("xp48ayxt8jeeamcri4lug70l6fnetk7v");
        var igpsportSync =  syncFactory.CreateIGPSportSyncService("eyJhbGciOiJSUzI1NiIsImtpZCI6IkJBRTA1NzIzODM0NjI1OUE0NzA1MjgyRjA4RDhBNkZBQTUzRTc5RTBSUzI1NiIsInR5cCI6ImF0K2p3dCIsIng1dCI6InV1QlhJNE5HSlpwSEJTZ3ZDTmltLXFVLWVlQSJ9.eyJuYmYiOjE3NDk5MTczOTgsImV4cCI6MTc1MDUyMjE5OCwiaXNzIjoiaHR0cDovL2F1dGhzZXJ2ZXIiLCJhdWQiOlsicWl3dS5zZXJ2aWNlLmFjdGl2aXR5LmFwaSIsInFpd3Uuc2VydmljZS5kZXZpY2UuYXBpIiwicWl3dS5zZXJ2aWNlLm1vYmlsZS5hcGkiLCJxaXd1LnNlcnZpY2UudXNlci5hcGkiLCJodHRwOi8vYXV0aHNlcnZlci9yZXNvdXJjZXMiXSwiY2xpZW50X2lkIjoicWl3dS5tb2JpbGUiLCJzdWIiOiIxMDI3Nzc0IiwiYXV0aF90aW1lIjoxNzQ5OTE3Mzk4LCJpZHAiOiJsb2NhbCIsIm1lbWJlcmlkIjoiMTAyNzc3NCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMDhkYmNjMTMtM2VmYy00ZWVlLTgzZDgtY2Y4Njg4NmQ2NjNhIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IiIsInBob25lX251bWJlciI6IiIsInBob25lX251bWJlcl92ZXJpZmllZCI6IkZhbHNlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiIiwiZW1haWxfdmVyaWZpZWQiOiJGYWxzZSIsInRlbmFudGlkIjoiIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiIiwiaWF0IjoxNzQ5OTE3Mzk4LCJzY29wZSI6WyJhY3Rpdml0eS5hcGkiLCJkZXZpY2UuYXBpIiwibW9iaWxlLmFwaSIsIm9wZW5pZCIsInVzZXIuYXBpIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImN1c3RvbSJdfQ.Fg9BhbrGAT3vnOtz32i7XfmAslVfmO0wEZ3wYm56cxr018mltf_a3614__zUfwAU-053hz34-8-e317feihHNZ5_oJqoMOLRk_DN1jzk1Lk03gZ32egKmou2mO0QfADlpCW4DfkHaA_mJE92HhfdkVt13ZSZAo6UDwuvkA2qfn-3chNtMPdKeJeEl8nnaiObrYmlgmHyn-JRL0Hfcv-PRVwdDFXfYviCEYiodPihtQ-4cWre0EoMwgByfz8-2uLKGxBUy41_gkfQKZqMQiSI-YvtQPqqCjqXLcELf25mqTCFpWqR7t__0NHSJFNegHTRtzpP-Wg2uc2GoSFMBofu1g");

        Progress<SyncInfo<IActivitySummary>> xingZheProgress = new(x => 
        logger.LogDebug("行者 {count}/{total}", x.Completed, x.Total));
        Progress<SyncInfo<IActivitySummary>> igpsportProgress = new(x => 
        logger.LogDebug("迹驰 {count}/{total}", x.Completed, x.Total));

        await Task.WhenAll(
            xingzheSync.SyncAsync(xingZheProgress),
            igpsportSync.SyncAsync(igpsportProgress));

        //try
        //{
        //    var igp = app.Services.GetIGPSportClientProvider().GetOrCreateFromAuthToken("");
        //    var xz = app.Services.GetXingZheClientProvider().get

        //    var syncService = app.Services.GetSyncService();


        //    var igpTask = syncService.AddRangeAsync(syncService
        //            .GetNotSyncFromIGPSportAsync(igp.GetActivitySummaryAsync())
        //            .SelectAwait(async x => await ActivityEntityFactory.FromIGPSportAsync(igp, x.Id, x.FitFileUrl)));

        //    var xzTask = syncService.AddRangeAsync(syncService
        //            .GetNotSyncFromXingZheAsync(xz.GetWorkoutSummaryAsync())
        //            .SelectAwait(async x => await ActivityEntityFactory.FromXingZheAsync(xz, x.Id)));


        //    await Task.WhenAll(igpTask, xzTask);
        //}
        //catch (Exception ex)
        //{
        //    logger.LogError(ex, "执行失败");
        //}


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