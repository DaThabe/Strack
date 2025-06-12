using Common;
using Common.Extension;
using Common.Model.File.Gpx;
using Common.Service.File;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Xml.Linq;
using UnitsNet;
using UnitsNet.Units;
using XingZhe.Exceptions;
using XingZhe.Model.User;
using XingZhe.Model.User.Workout;
using XingZhe.Model.User.Workout.Detail;
using XingZhe.Model.User.Workout.Record;
using XingZhe.Model.User.Workout.Summary;

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
    /// 获取训练摘要
    /// </summary>
    /// <param name="offset">偏移</param>
    /// <param name="limit">数量</param>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    Task<List<WorkoutSummary>> GetWorkoutSummaryAsync(int offset, int limit, WorkoutType? sport = null);

    /// <summary>
    /// 获取训练摘要
    /// </summary>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    IAsyncEnumerable<WorkoutSummary> GetWorkoutSummaryAsync(WorkoutType? sport = null);

    /// <summary>
    /// 获取训练明细
    /// </summary>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<WorkoutDetail> GetWorkoutDetailAsync(long workoutId);

    /// <summary>
    /// 获取训练轨迹 (Gpx文件, 仅包含经纬度,时间)
    /// </summary>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<GpxFile> GetWorkoutTrackAsync(long workoutId);

    /// <summary>
    /// 获取训练记录点
    /// </summary>
    /// <param name="workoutId">运动记录Id</param>
    /// <returns></returns>
    Task<List<Record>> GetWorkoutRecordAsync(long workoutId);
}

