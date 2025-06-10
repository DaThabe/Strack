using Common;
using Common.Extension;
using Common.Model.File.Fit;
using IGPSport.Exceptions;
using IGPSport.Model.User;
using IGPSport.Model.User.Activity;
using IGPSport.Model.User.Activity.Detail;
using IGPSport.Model.User.Activity.Summary;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Globalization;
using UnitsNet;
using UnitsNet.Units;

namespace IGPSport.Service;

public interface IIGPSportClient
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    Task<UserInfo> GetUserInfoAsync();

    /// <summary>
    /// 获取活动摘要列表
    /// </summary>>
    /// <param name="pageNo">页码</param>
    /// <param name="pageSize">页面数量 (最大20)</param>
    /// <param name="type">活动类型</param>
    /// <returns></returns>
    Task<List<ActivitySummary>> GetActivitySummaryAsync(int pageNo, int pageSize = 20, ActivityType type = ActivityType.All);

    /// <summary>
    /// 获取活动摘要列表
    /// </summary>
    /// <param name="type">活动类型</param>
    /// <returns></returns>
    IAsyncEnumerable<ActivitySummary> GetActivitySummaryAsync(ActivityType type = ActivityType.All);

    /// <summary>
    /// 获取活动详情
    /// </summary>
    /// <param name="activityId">活动Id</param>
    /// <returns></returns>
    Task<ActivityDetail> GetActivityDetail(long activityId);

    /// <summary>
    /// 获取活动 Fit 文件
    /// </summary>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    Task<FitFile> GetActivityFitFileAsync(string fitFileUrl);
}

