using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Strack.Extension;
using Strack.External.Xingzhe.Model;
using System.Xml.Linq;
using System.Xml.XPath;
using UnitsNet;

namespace Strack.External.Xingzhe;


/// <summary>
/// 行者 API
/// </summary>
public interface IXingzheApi
{
    /// <summary>
    /// 获取一些活动信息
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="offset">偏移</param>
    /// <param name="limit">数量</param>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    Task<List<ActivitySummary>> GetActivitySummariesAsync(long userId, int offset, int limit, XingzheActivityType? sport = null);

    /// <summary>
    /// 获取活动明细
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<ActivityDetail> GetActivityDetailAsync(long userId, long workoutId);

    /// <summary>
    /// 获取基础采样点
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<List<SamplingSummary>> GetTrackPointSummaryAsync(long userId, long workoutId);

    /// <summary>
    /// 获取详细采样点
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<List<SamplingDetail>> GetTrackPointDetailAsync(long userId, long workoutId);
}

/// <summary>
/// 行者 API 扩展
/// </summary>
public static class XingzheApiExtension
{
    /// <summary>
    /// 获取所有活动信息
    /// </summary>
    /// <param name="api">行者Api</param>
    /// <param name="userId">用户Id</param>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    public static async Task<List<ActivitySummary>> GetActivitySummariesAsync(this IXingzheApi api,  long userId, XingzheActivityType? sport = null)
    {
        List<ActivitySummary> workoutSummaries = [];

        for (int offset = 0, limit = 1000; true; offset += limit)
        {
            var results = await api.GetActivitySummariesAsync(userId, offset, limit, sport);
            if (results.Count == 0) break;

            workoutSummaries.AddRange(results);
        }

        return workoutSummaries;
    }
}


