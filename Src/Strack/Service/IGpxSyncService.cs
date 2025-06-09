using Common.Model.File.Fit;
using Common.Model.File.Gpx;
using Common.Service.File;
using IGPSport.Service;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using XingZhe.Model.Exception;
using XingZhe.Model.Workout;
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
                    var time = t.Points.FirstOrDefault()?.Timestamp ?? DateTimeOffset.Now;
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
                    var time = t.Points.FirstOrDefault()?.Timestamp ?? DateTimeOffset.Now;
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
        var client = iGPSportClientProvider.Sessions.FirstOrDefault()?.Client ?? throw new IGPSportAPIException("iGPSORT 没有可用请求客户端");

        await foreach (var i in client.GetActivitySummariesAsync())
        {
            try
            {
                if (File.Exists(Path.Combine("IGPSport", $"{i.Id}.gpx"))) continue;

                var fitFile = await client.GetActivityFitFileAsync(i.FitFileUrl);
                var gpx = ToGpxFile(fitFile);

                await SaveGpxFile("IGPSport", $"{i.Id}.gpx", gpx);

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

        await foreach (var i in client.GetWorkoutSummariesAsync())
        {
            try
            {
                if (File.Exists(Path.Combine("XingZhe", $"{i.Id}.gpx"))) continue;

                var gpx = await client.GetWorkoutGpxFileAsync(i.Id);
                var track = gpx.Tracks.FirstOrDefault();

                if (track == null) continue;

                var records = await client.GetWorkoutStreamAsync(i.Id);
                records.AttachToTrackPoint(track.Points);

                await SaveGpxFile("XingZhe", $"{i.Id}.gpx", gpx);

                logger.LogInformation("行者Gpx下载完成:{gpx}", gpx);
                await Task.Delay(10);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "行者 活动数据保存失败");
            }
        }
    }



    private async Task SaveGpxFile(string folder, string fileName, GpxFile gpx)
    {
        Directory.CreateDirectory(folder);
        var filePath = Path.Combine(folder, fileName);

        using var fs = File.OpenWrite(filePath);
        await gpxService.Serialize(gpx).SaveAsync(fs, SaveOptions.None, default);
    }

    private static GpxFile ToGpxFile(FitFile fit)
    {
        GpxFile gpx = new();

        gpx.Metadata.Timestamp = DateTimeOffset.Now;
        gpx.Metadata.AuthorName = "Strack";

        Track track = new() { Name = "运动轨迹" };

        foreach (var x in fit.Records)
        {
            if (x.Timestamp == null) continue;
            if (x.Longitude == null) continue;
            if (x.Latitude == null) continue;

            var point = new TrackPoint()
            {
                Timestamp = x.Timestamp.Value,
                Longitude = x.Longitude.Value,
                Latitude = x.Latitude.Value,
                Altitude = x.Altitude,
                Cadence = x.Cadence,
                Distance = x.Distance,
                Heartrate = x.Heartrate,
                Power = x.Power,
                Speed = x.Speed,
                Temperature = x.Temperature,
            };

            track.Points.Add(point);
        }

        gpx.Tracks.Add(track);
        return gpx;
    }
}