public class IGPSportClient(
    IServiceProvider services,
    HttpClient client
    ) : IIGPSportClient
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
            var id = data.GetValue<long>("memberId");
            //头像网址
            var avatarUrl = data.GetValue<string>("avatar");
            //昵称
            var nickName = data.GetValue<string>("nickName");
            //手机号
            var phoneNumber = data.GetValue<long>("phone");
            //性别
            var gender = data.GetValue<GenderType>("sex");
            //生日 yyyy-MM-dd
            var birthdayString = data.GetValue<string>("birthDate");
            var birthday = DateTime.ParseExact(birthdayString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //注册时间 MM/dd/yyyy HH:mm:ss
            var regTimeString = data.GetValue<string>("regTime");
            var regTime = DateTime.ParseExact(regTimeString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //身高 (cm)
            var height = data.GetValue<double>("height");
            //体重 (kg)
            var weight = data.GetValue<double>("weight");
            //粉丝数量
            var fans = data.GetValue<int>("fans");

            //最大心率 (次/分钟)
            var mhr = data.GetValue<double>("mhr");
            //阈值心率 (次/分钟)
            var lthr = data.GetValue<double>("lthr");
            //阈值功率 (瓦)
            var ftp = data.GetValue<double>("ftp");
        

            var result = new UserInfo()
            {
                Id = id,
                AvatarUrl = avatarUrl,
                NickName = nickName,
                PhoneNumber = phoneNumber,
                Gender = gender,
                Birthday = new DateTimeOffset(birthday, TimeSpan.FromHours(8)),
                RegisterTime = new DateTimeOffset(regTime, TimeSpan.FromHours(8)),

                Height = Length.FromCentimeters(height),
                Weight = Mass.FromKilograms(weight),
                Fans = fans,

                MHR = Frequency.FromBeatsPerMinute(mhr),
                LTHR = Frequency.FromBeatsPerMinute(lthr),
                FTP = Power.FromWatts(ftp),
            };

            logger.LogTrace("活动信息获取完成:{info}", result);
            return result;
        }
        catch (Exception ex)
        {
            throw new IGSportAPIException("用户信息获取失败", ex);
        }
    }

    public async Task<List<ActivitySummary>> GetActivitySummaryAsync(int pageNo, int pageSize = 20, ActivityType sport = ActivityType.All)
    {
        logger.LogTrace("正在获取活动简要, PageNo.:{pageNo}, PageSize:{pageSize}, Sport:{sport}", pageNo, pageSize, sport);

        try
        {
            var url = BuildActivitySummaryUrl(pageNo, pageSize, sport);
            var root = await client.GetJsonAsync(url);
            var rows = root.SelectToken("data.rows") ?? throw new ArgumentException("响应结果不存在 data.rows 节点");

            List<ActivitySummary> activitySummaries = [];

            foreach (var node in rows)
            {
                var item = MapperActivitySummary(node);
                activitySummaries.Add(item);
            }

            logger.LogTrace("活动简要获取完成, 数量:{count}", activitySummaries.Count);
            return activitySummaries;
        }
        catch (Exception ex)
        {
            throw new IGSportAPIException("活动简要获取失败", ex);
        }

        //转换一个元素
        static ActivitySummary MapperActivitySummary(JToken node)
        {
            //训练Id
            var id = node.GetValue<long>("rideId");
            //标题
            var title = node.GetValue<string>("title");
            //训练开始时间 yyyy.MM.DD
            var startTimeString = node.GetValue<string>("startTime");
            var startTime = DateTime.ParseExact(startTimeString, "yyyy.MM.dd", CultureInfo.InvariantCulture);

            //Fit 文件地址
            var fitFileUrl = node.GetValue<string>("fitOssPath");
            //训练类型
            var sportType = node.GetValue<ActivityType>("exerciseType");

            //平均均速 (秒/米)
            var avgSpeed = node.GetValue<double>("avgSpeed");
            //距离 (米)
            var distance = node.GetValue<double>("rideDistance");
            //训练时间 (秒)
            var duration = node.GetValue<double>("totalMovingTime");


            ActivitySummary activitySummary = new()
            {
                Id = id,
                Title = title,
                Type = sportType,
                StartTime = new DateTimeOffset(startTime, TimeSpan.FromHours(8)),
                FitFileUrl = fitFileUrl,
                Distance = Length.FromMeters(distance),
                Duration = TimeSpan.FromSeconds(duration),
                AvgSpeed = Speed.FromMetersPerSecond(avgSpeed)
            };

            return activitySummary;
        }
    }
    public async IAsyncEnumerable<ActivitySummary> GetActivitySummaryAsync(ActivityType sport =  ActivityType.All)
    {
        int retryCount = 0;

        for (int pageNo = 1, pageSize = 20; true; pageNo++)
        {
            List<ActivitySummary> results = [];

            try
            {
                var summaries = await GetActivitySummaryAsync(pageNo, pageSize, sport);
                if (summaries.Count == 0) break;

                results.AddRange(summaries);
                await Task.Delay(100);
            }
            catch(IGSportAPIException ex) when(ex.InnerException is HttpRequestException httpEx)
            {
                if (retryCount > 3)
                {
                    logger.LogError(ex, "迹驰活动列表获取失败,已重试3次, 结束请求");
                    break;
                }

                logger.LogError(ex, "迹驰活动列表请求失败:{code}, 将在1秒后重新请求", httpEx.StatusCode);

                await Task.Delay(1000);
                pageNo--;
                retryCount++;

                continue;
            }
            catch (IGSportAPIException)
            {
                break;
            }

            foreach (var summary in results) yield return summary;
        }

        yield break;
    }

    public async Task<ActivityDetail> GetActivityDetail(long activityId)
    {
        logger.LogTrace("正在获取迹驰活动数据, ActivityId:{activityId}", activityId);

        try
        {
            var url = BuildActivityDataUrl(activityId);
            var root = await client.GetJsonAsync(url);
            var data = root.SelectToken("data") ?? throw new ArgumentException("响应结果不存在 data 节点");

            //Id
            var id = data.GetValue<int>("rideId");
            //运动类型
            var sport = data.GetValue<ActivityType>("label");

            //标题
            var title = data.GetValue<string?>("title");

            //开始时间
            var startTime = data.GetValueOrDefault<string>("startTime")?.ToDateTimeOffset("yyyy-MM-dd HH:mm:ss", TimeSpan.FromHours(8));
            //结束时间
            var endTime = data.GetValueOrDefault<string>("endTime")?.ToDateTimeOffset("yyyy-MM-dd HH:mm:ss", TimeSpan.FromHours(8));

            //卡路里 (千卡)
            var calories = data.GetValueOrDefault<int?>("calorie");
            //总距离 (米)
            var distance = data.GetValueOrDefault<int?>("rideDistance");
            //总时间 (秒)
            var duration = data.GetValueOrDefault<int?>("totalTime");


            //海拔 (米)
            var avgAltitude = data.GetValueOrDefault<int?>("avgAltitude");
            var minAltitude = data.GetValueOrDefault<int?>("minAltitude");
            var maxAltitude = data.GetValueOrDefault<int?>("maxAltitude");

            //踏频  (次/分钟)
            var avgCad = data.GetValueOrDefault<int?>("avgCad");
            var maxCad = data.GetValueOrDefault<int?>("maxCad");

            //上坡距离 (米)
            var upslopeDistance = data.GetValueOrDefault<int?>("upslopeDistance");
            //下坡距离 (米)
            var downslopeDistance = data.GetValueOrDefault<int?>("downslopeDistance");

            //总上升高度 (米)
            var totalAscent = data.GetValueOrDefault<int?>("totalAscent");
            //总下降高度 (米)
            var totalDescent = data.GetValueOrDefault<int?>("totalDescent");

            //最大爬升坡度 (百分比)
            var upslopeMaxGrade = data.GetValueOrDefault<double?>("upslopeMaxGrade");
            //平均爬升坡度 (百分比)
            var upslopeAvgGrade = data.GetValueOrDefault<double?>("upslopeAvgGrade");

            //最大下降坡度 (百分比)
            var downslopeMaxGrade = data.GetValueOrDefault<double?>("downslopeMaxGrade");
            //平均下坡坡度 (百分比)
            var downslopeAvgGrade = data.GetValueOrDefault<double?>("downslopeAvgGrade");

            //平均上坡垂直速度
            var upslopeAvgVerticalSpeed = data.GetValueOrDefault<int?>("upslopeAvgVerticalSpeed");
            //最大上坡垂直速度
            var upslopeMaxVerticalSpeed = data.GetValueOrDefault<int?>("upslopeMaxVerticalSpeed");

            //平均上坡垂直速度
            var downslopeAvgVerticalSpeed = data.GetValueOrDefault<int?>("downslopeAvgVerticalSpeed");
            //最大上坡垂直速度
            var downslopeMaxVerticalSpeed = data.GetValueOrDefault<int?>("downslopeMaxVerticalSpeed");


            //心率  (次/分钟)
            var minHrm = data.GetValueOrDefault<int?>("minHrm");
            var maxHrm = data.GetValueOrDefault<int?>("maxHrm");
            var avgHrm = data.GetValueOrDefault<int?>("avgHrm");


            //功率  (瓦)
            var avgPower = data.GetValueOrDefault<double?>("avgPower");
            var maxPower = data.GetValueOrDefault<double?>("maxPower");
            //正常化功率 (瓦)
            var pwrNP = data.GetValueOrDefault<double?>("pwrNP");
            //强度因子
            var pwrIF = data.GetValueOrDefault<double?>("pwrIF");
            //训练压力得分
            var pwrTSS = data.GetValueOrDefault<int?>("pwrTSS");


            //速度 (米/秒)
            var avgSpeed = data.GetValueOrDefault<double?>("avgSpeed");
            var maxSpeed = data.GetValueOrDefault<double?>("maxSpeed");


            //温度 (摄氏度)
            var avgTemperature = data.GetValueOrDefault<int?>("avgTemperature");
            var maxTemperature = data.GetValueOrDefault<int?>("maxTemperature");



            ActivityDetail detail = new()
            {
                Id = activityId,
                Title = title,
                Type = sport,
                Calories = calories?.ToEnergy(EnergyUnit.Kilocalorie),
                Distance = distance?.ToLength(LengthUnit.Meter),
                Duration = duration?.ToTimeSpan(TimeSpanUnit.Seconds),

                BeginTime = startTime,
                FinishTime = endTime,

                Cadence = new CadenceData()
                {
                    Avg = avgCad?.ToFrequency(FrequencyUnit.BeatPerMinute),
                    Max = maxCad?.ToFrequency(FrequencyUnit.BeatPerMinute)
                },
                Elevation = new ElevationData()
                {
                    //海拔
                    AvgAltitude = avgAltitude?.ToLength(LengthUnit.Meter),
                    MinAltitude = minAltitude?.ToLength(LengthUnit.Meter),
                    MaxAltitude = maxAltitude?.ToLength(LengthUnit.Meter),

                    //上升下降高度
                    AscentHeight = totalAscent?.ToLength( LengthUnit.Meter),
                    DescentHeight = totalDescent?.ToLength(LengthUnit.Meter),

                    //上升下降距离
                    UpslopeDistance = upslopeDistance?.ToLength(LengthUnit.Meter),
                    DownslopeDistance = downslopeAvgGrade?.ToLength(LengthUnit.Meter),

                    //上升速度
                    AvgAscentSpeed = upslopeAvgVerticalSpeed?.ToSpeed(SpeedUnit.MeterPerHour),
                    MaxAscentSpeed = upslopeMaxVerticalSpeed?.ToSpeed(SpeedUnit.MeterPerHour),

                    //下降速度
                    AvgDescentSpeed = downslopeAvgVerticalSpeed?.ToSpeed(SpeedUnit.MeterPerHour),
                    MaxDescentSpeed = downslopeMaxVerticalSpeed?.ToSpeed(SpeedUnit.MeterPerHour),

                    //下降坡度
                    AvgDownslopeGrade = downslopeAvgGrade,
                    MaxDownslopeGrade = downslopeMaxGrade,

                    //上升坡度
                    AvgUpslopeGrade = upslopeAvgGrade,
                    MaxUpslopeGrade = downslopeMaxGrade,
                },
                Heartrate = new HeartrateData()
                {
                    Avg = avgHrm?.ToFrequency(FrequencyUnit.BeatPerMinute),
                    Max = avgHrm?.ToFrequency(FrequencyUnit.BeatPerMinute),
                    Min = avgHrm?.ToFrequency(FrequencyUnit.BeatPerMinute)
                },
                Power = new PowerData()
                {
                    Avg = avgPower?.ToPower(PowerUnit.Watt),
                    Max = maxPower?.ToPower(PowerUnit.Watt),

                    Np = pwrNP?.ToPower(PowerUnit.Watt),
                    If = pwrIF,
                    Tss = pwrTSS
                },
                Speed = new SpeedData()
                {
                    Avg = avgSpeed?.ToSpeed(SpeedUnit.MeterPerSecond),
                    Max = maxSpeed?.ToSpeed(SpeedUnit.MeterPerSecond)
                },
                Temperature = new TemperatureData()
                {
                    Max = maxTemperature?.ToTemperature(),
                    Avg = maxTemperature?.ToTemperature()
                }
            };

            logger.LogInformation("训练数据获取完成:{info}", detail);
            return detail;
        }
        catch (Exception ex)
        {
            throw new IGSportAPIException("训练数据获取失败", ex);
        }
    }

    public async Task<FitFile> GetActivityFitFileAsync(string fitFileUrl)
    {
        try
        {
            const string host = "https://igp-zh.oss-cn-hangzhou.aliyuncs.com/";
            fitFileUrl = fitFileUrl.Trim();
            if (host.StartsWith(fitFileUrl)) throw new ArgumentException("无效 Fit 文件地址");

            using var stream = await client.GetStreamAsync(fitFileUrl);
            return await services.GetFitService().DeserializeAsync(stream);
        }
        catch(Exception ex)
        {
            throw new IGSportAPIException("Fit文件获取失败", ex);
        }
    }


    private readonly ILogger<IGPSportClient> logger = services.GetLogger<IGPSportClient>();


    /// <summary>
    /// 构建用户信息网址
    /// </summary>
    /// <returns></returns>
    private static string BuildUserInfoUrl()
    {
        const string url = "https://prod.zh.igpsport.com/service/mobile/api/User/UserInfo";
        return url;
    }

    /// <summary>
    /// 构建活动简要网址
    /// </summary>
    /// <param name="pageNo">页面码数 (从1开始)</param>
    /// <param name="pageSize">页面元素数量 (最大20)</param>
    /// <param name="sport">运动类型</param>
    /// <param name="reqType">请求类型? 不知道干嘛的</param>
    /// <returns></returns>
    private static string BuildActivitySummaryUrl(int pageNo = 1, int pageSize = 20, ActivityType sport = ActivityType.All, int reqType = 1)
    {
        return $"https://prod.zh.igpsport.com/service/web-gateway/web-analyze/activity/queryMyActivity?pageNo={pageNo}&pageSize={pageSize}&reqType={reqType}&sort={(int)sport}";
    }

    /// <summary>
    /// 构建活动明细网址
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    private static string BuildActivityDataUrl(long activityId)
    {
        return $"https://prod.zh.igpsport.com/service/web-gateway/web-analyze/activity/queryActivityDetail/{activityId}";
    } 
}