using Common.Extension;
using Common.Model.File.Gpx;
using Common.Service.File;
using IGPSport.Exceptions;
using IGPSport.Service;
using Microsoft.Extensions.Logging;
using XingZhe.Exceptions;
using XingZhe.Model.User.Workout.Record;
using XingZhe.Service;

namespace Strack.Service;


/// <summary>
/// 同步所有活动到 Gpx 文件
/// </summary>
public interface IGpxSyncService
{
    /// <summary>
    /// 合并所有路径到一个Gpx文件
    /// </summary>
    /// <returns></returns>
    Task CombineAsync();

    /// <summary>
    /// 从 iGPSORT 同步
    /// </summary>
    /// <returns></returns>
    Task FromIGPSportAsync();

    /// <summary>
    /// 从 行者 同步
    /// </summary>
    /// <returns></returns>
    Task FromXingZheAsync();
}

public class GpxSyncService(
    ILogger<GpxSyncService> logger,
    IIGPSportClientProvider iGPSportClientProvider,
    IXingZheClientProvider xingZheClientProvider,
    IGpxService gpxService) : IGpxSyncService
{
    public async Task CombineAsync()
    {
        GpxFile gpx = new();

        foreach(var i in Directory.EnumerateFiles("XingZhe", "*.gpx"))
        {
            try
            {
                var cur = await gpxService.LoadFromPathAsync(i);

                foreach (var t in cur.Tracks)
                {
                    //更新轨迹名称
                    var time = t.Points.FirstOrDefault()?.Time ?? DateTimeOffset.Now;
                    //yyyyMMdd_HHmmss_行者轨迹
                    t.Name = $"{time.ToOffset(TimeSpan.FromHours(8)):yyyyMMdd_HHmmss}_行者轨迹";
                }

                gpx.Tracks.AddRange(cur.Tracks);
                logger.LogTrace("已添加 行者 轨迹:{gpx}", cur);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "轨迹合并失败:{file}", i);
            }
        }

        foreach (var i in Directory.EnumerateFiles("IGPSport", "*.gpx"))
        {
            try
            {
                var cur = await gpxService.LoadFromPathAsync(i);

                foreach (var t in cur.Tracks)
                {
                    //更新轨迹名称
                    var time = t.Points.FirstOrDefault()?.Time ?? DateTimeOffset.Now;
                    //yyyyMMdd_HHmmss_行者轨迹
                    t.Name = $"{time.ToOffset(TimeSpan.FromHours(8)):yyyyMMdd_HHmmss}_iGPSPORT轨迹";
                }

                gpx.Tracks.AddRange(cur.Tracks);
                logger.LogTrace("已添加 IGPSport 轨迹:{gpx}", cur);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "轨迹合并失败:{file}", i);
            }
        }

        


        try
        {
            gpx.Tracks = gpx.Tracks.OrderBy(x => x.Name).ToList();
            await gpxService.SaveAsync(gpx, "", "all.gpx");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "合并文件保存失败:{gpx}", gpx);
        }
    }

    public async Task FromIGPSportAsync()
    {
        var client = iGPSportClientProvider.Sessions.FirstOrDefault()?.Client ?? throw new IGSportAPIException("iGPSORT 没有可用请求客户端");

        await foreach (var i in client.GetActivitySummaryAsync())
        {
            try
            {
                if (File.Exists(Path.Combine("IGPSport", $"{i.Id}.gpx"))) continue;

                var fitFile = await client.GetActivityFitFileAsync(i.FitFileUrl);

                var gpx = fitFile.ToGpxFile();
                await gpxService.SaveAsync(gpx, "IGPSport", $"{i.Id}.gpx");

                logger.LogInformation("iGSPORT Gpx 下载完成:{gpx}", gpx);
                await Task.Delay(10);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "iGSPORT 活动数据保存失败");
            }
        }
    }

    public async Task FromXingZheAsync()
    {
        var client = xingZheClientProvider.Sessions.FirstOrDefault()?.Client ?? throw new XingZheAPIException("行者 没有可用请求客户端");

        await foreach (var i in client.GetWorkoutSummaryAsync())
        {
            try
            {
                if (File.Exists(Path.Combine("XingZhe", $"{i.Id}.gpx"))) continue;

                var gpx = await client.GetWorkoutTrackAsync(i.Id);
                var track = gpx.Tracks.FirstOrDefault();

                if (track == null) continue;

                var records = await client.GetWorkoutRecordAsync(i.Id);
                records.AttachToTrackPoint(track.Points);

                await gpxService.SaveAsync(gpx, "XingZhe", $"{i.Id}.gpx");

                logger.LogInformation("行者Gpx下载完成:{gpx}", gpx);
                await Task.Delay(10);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "行者 活动数据保存失败");
            }
        }
    }
}