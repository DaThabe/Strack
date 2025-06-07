using Common;
using Common.Extension;
using Common.Model.File.Gpx;
using Common.Service.File;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Xml.Linq;
using UnitsNet;
using XingZhe.Model;
using XingZhe.Model.Exception;
using XingZhe.Model.Workout;

namespace XingZhe.Service;


/// <summary>
/// 行者客户端
/// </summary>
public interface IXingZheClient
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    Task<UserInfo> GetUserInfoAsync();

    /// <summary>
    /// 获取一些活动信息
    /// </summary>
    /// <param name="offset">偏移</param>
    /// <param name="limit">数量</param>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    Task<List<WorkoutSummary>> GetWorkoutSummariesAsync(int offset, int limit, WorkoutType? sport = null);

    /// <summary>
    /// 获取所有训练简要
    /// </summary>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    IAsyncEnumerable<WorkoutSummary> GetWorkoutSummariesAsync(WorkoutType? sport = null);

    /// <summary>
    /// 获取训练明细
    /// </summary>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<WorkoutDetail> GetWorkoutDetailAsync(long workoutId);

    /// <summary>
    /// 获取训练Gpx文件 (轨迹点只有经纬度,海拔,时间)
    /// </summary>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<GpxFile> GetWorkoutGpxFileAsync(long workoutId);

    /// <summary>
    /// 获取训练采样点明细
    /// </summary>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<List<Record>> GetWorkoutStreamAsync(long workoutId);
}