internal class XingzheApi(
    ILogger<XingzheApi> logger,
    IXingzheHttpClient httpClient
    ) : IXingzheApi
{
    public async Task<List<ActivitySummary>> GetActivitySummariesAsync(long userId, int offset = 0, int limit = 24, XingzheActivityType? sport = null)
    {
        logger.LogTrace("正在获取活动信息, SessionId:{session}, Offset:{offset}, Limit:{limit}, Sport:{sport}", userId, offset, limit, sport);

        var url = BuildWorkoutsSummaryUrl(offset, limit, sport);
        var json = await httpClient.GetJsonAsync(userId, url);

        var dataNode = json.SelectToken("data.data") ?? throw new InvalidOperationException("无法获取训练数据");
        List<ActivitySummary> workoutSummaries = [];

        foreach (var node in dataNode)
        {
            //训练Id
            var id = GetValue<long>(node, "id");
            //标题
            var title = GetValue<string>(node, "title");
            //训练开始时间 Unix 毫秒 Utc+8
            var beginTime = GetValue<long>(node, "start_time");

            //训练类型
            var sportType = GetValue<XingzheActivityType>(node, "sport");

            //平均均速 (千米时)
            var avgSpeed = GetValue<double>(node, "avg_speed");
            //距离 (米)
            var distance = GetValue<double>(node, "distance");
            //训练时间 (秒)
            var duration = GetValue<double>(node, "duration");


            ActivitySummary ActivityDetail = new()
            {
                Id = id,
                Title = title,
                Type = sportType,
                Timestamp = beginTime.ToBeijingTime(),
                Distance = Length.FromMeters(distance),
                Duration = TimeSpan.FromSeconds(duration),
                AvgSpeed = Speed.FromKilometersPerHour(avgSpeed)
            };

            workoutSummaries.Add(ActivityDetail);
            logger.LogTrace("活动信息获取完成:{info}", ActivityDetail);
        }


        logger.LogInformation("活动信息获取完成, 数量:{count}", workoutSummaries.Count);
        return workoutSummaries;
    }
    public async Task<ActivityDetail> GetActivityDetailAsync(long userId, long workoutId)
    {
        logger.LogTrace("正在获取训练详情, SessionId:{sessionId}", userId);

        var url = BuildActivityDetailUrl(workoutId);
        var node = await httpClient.GetJsonAsync(userId, url);
        node = node.SelectToken("data.workout") ?? throw new InvalidOperationException("无法获取训练详情");

        //标题
        var title = GetValue<string>(node, "title");
        //开始时间 (Unix Utc+8)
        var start_time = GetValue<long>(node, "start_time");
        //开始时间 (Unix Utc+8)
        var end_time = GetValue<long>(node, "end_time");
        //训练类型
        var sport = GetValue<XingzheActivityType>(node, "sport");

        //平均海拔 (米)
        var avg_altitude = GetValue<double>(node, "avg_altitude");
        //平均踏频 (次/分钟)
        var avg_cadence = GetValue<double>(node, "avg_cadence");
        //平均心率 (次/分钟)
        var avg_heartrate = GetValue<double>(node, "avg_heartrate");
        //平均速度 (千米/时)
        var avg_speed = GetValue<double>(node, "avg_speed");

        //最大海拔 (米)
        var max_altitude = GetValue<double>(node, "max_altitude");
        //最大踏频  (次/分钟)
        var max_cadence = GetValue<double>(node, "max_cadence");
        //最大心率  (次/分钟)
        var max_heartrate = GetValue<double>(node, "max_heartrate");
        //最大速度 (千米/时)
        var max_speed = GetValue<double>(node, "max_speed");

        //卡路里 (千卡)
        var calories = GetValue<int>(node, "calories");
        //总距离 (米)
        var distance = GetValue<int>(node, "distance");
        //总时间 (秒)
        var duration = GetValue<int>(node, "duration");


        ActivityDetail ActivityDetail = new()
        {
            Id = workoutId,
            Title = title,
            BeginTime = start_time.ToBeijingTime(),
            FinishTime = end_time.ToBeijingTime(),
            Sport = sport,

            AvgAltitude = Length.FromMeters(avg_altitude),
            AvgCadence = Frequency.FromCyclesPerMinute(avg_cadence),
            AvgHeartrate = Frequency.FromBeatsPerMinute(avg_heartrate),
            AvgSpeed = Speed.FromKilometersPerHour(avg_speed),

            MaxAltitude = Length.FromMeters(max_altitude),
            MaxCadence = Frequency.FromCyclesPerMinute(max_cadence),
            MaxHeartrate = Frequency.FromBeatsPerMinute(max_heartrate),
            MaxSpeed = Speed.FromKilometersPerHour(max_speed),

            Calories = Energy.FromKilocalories(calories),
            Distance = Length.FromMeters(distance),
            Duration = TimeSpan.FromSeconds(duration)
        };

        logger.LogInformation("训练详情获取完成:{info}", ActivityDetail);
        return ActivityDetail;
    }

    public async Task<List<SamplingSummary>> GetTrackPointSummaryAsync(long userId, long workoutId)
    {
        logger.LogTrace("正在获取训练位置信息, SessionId:{sessionId}", userId);

        string url = $"https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/gpx/";
        var xmlContent = await httpClient.GetStringAsync(userId, url);
        XDocument xml = XDocument.Parse(xmlContent);

        var elements = xml.XPathSelectElements("//*[local-name()='trkpt']");
        if (elements is null) return [];


        List<SamplingSummary> trackPointBases = [];

        foreach (XElement element in elements)
        {
            string latString = element.Attribute("lat")?.Value ?? "0.0";
            string lonString = element.Attribute("lon")?.Value ?? "0.0";
            string timeString = element.Value ?? throw new InvalidOperationException("无法解析的时间");

            var lat = double.Parse(latString);
            var lon = double.Parse(lonString);
            var time = Convert.ToDateTime(timeString) - TimeSpan.FromHours(8);

            trackPointBases.Add(new SamplingSummary()
            {
                Latitude = lat,
                Longitude = lon,

                // 修复了 GitHub Issue #1：行者导出的GPX时间为北京时间但时区用的是UTC
                // 参考：https://github.com/DaThabe/XingzheExport/issues/1
                Timestamp = new DateTimeOffset(time, TimeSpan.FromHours(8))
            });
        }

        logger.LogInformation("训练位置信息获取完成, 采样点数:{count}", trackPointBases.Count);
        return trackPointBases;
    }
    public async Task<List<SamplingDetail>> GetTrackPointDetailAsync(long userId, long workoutId)
    {
        logger.LogTrace("正在获取训练采样节点, SessionId:{sessionId}", userId);

        string url = $"https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/stream/";
        var json = await httpClient.GetJsonAsync(userId, url);

        List<SamplingDetail> trackPoints = [];

        var dataNode = json["data"] ?? throw new InvalidOperationException("无法获取训练采样数据");

        // 海拔高度 (米)
        var altitudes = GetValue<int[]>(dataNode, "altitude");

        // 踏频 (次/分钟)
        var cadences = GetValue<int[]>(dataNode, "cadence");
        // 心率 (次/分钟)
        var heartrates = GetValue<int[]>(dataNode, "heartrate");
        // 温度 (摄氏度℃)
        var temperatures = GetValue<int[]>(dataNode, "temperature");

        // 距离 (米)
        var distance = GetValue<float[]>(dataNode, "distance");
        // 速度 (米/秒)
        var speeds = GetValue<float[]>(dataNode, "speed");

        // 左平衡
        var left_balances = GetValue<int[]>(dataNode, "left_balance");
        // 右平衡
        var right_balances = GetValue<int[]>(dataNode, "right_balance");
        // 功率
        var powers = GetValue<int[]>(dataNode, "power");

        // Unix时间戳 (毫秒Utc+0)
        var timestamps = GetValue<long[]>(dataNode, "timestamp");

        //位置采样点
        var trackPointBases = await GetTrackPointSummaryAsync(userId, workoutId);


        int count = timestamps.Length;
        for (int i = 0; i < count; i++)
        {
            var basePoint = trackPointBases.ElementAtOrDefault(i);
            if (basePoint is null) continue;

            trackPoints.Add(new SamplingDetail
            {
                Latitude = basePoint.Latitude,
                Longitude = basePoint.Longitude,
                Heartrate = Frequency.FromBeatsPerMinute(heartrates.ElementAtOrDefault(i)),
                Altitude = Length.FromMeters(altitudes.ElementAtOrDefault(i)),
                Distance = Length.FromMeters(distance.ElementAtOrDefault(i)),
                Cadence = Frequency.FromCyclesPerMinute(cadences.ElementAtOrDefault(i)),
                Temperature = Temperature.FromDegreesCelsius(temperatures.ElementAtOrDefault(i)),
                Speed = Speed.FromMetersPerSecond(speeds.ElementAtOrDefault(i)),
                Power = Power.FromWatts(powers.ElementAtOrDefault(i)),
                Timestamp = timestamps[i].ToBeijingTime()
            });
        }

        logger.LogInformation("训练采样节点获取完成, 总数:{count}", trackPoints.Count);
        return trackPoints;
        
    }

    
    /// <summary>
    /// 获取Json值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static T GetValue<T>(JToken json, string path)
    {
        var node = json.SelectToken(path) ?? throw new InvalidOperationException($"无法获取Json值, 该路径不存在:{path}");
        return node.ToObject<T>() ?? throw new InvalidOperationException($"无法转换Json值, 目标不匹配, Json:{node}, 目标:{typeof(T)}");
    }

    /// <summary>
    /// 构建训练记录的简略信息的Url
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <param name="sport"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    private static string BuildWorkoutsSummaryUrl(int offset = 0, int limit = 24, XingzheActivityType? sport = null, int? year = null, int? month = null)
    {
        const string baseUrl = "https://www.imxingzhe.com/api/v1/pgworkout/";
        List<string> @params = [];

        @params.Add($"offset={offset}");
        @params.Add($"limit={limit}");

        if (sport != null) @params.Add($"sport={(int)sport}");
        if (year != null) @params.Add($"year={year}");
        if (month != null) @params.Add($"year={month}");

        return @params.Count == 0 ? baseUrl : $"{baseUrl}?{string.Join('&', @params)}";
    }

    /// <summary>
    /// 构建训练记录的详细信息的Url
    /// </summary>
    /// <param name="workoutId"></param>
    /// <param name="segments"></param>
    /// <param name="slopes"></param>
    /// <param name="pois"></param>
    /// <param name="laps"></param>
    /// <returns></returns>
    private static string BuildActivityDetailUrl(long workoutId, bool? segments = null, bool? slopes = null, bool? pois = null, bool? laps = null)
    {
        string baseUrl = $"https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/";
        List<string> @params = [];


        if (segments != null) @params.Add($"segments={segments}");
        if (slopes != null) @params.Add($"slopes={slopes}");
        if (pois != null) @params.Add($"pois={pois}");
        if (laps != null) @params.Add($"pois={laps}");

        return @params.Count == 0 ? baseUrl : $"{baseUrl}?{string.Join('&', @params)}";
    }
}