/// <summary>
/// 行者客户端
/// </summary>
/// <param name="services"></param>
/// <param name="client"></param>
public class XingZheClient(IServiceProvider services, HttpClient client) : IXingZheClient
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
                Name = username,
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

    public async Task<List<WorkoutSummary>> GetWorkoutSummaryAsync(int offset = 0, int limit = 24, WorkoutType? sport = null)
    {
        logger.LogTrace("正在获取训练摘要列表, Offset:{offset}, Limit:{limit}, Sport:{sport}", offset, limit, sport);

        try
        {
            var url = BuildWorkoutsSummaryUrl(offset, limit, sport);
            var root = await client.GetJsonAsync(url);
            var data = root.SelectToken("data.data") ?? throw new ArgumentException("响应结果不存在 data.data 节点");

            List<WorkoutSummary> items = [];

            foreach (var node in data)
            {
                var item = MapperWorkoutSummary(node);
                items.Add(item);
            }

            logger.LogInformation("训练摘要列表获取完成, 数量:{count}", items.Count);
            return items;
        }
        catch (Exception ex)
        {
            throw new XingZheAPIException("训练摘要列表获取失败", ex);
        }

        //转换一个元素
        static WorkoutSummary MapperWorkoutSummary(JToken node)
        {
            //训练Id
            var id = node.GetValue<long>("id");
            //标题
            var title = node.GetValue<string>("title");
            //训练开始时间 Unix 毫秒 Utc+8
            var beginTime = node.GetValue<long>("start_time");

            //训练类型
            var sportType = node.GetValue<WorkoutType>("sport");
            //缩略图网址
            var thumbnail = node.GetValue<string>("thumbnail");

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
                StartTime = beginTime.ToBeijingTime(),
                ThumbnailUrl = thumbnail,
                Distance = Length.FromMeters(distance),
                Duration = TimeSpan.FromSeconds(duration),
                AvgSpeed = Speed.FromKilometersPerHour(avgSpeed)
            };

            return summary;
        }
    }
    public async IAsyncEnumerable<WorkoutSummary> GetWorkoutSummaryAsync(WorkoutType? sport = null)
    {
        int retryCount = 0;

        for (int offset = 0, limit = 1000; true; offset += limit)
        {
            List<WorkoutSummary> results;

            try
            {
                results = await GetWorkoutSummaryAsync(offset, limit, sport);
                retryCount = 0;
                if (results.Count == 0) break;
            }
            catch(XingZheAPIException ex) when(ex.InnerException is HttpRequestException httpEx)
            {
                if (retryCount > 3)
                {
                    logger.LogError(ex, "训练摘要列表获取失败,已重试3次, 结束请求");
                    break;
                }

                logger.LogError(ex, "训练摘要列表请求失败:{code}, 将在1秒后重新请求", httpEx.StatusCode);

                await Task.Delay(1000);
                offset -= limit;
                retryCount++;
                
                continue;
            }

            foreach (var i in results) yield return i;
        }

        yield break;
    }

    public async Task<WorkoutDetail> GetWorkoutDetailAsync(long workoutId)
    {
        logger.LogTrace("正在获取训练明细, WorkoutId:{workoutId}", workoutId);

        try
        {
            var url = BuildActivityDetailUrl(workoutId);
            var root = await client.GetJsonAsync(url);
            var workout = root.SelectToken("data.workout") ?? throw new ArgumentException("响应结果不存在 data.workout 节点");
            var user = root.SelectToken("data.user") ?? throw new ArgumentException("响应结果不存在 data.user 节点");

            //标题
            var title = workout.GetValue<string>("title");
            //开始时间 (Unix Utc+8)
            var start_time = workout.GetValue<long>("start_time");
            //开始时间 (Unix Utc+8)
            var end_time = workout.GetValue<long>("end_time");
            //训练类型
            var sport = workout.GetValue<WorkoutType>("sport");
            //卡路里 (卡)
            var calories = workout.GetValue<int>("calories");
            //总距离 (米)
            var distance = workout.GetValue<int>("distance");
            //总时间 (秒)
            var duration = workout.GetValue<int>("duration");

            //用户Id
            var userId = user.GetValue<long>("userid");
            //用户名
            var username = user.GetValue<string>("username");
            //头像网址
            var avatarUrl = user.GetValue<string>("avatar");
            //功率 (瓦)
            var ftp = user.GetValueOrDefault<int?>("ftp");
            //阈值心率 (次/分)
            var lthr = user.GetValueOrDefault<int?>("lthr");
            //最大心率  (次/分)
            var max_hr = user.GetValueOrDefault<int?>("max_hr");
            //体重 (千克)
            var weight = user.GetValueOrDefault<int?>("weight");


            //海拔 (米)  没有海拔传感器的数据会返回 65036
            var avg_altitude = workout.GetValueOrDefault<double?>("avg_altitude");
            var max_altitude = workout.GetValueOrDefault<double?>("max_altitude");

            
            //踏频  (次/分钟)
            var avg_cadence = workout.GetValueOrDefault<double?>("avg_cadence");
            var max_cadence = workout.GetValueOrDefault<double?>("max_cadence");


            //下坡时间 (秒)
            var down_duration = workout.GetValueOrDefault<int?>("grade_data.down_duration");
            //下坡距离 (米)
            var down_distance = workout.GetValueOrDefault<int?>("grade_data.down_distance");

            //上坡时间 (秒)
            var up_duration = workout.GetValueOrDefault<int?>("grade_data.up_duration");
            //上坡距离 (米)
            var up_distance = workout.GetValueOrDefault<int?>("grade_data.up_distance");

            //平路时间 (秒)
            var flat_duration = workout.GetValueOrDefault<int?>("grade_data.flat_duration");
            //平路距离 (米)
            var flat_distance = workout.GetValueOrDefault<int?>("grade_data.flat_distance");


            //爬升坡度 (百分比)
            var avg_grade = workout.GetValueOrDefault<double?>("avg_grade");
            var min_grade = workout.GetValueOrDefault<double?>("min_grade");
            var max_grade = workout.GetValueOrDefault<double?>("max_grade");

            //心率  (次/分钟)
            var avg_heartrate = workout.GetValueOrDefault<double?>("avg_heartrate");
            var max_heartrate = workout.GetValueOrDefault<double?>("max_heartrate");


            //功率  (瓦)
            var power_avg = workout.GetValueOrDefault<double?>("power_avg");
            var power_max = workout.GetValueOrDefault<double?>("power_max");
            var power_ftp = workout.GetValueOrDefault<double?>("power_ftp");
            //正常化功率 (瓦)
            var power_np = workout.GetValueOrDefault<double?>("power_np");
            //强度因子
            var power_if = workout.GetValueOrDefault<double?>("power_if");
            //变异指数
            var power_vi = workout.GetValueOrDefault<double?>("power_vi");
            //训练压力得分
            var power_tss = workout.GetValueOrDefault<int?>("power_tss");


            //速度 (千米/时)
            var avg_speed = workout.GetValueOrDefault<double?>("avg_speed");
            var max_speed = workout.GetValueOrDefault<double?>("max_speed");


            //温度 (摄氏度)
            var avg_temp = workout.GetValueOrDefault<double?>("weather.avg_temp");
            var max_temp = workout.GetValueOrDefault<double?>("weather.max_temp");
            var min_temp = workout.GetValueOrDefault<double?>("weather.min_temp");


            WorkoutDetail data = new()
            {
                Id = workoutId,
                Title = title,
                Type = sport,
                Calories = calories.ToEnergy(EnergyUnit.Calorie),
                Distance = distance.ToLength(LengthUnit.Meter),
                Duration = duration.ToTimeSpan(TimeSpanUnit.Seconds),

                BeginTime = start_time.ToBeijingTime(),
                FinishTime = end_time.ToBeijingTime(),

                Cadence = new CadenceData()
                {
                    Avg = avg_cadence?.ToFrequency(FrequencyUnit.BeatPerMinute),
                    Max = max_cadence?.ToFrequency(FrequencyUnit.BeatPerMinute)
                },
                Elevation = new ElevationData()
                {
                    AvgAltitude = avg_altitude?.ToLength(LengthUnit.Meter),
                    MaxAltitude = max_altitude?.ToLength(LengthUnit.Meter),

                    AvgGrade = avg_grade,
                    MinGrade = min_grade,
                    MaxGrade = max_grade,

                    UpslopeDuration = up_duration?.ToTimeSpan(TimeSpanUnit.Seconds),
                    UpslopeDistance = up_distance?.ToLength(LengthUnit.Meter),

                    DownslopeDuration = down_duration?.ToTimeSpan(TimeSpanUnit.Seconds),
                    DownslopeDistance = down_distance?.ToLength(LengthUnit.Meter),

                    FlatDuration = flat_duration?.ToTimeSpan(TimeSpanUnit.Seconds),
                    FlatDistance = flat_distance?.ToLength(LengthUnit.Meter)
                },
                Heartrate = new HeartrateData()
                {
                    Avg = avg_heartrate?.ToFrequency(FrequencyUnit.BeatPerMinute),
                    Max = max_heartrate?.ToFrequency(FrequencyUnit.BeatPerMinute)
                },
                Power = new PowerData()
                {
                    Avg = power_avg?.ToPower(),
                    Max = power_max?.ToPower(),
                    Ftp = power_ftp?.ToPower(),
                    Np = power_np?.ToPower(),

                    If = power_if,
                    Vi = power_vi,
                    Tss = power_tss
                },
                Speed = new SpeedData()
                {
                    Avg = avg_speed?.ToSpeed(SpeedUnit.KilometerPerHour),
                    Max = max_speed?.ToSpeed(SpeedUnit.KilometerPerHour)
                },
                Temperature = new TemperatureData()
                {
                    Min = min_temp?.ToTemperature(),
                    Max = max_temp?.ToTemperature(),
                    Avg = avg_temp?.ToTemperature()
                },
                User = new UserData()
                {
                    Id = userId,
                    Name = username,
                    AvatarUrl = avatarUrl,
                    Ftp = ftp?.ToPower(PowerUnit.Watt),
                    LtHr = lthr?.ToFrequency(FrequencyUnit.BeatPerMinute),
                    MaxHr = max_hr?.ToFrequency(FrequencyUnit.BeatPerMinute),
                    Weight = weight?.ToMass(MassUnit.Kilogram)
                }
            };

            logger.LogInformation("训练明细获取完成:{info}", data);
            return data;
        }
        catch (Exception ex)
        {
            throw new XingZheAPIException("训练明细获取失败", ex);
        }
    }

    public async Task<GpxFile> GetWorkoutTrackAsync(long workoutId)
    {
        logger.LogTrace("正在获取训轨迹, WorkoutId:{workoutId}", workoutId);

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
                    point.Time = point.Time is null ? null : new DateTimeOffset(point.Time.Value.DateTime, TimeSpan.FromHours(8));
                }
            }

            logger.LogInformation("训练轨迹获取完成: {gpx}", gpxFile);
            return gpxFile;
        }
        catch(Exception ex)
        {
            throw new XingZheAPIException("取训轨迹获取失败", ex);
        }
    }
    public async Task<List<Record>> GetWorkoutRecordAsync(long workoutId)
    {
        logger.LogTrace("正在获取训练记录点信息, WorkoutId:{workoutId}", workoutId);

        try
        {
            string url = BuildTrackPointDetailUrl(workoutId);
            var root = await client.GetJsonAsync(url);
            var data = root?.SelectToken("data") ?? throw new ArgumentException("响应结果不存在 data 节点");

            List<Record> trackPoints = [];

            // Unix时间戳 (毫秒Utc+0)
            var timestamps = data.GetValue<long[]>("timestamp");
            //位置 (经纬度)
            var locations = data.GetValue<double[][]>("location");
            // 海拔高度 (米)
            var altitudes = data.GetValue<int[]>("altitude");

            // 心率 (次/分钟)
            var heartrates = data.GetValue<int[]>("heartrate");
            // 温度 (摄氏度℃)
            var temperatures = data.GetValue<int[]>("temperature");
            // 距离 (米)
            var distance = data.GetValue<double[]>("distance");
            // 速度 (米/秒)
            var speeds = data.GetValue<double[]>("speed");
            //踏频 (次/分钟)
            var cadences = data.GetValue<double[]>("cadence");
            //功率 (瓦)
            var powers = data.GetValue<int[]>("power");
            //左平衡 (百分比)
            var leftBalances = data.GetValue<int[]>("left_balance");
            //右平衡 (百分比)
            var rightBalances = data.GetValue<int[]>("right_balance");

            
            int count = timestamps.Length;
            for (int i = 0; i < count; i++)
            {
                var lon = locations.ElementAtOrDefault(i)?[0];
                var lat = locations.ElementAtOrDefault(i)?[1];

                if (lon == null || lat == null) continue;

                var point = new Record
                {
                    Latitude = lat.Value,
                    Longitude = lon.Value,
                    Heartrate = Frequency.FromBeatsPerMinute(heartrates.ElementAtOrDefault(i)),
                    Timestamp = timestamps.ElementAtOrDefault(i).ToBeijingTime(),
                    Altitude = Length.FromMeters(altitudes.ElementAtOrDefault(i)),
                    Distance = Length.FromMeters(distance.ElementAtOrDefault(i)),
                    Cadence = Frequency.FromCyclesPerMinute(cadences.ElementAtOrDefault(i)),
                    Temperature = Temperature.FromDegreesCelsius(temperatures.ElementAtOrDefault(i)),
                    Speed = Speed.FromMetersPerSecond(speeds.ElementAtOrDefault(i)),
                    Power = Power.FromWatts(powers.ElementAtOrDefault(i)),
                    LeftBalance = Ratio.FromPercent(leftBalances.ElementAtOrDefault(i)),
                    RightBalance = Ratio.FromPercent(rightBalances.ElementAtOrDefault(i))
                };

                trackPoints.Add(point);
            }

            logger.LogInformation("训练记录点信息获取完成, 总数:{count}", trackPoints.Count);
            return trackPoints;
        }
        catch (Exception ex)
        {
            throw new XingZheException("训练记录点信息获取失败", ex);
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