/// <summary>
/// 行者客户端
/// </summary>
/// <param name="logger"></param>
/// <param name="client"></param>
public class XingZheClient(IServiceProvider services,HttpClient client) : IXingZheClient
{
    public async Task<UserInfo> GetUserInfoAsync()
    {
        logger.LogTrace("正在获取用户信息");

        try
        {
            var url = BuildUserInfoUrl();
            var root = await client.GetJsonOrDefaultAsync(url);
            JToken data = root?.SelectToken("data") ?? throw new ArgumentException("响应结果不存在 data 节点");

            //用户Id
            var id = data.GetValue<long>("id");
            //头像网址
            var avatarUrl = data.GetValue<string>("avatar");
            //用户名
            var username = data.GetValue<string>("username");
            //手机号
            var phoneNumber = data.GetValue<long>("mobile");
            //性别
            var gender = data.GetValue<GenderType>("sex");
            //生日 yyyy-MM-dd
            var birthdayString = data.GetValue<string>("birthday");
            var birthday = DateTime.ParseExact(birthdayString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //身高 (cm)
            var height = data.GetValue<double>("height");
            //体重 (kg)
            var weight = data.GetValue<double>("weight");

            var result = new UserInfo()
            {
                Id = id,
                AvatarUrl = avatarUrl,
                Birthday = new DateTimeOffset(birthday, TimeSpan.FromHours(8)),
                Username = username,
                PhoneNumber = phoneNumber,
                Gender = gender,
                Height = Length.FromCentimeters(height),
                Weight = Mass.FromKilograms(weight),
            };

            logger.LogTrace("活动信息获取完成:{info}", result);
            return result;
        }
        catch(Exception ex)
        {
            throw new XingZheAPIException("用户信息获取失败", ex);
        }
    }

    public async Task<List<WorkoutSummary>> GetWorkoutSummariesAsync(int offset = 0, int limit = 24, WorkoutType? sport = null)
    {
        logger.LogTrace("正在获取训练简要, Offset:{offset}, Limit:{limit}, Sport:{sport}", offset, limit, sport);

        try
        {
            var url = BuildWorkoutsSummaryUrl(offset, limit, sport);
            var root = await client.GetJsonAsync(url);
            var data = root.SelectToken("data.data") ?? throw new ArgumentException("响应结果不存在 data.data 节点");

            List<WorkoutSummary> workoutSummaries = [];

            foreach (var node in data)
            {
                var item = ParserItem(node);
                workoutSummaries.Add(item);
            }

            logger.LogInformation("训练简要获取完成, 数量:{count}", workoutSummaries.Count);
            return workoutSummaries;
        }
        catch (Exception ex)
        {
            throw new XingZheAPIException("训练简要获取失败", ex);
        }

        //转换一个元素
        WorkoutSummary ParserItem(JToken node)
        {
            //训练Id
            var id = node.GetValue<long>("id");
            //标题
            var title = node.GetValue<string>("title");
            //训练开始时间 Unix 毫秒 Utc+8
            var beginTime = node.GetValue<long>("start_time");

            //训练类型
            var sportType = node.GetValue<WorkoutType>("sport");

            //平均均速 (千米时)
            var avgSpeed = node.GetValue<double>("avg_speed");
            //距离 (米)
            var distance = node.GetValue<double>("distance");
            //训练时间 (秒)
            var duration = node.GetValue<double>("duration");


            WorkoutSummary summary = new()
            {
                Id = id,
                Title = title,
                Type = sportType,
                Timestamp = beginTime.ToBeijingTime(),
                Distance = Length.FromMeters(distance),
                Duration = TimeSpan.FromSeconds(duration),
                AvgSpeed = Speed.FromKilometersPerHour(avgSpeed)
            };

            logger.LogTrace("活动信息获取完成:{info}", summary);
            return summary;
        }
    }
    public async IAsyncEnumerable<WorkoutSummary> GetWorkoutSummariesAsync(WorkoutType? sport = null)
    {
        for (int offset = 0, limit = 24; true; offset += limit)
        {
            var results = await GetWorkoutSummariesAsync(offset, limit, sport);
            if (results.Count == 0) break;

            foreach (var i in results) yield return i;
        }

        yield break;
    }

    public async Task<WorkoutDetail> GetWorkoutDetailAsync(long workoutId)
    {
        logger.LogTrace("正在获取训练详情, WorkoutId:{workoutId}", workoutId);

        try
        {
            var url = BuildActivityDetailUrl(workoutId);
            var root = await client.GetJsonAsync(url);
            var workout = root.SelectToken("data.workout") ?? throw new ArgumentException("响应结果不存在 data.workout 节点");

            //标题
            var title = workout.GetValue<string>("title");
            //开始时间 (Unix Utc+8)
            var start_time = workout.GetValue<long>("start_time");
            //开始时间 (Unix Utc+8)
            var end_time = workout.GetValue<long>("end_time");
            //训练类型
            var sport = workout.GetValue<WorkoutType>("sport");

            //平均海拔 (米)
            var avg_altitude = workout.GetValue<double>("avg_altitude");
            //平均踏频 (次/分钟)
            var avg_cadence = workout.GetValue<double>("avg_cadence");
            //平均心率 (次/分钟)
            var avg_heartrate = workout.GetValue<double>("avg_heartrate");
            //平均速度 (千米/时)
            var avg_speed = workout.GetValue<double>("avg_speed");

            //最大海拔 (米)
            var max_altitude = workout.GetValue<double>("max_altitude");
            //最大踏频  (次/分钟)
            var max_cadence = workout.GetValue<double>("max_cadence");
            //最大心率  (次/分钟)
            var max_heartrate = workout.GetValue<double>("max_heartrate");
            //最大速度 (千米/时)
            var max_speed = workout.GetValue<double>("max_speed");

            //卡路里 (千卡)
            var calories = workout.GetValue<int>("calories");
            //总距离 (米)
            var distance = workout.GetValue<int>("distance");
            //总时间 (秒)
            var duration = workout.GetValue<int>("duration");


            WorkoutDetail ActivityDetail = new()
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
        catch (Exception ex)
        {
            throw new XingZheAPIException("训练详情获取失败", ex);
        }
    }

    public async Task<GpxFile> GetWorkoutGpxFileAsync(long workoutId)
    {
        logger.LogTrace("正在获取训 Gpx 文件, WorkoutId:{workoutId}", workoutId);

        try
        {
            string url = BuildTrackPointSummaryUrl(workoutId);
            var gpxDocString = await client.GetStringAsync(url);

            var document = XDocument.Parse(gpxDocString);
            var gpxFile = gpxService.Deserialize(document);

            foreach(var track in gpxFile.Tracks)
            {
                foreach(var point in track.Points)
                {
                    point.Timestamp = new DateTimeOffset(point.Timestamp.DateTime, TimeSpan.FromHours(8));
                }
            }

            logger.LogInformation("取训 Gpx 文件获取完成: {gpx}", gpxFile);
            return gpxFile;
        }
        catch(Exception ex)
        {
            throw new XingZheAPIException("取训 Gpx 文件获取失败", ex);
        }
    }
    public async Task<List<Record>> GetWorkoutStreamAsync(long workoutId)
    {
        logger.LogTrace("正在获取训练采样点详情, WorkoutId:{workoutId}", workoutId);

        try
        {
            string url = BuildTrackPointDetailUrl(workoutId);
            var root = await client.GetJsonAsync(url);
            var data = root?.SelectToken("data") ?? throw new ArgumentException("响应结果不存在 data 节点");

            List<Record> trackPoints = [];


            // 海拔高度 (米)
            var altitudes = data.GetValue<int[]>("altitude");

            // 踏频 (次/分钟)
            var cadences = data.GetValue<int[]>("cadence");
            // 心率 (次/分钟)
            var heartrates = data.GetValue<int[]>("heartrate");
            // 温度 (摄氏度℃)
            var temperatures = data.GetValue<int[]>("temperature");

            // 距离 (米)
            var distance = data.GetValue<double[]>("distance");
            // 速度 (米/秒)
            var speeds = data.GetValue<double[]>("speed");

            // 左平衡
            var left_balances = data.GetValue<int[]>("left_balance");
            // 右平衡
            var right_balances = data.GetValue<int[]>("right_balance");
            // 功率
            var powers = data.GetValue<int[]>("power");

            // Unix时间戳 (毫秒Utc+0)
            var timestamps = data.GetValue<long[]>("timestamp");


            int count = timestamps.Length;
            for (int i = 0; i < count; i++)
            {
                trackPoints.Add(new Record
                {
                    Timestamps = timestamps.ElementAtOrDefault(i).ToBeijingTime(),
                    Heartrate = Frequency.FromBeatsPerMinute(heartrates.ElementAtOrDefault(i)),
                    Altitude = Length.FromMeters(altitudes.ElementAtOrDefault(i)),
                    Distance = Length.FromMeters(distance.ElementAtOrDefault(i)),
                    Cadence = Frequency.FromCyclesPerMinute(cadences.ElementAtOrDefault(i)),
                    Temperature = Temperature.FromDegreesCelsius(temperatures.ElementAtOrDefault(i)),
                    Speed = Speed.FromMetersPerSecond(speeds.ElementAtOrDefault(i)),
                    Power = Power.FromWatts(powers.ElementAtOrDefault(i))
                });
            }

            logger.LogInformation("训练采样点详情获取完成, 总数:{count}", trackPoints.Count);
            return trackPoints;
        }
        catch (Exception ex)
        {
            throw new XingZheException("训练采样点详情获取失败", ex);
        }
    }



    private readonly ILogger<XingZheClient> logger = services.GetLogger<XingZheClient>();
    private readonly IGpxService gpxService = services.GetGpxService();


    /// <summary>
    /// 构建用户信息网址
    /// </summary>
    /// <returns></returns>
    private static string BuildUserInfoUrl()
    {
        const string url = "https://imxingzhe.com/api/v1/user/user_info/";
        return url;
    }

    /// <summary>
    /// 构建训练简要网址
    /// </summary>
    /// <param name="offset">偏移量</param>
    /// <param name="limit">获取数量</param>
    /// <param name="sport">运动类型</param>
    /// <param name="year">获取年份</param>
    /// <param name="month">获取月份</param>
    /// <returns></returns>
    private static string BuildWorkoutsSummaryUrl(int offset = 0, int limit = 24, WorkoutType? sport = null, int? year = null, int? month = null)
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
    /// 构建训练详情网址
    /// </summary>
    /// <param name="workoutId">训练Id</param>
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

    /// <summary>
    /// 构建训练采样点简要网址
    /// </summary>
    /// <param name="workoutId">训练Id</param>
    /// <returns></returns>
    private static string BuildTrackPointSummaryUrl(long workoutId)
    {
        return $"https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/gpx/";
    }

    /// <summary>
    /// 构建训练采样详情要网址
    /// </summary>
    /// <param name="workoutId">训练Id</param>
    /// <returns></returns>
    private static string BuildTrackPointDetailUrl(long workoutId)
    {
        return $"https://www.imxingzhe.com/api/v1/pgworkout/{workoutId}/stream/";
    }
}