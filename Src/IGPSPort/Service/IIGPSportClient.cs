using Common;
using Common.Extension;
using Common.Model.File.Fit;
using Common.Service.File;
using IGPSport.Model.Activity;
using IGPSport.Model.User;
using IGPSport.Service;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using UnitsNet;
using XingZhe.Model.Exception;

namespace IGPSport.Service;

public interface IIGPSportClient
{
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    Task<UserInfo> GetUserInfoAsync();

    /// <summary>
    /// 获取活动简要
    /// </summary>>
    /// <param name="pageNo">页码</param>
    /// <param name="pageSize">页面数量 (最大20)</param>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    Task<List<ActivitySummary>> GetActivitySummariesAsync(int pageNo, int pageSize = 20, ActivityType sport = ActivityType.All);

    /// <summary>
    /// 获取所有活动简要
    /// </summary>
    /// <param name="sport">运动类型</param>
    /// <returns></returns>
    IAsyncEnumerable<ActivitySummary> GetActivitySummariesAsync(ActivityType sport = ActivityType.All);

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
        _logger.LogTrace("正在获取用户信息");

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

            _logger.LogTrace("活动信息获取完成:{info}", result);
            return result;
        }
        catch (Exception ex)
        {
            throw new IGPSportAPIException("用户信息获取失败", ex);
        }
    }

    public async Task<List<ActivitySummary>> GetActivitySummariesAsync(int pageNo, int pageSize = 20, ActivityType sport = ActivityType.All)
    {
        _logger.LogTrace("正在获取活动简要, PageNo.:{pageNo}, PageSize:{pageSize}, Sport:{sport}", pageNo, pageSize, sport);

        try
        {
            var url = BuildActivitySummaryUrl(pageNo, pageSize, sport);
            var root = await client.GetJsonAsync(url);
            var rows = root.SelectToken("data.rows") ?? throw new ArgumentException("响应结果不存在 data.rows 节点");

            List<ActivitySummary> activitySummaries = [];

            foreach (var node in rows)
            {
                var item = ParserItem(node);
                activitySummaries.Add(item);
            }

            _logger.LogInformation("活动简要获取完成, 数量:{count}", activitySummaries.Count);
            return activitySummaries;
        }
        catch (Exception ex)
        {
            throw new IGPSportAPIException("活动简要获取失败", ex);
        }

        //转换一个元素
        ActivitySummary ParserItem(JToken node)
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

            _logger.LogTrace("活动信息获取完成:{info}", activitySummary);
            return activitySummary;
        }
    }
    public async IAsyncEnumerable<ActivitySummary> GetActivitySummariesAsync(ActivityType sport =  ActivityType.All)
    {
        for (int pageNo = 1, pageSize = 20; true; pageNo++)
        {
            List<ActivitySummary> results = [];

            try
            {
                var summaries = await GetActivitySummariesAsync(pageNo, pageSize, sport);
                if (summaries.Count == 0) break;

                results.AddRange(summaries);
            }
            catch(Exception)
            {
                //请求失败直接返回
                break;
            }

            foreach (var summary in results) yield return summary;
        }

        yield break;
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
            throw new IGPSportAPIException("Fit文件获取失败", ex);
        }
    }



    //日志记录器
    private ILogger<IGPSportClient> _logger = services.GetLogger<IGPSportClient>();


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
    private static string BuildActivityDetailUrl(int activityId)
    {
        return $"https://prod.zh.igpsport.com/service/web-gateway/web-analyze/activity/queryActivityDetail/{activityId}";
    } 
}