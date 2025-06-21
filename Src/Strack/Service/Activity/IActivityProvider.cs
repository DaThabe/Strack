using Common.Extension;
using IGPSport.Model.User.Activity.Detail;
using IGPSport.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Strack.Data;
using Strack.Data.Extension;
using Strack.Exceptions;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Activity.Record;
using Strack.Model.Entity.Enum;
using System.Net;
using System.Runtime.CompilerServices;
using XingZhe.Model.User.Workout.Detail;
using XingZhe.Service;

using FitRecord = Common.Model.File.Fit.Record;
using XingZheRecord = XingZhe.Model.User.Workout.Record.Record;

namespace Strack.Service.Activity;


/// <summary>
/// 第三方活动获取业务
/// </summary>
public interface IActivityProvider
{
    /// <summary>
    /// 获取一个活动
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="userId"></param>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<ActivityEntity> GetAsync(PlatformType platform, long userId, long activityId, CancellationToken cancellation = default);

    /// <summary>
    /// 获取活动明细
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="userId"></param>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<ActivityEntity> GetDetailAsync(PlatformType platform, long userId, long activityId, CancellationToken cancellation = default);

    /// <summary>
    /// 获取所有记录点
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="userId"></param>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<List<ActivityRecordEntity>> GetRecordsAsync(Guid activityEntityId, PlatformType platform, long userId, long activityId, CancellationToken cancellation = default);

    /// <summary>
    /// 获取所有活动Id
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    IAsyncEnumerable<long> GetActivityIdsAsync(PlatformType platform, long userId, CancellationToken cancellation = default);
}

public class ActivityProvider(
    IDbContextFactory<StrackDbContext> dbFactory,
    IXingZheClientProvider xingZheClientProvider,
    IIGPSportClientProvider iGPSportClientProvider
    ) : IActivityProvider
{
    public async IAsyncEnumerable<long> GetActivityIdsAsync(PlatformType platform, long userId, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        var userCredential = await GetUserCredentialAsync(platform, userId, cancellation);

        if (platform == PlatformType.XingZhe)
        {
            var client = xingZheClientProvider.GetOrCreateFromSessionId(userCredential.Content);
            await foreach(var i in client.GetWorkoutSummaryAsync()) yield return i.Id;
        }
        else if (platform == PlatformType.IGPSport)
        {
            var client = iGPSportClientProvider.GetOrCreateFromAuthToken(userCredential.Content);
            await foreach (var i in client.GetActivitySummaryAsync(cancellation: cancellation)) yield return i.Id;
        }
        else
        {
            throw new StrackDbException($"不支持的活动获取平台:{platform}");
        }
    }
    public async Task<ActivityEntity> GetDetailAsync(PlatformType platform, long userId, long activityId, CancellationToken cancellation = default)
    {
        var userCredential = await GetUserCredentialAsync(platform, userId, cancellation);

        if (platform == PlatformType.XingZhe)
        {
            var client = xingZheClientProvider.GetOrCreateFromSessionId(userCredential.Content);
            var detail = await client.GetWorkoutDetailAsync(activityId, cancellation);

            return detail.ToActivityEntity(userCredential.UserEntityId);
        }

        if (platform == PlatformType.IGPSport)
        {
            var client = iGPSportClientProvider.GetOrCreateFromAuthToken(userCredential.Content);
            var detail = await client.GetActivityDetail(activityId);

            return detail.ToActivityEntity(userCredential.UserEntityId);
        }

        throw new StrackDbException($"不支持的活动获取平台:{platform}");
    }
    public async Task<List<ActivityRecordEntity>> GetRecordsAsync(Guid activityEntityId, PlatformType platform, long userId, long activityId, CancellationToken cancellation = default)
    {
        var userCredential = await GetUserCredentialAsync(platform, userId, cancellation);

        if (platform == PlatformType.XingZhe)
        {
            var client = xingZheClientProvider.GetOrCreateFromSessionId(userCredential.Content);
            var records = await client.GetWorkoutRecordAsync(activityId, cancellation);

            return records.ToActivityRecordEntity(activityEntityId).ToList();
        }

        if (platform == PlatformType.IGPSport)
        {
            var client = iGPSportClientProvider.GetOrCreateFromAuthToken(userCredential.Content);
            var activity = await client.GetActivityDetail(activityId, cancellation);
            var records = (await client.GetActivityFitFileAsync(activity.FitUrl, cancellation)).Records;

            return records.ToActivityRecordEntity(activityEntityId).ToList();
        }

        throw new StrackDbException($"不支持的活动获取平台:{platform}");
    }
    public async Task<ActivityEntity> GetAsync(PlatformType platform, long userId, long activityId, CancellationToken cancellation = default)
    {
        var userCredential = await GetUserCredentialAsync(platform, userId, cancellation);

        if (platform == PlatformType.XingZhe)
        {
            var client = xingZheClientProvider.GetOrCreateFromSessionId(userCredential.Content);
            var detail = await client.GetWorkoutDetailAsync(activityId, cancellation);
            var records = await client.GetWorkoutRecordAsync(detail.Id, cancellation);

            return ActivityEntityFactory.FromXingZhe(userCredential.UserEntityId, detail, records);
        }

        if (platform == PlatformType.IGPSport)
        {
            var client = iGPSportClientProvider.GetOrCreateFromAuthToken(userCredential.Content);
            var detail = await client.GetActivityDetail(activityId, cancellation);
            var records = (await client.GetActivityFitFileAsync(detail.FitUrl, cancellation)).Records;

            return ActivityEntityFactory.FromIGPSport(userCredential.UserEntityId, detail, records);
        }

        throw new StrackDbException($"不支持的活动获取平台:{platform}");
    }


    //用户凭证信息
    private record UserCredential(Guid UserEntityId, CredentialType Type, string Content);
    //获取用户凭证
    private async Task<UserCredential> GetUserCredentialAsync(PlatformType platform, long userId, CancellationToken cancellation = default)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync(cancellation);
        var userEntity = await dbContext.FindUserByExternalIdAsync(platform, userId, x => x.Include(x => x.Credential), cancellation: cancellation);
        var credential = userEntity.Credential ?? throw new StrackDbException($"用户凭证无效:{platform}-{userId}");

        return new(userEntity.Id, credential.Type, credential.Content);
    }

}


internal static class ActivityEntityFactory
{
    /// <summary>
    /// 转为活动实体
    /// </summary>
    /// <param name="userEntityId"></param>
    /// <param name="detail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    public static ActivityEntity FromIGPSport(Guid userEntityId, ActivityDetail detail, IEnumerable<FitRecord> records)
    {
        var activityEntity = new ActivityEntity { UserId = userEntityId };

        CopyToEntity(detail, activityEntity);

        activityEntity.Records = activityEntity.Records = records.Select(r =>
        {
            var entity = new ActivityRecordEntity
            {
                Activity = activityEntity,
                ActivityId = activityEntity.Id
            };

            CopyToEntity(r, entity);
            return entity;

        }).ToHashSet();

        return activityEntity;
    }
    /// <summary>
    /// 转为活动实体
    /// </summary>
    /// <param name="userEntityId"></param>
    /// <param name="detail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    public static ActivityEntity FromXingZhe(Guid userEntityId, WorkoutDetail detail, IEnumerable<XingZheRecord> records)
    {
        var activityEntity = new ActivityEntity { UserId = userEntityId };

        CopyToEntity(detail, activityEntity);

        activityEntity.Records = activityEntity.Records = records.Select(r =>
        {
            var entity = new ActivityRecordEntity
            {
                Activity = activityEntity,
                ActivityId = activityEntity.Id
            };

            CopyToEntity(r, entity);
            return entity;

        }).ToHashSet();

        return activityEntity;
    }


    public static ActivityEntity ToActivityEntity(this ActivityDetail model, Guid userEntityId)
    {
        ActivityEntity entity = new() { UserId = userEntityId };
        CopyToEntity(model, entity);

        return entity;
    }
    public static ActivityEntity ToActivityEntity(this WorkoutDetail model, Guid userEntityId)
    {
        ActivityEntity entity = new() { UserId = userEntityId };
        CopyToEntity(model, entity);

        return entity;
    }

    public static ActivityRecordEntity ToActivityRecordEntity(this FitRecord model, Guid activityEntityId)
    {
        ActivityRecordEntity entity = new() { ActivityId = activityEntityId };
        CopyToEntity(model, entity);

        return entity;
    }
    public static ActivityRecordEntity ToActivityRecordEntity(this XingZheRecord model, Guid activityEntityId)
    {
        ActivityRecordEntity entity = new() { ActivityId = activityEntityId };
        CopyToEntity(model, entity);

        return entity;
    }


    public static IEnumerable<ActivityRecordEntity> ToActivityRecordEntity(this IEnumerable<FitRecord> models, Guid activityEntityId)
    {
        foreach(var i in  models)
        {
            yield return i.ToActivityRecordEntity(activityEntityId);
        }
    }
    public static IEnumerable<ActivityRecordEntity> ToActivityRecordEntity(this IEnumerable<XingZheRecord> models, Guid activityEntityId)
    {
        foreach (var i in models)
        {
            yield return i.ToActivityRecordEntity(activityEntityId);
        }
    }


    //活动记录拷贝到实体
    public static void CopyToEntity(this ActivityDetail model, ActivityEntity entity)
    {
        entity.Type = model.Type.ToStrackActivityType();
        entity.Title = model.Title;
        entity.CaloriesKilocalories = model.Calories?.Kilocalories.Round();

        entity.FinishUnixTimeSeconds = model.FinishTime.ToUnixTimeSeconds();
        entity.BeginUnixTimeSeconds = model.BeginTime.ToUnixTimeSeconds();


        var metrics = entity.Metrics;

        //海拔
        metrics.Altitude = new()
        {
            MaxMeters = model.Altitude?.Max?.Meters.Round(),
            MinMeters = model.Altitude?.Min?.Meters.Round(),
            AvgMeters = model.Altitude?.Avg?.Meters.Round(),
        };
        //踏频
        metrics.Cadence = new()
        {
            AvgCpm = model.Cadence?.Avg?.CyclesPerMinute.Round(),
            MaxCpm = model.Cadence?.Max?.CyclesPerMinute.Round()
        };
        //距离
        metrics.Distance = new()
        {
            TotalMeters = model.Distance?.Total?.Meters.Round(),
            UpslopeMeters = model.Distance?.Upslope?.Meters.Round(),
            DownslopeMeters = model.Distance?.Downslope?.Meters.Round(),
        };
        //时间
        metrics.Duration = new()
        {
            TotalSeconds = model.Duration?.Total?.TotalSeconds.Round(),
        };
        //高程
        metrics.Elevation = new()
        {
            AscentHeightMeters = model.Elevation?.AscentHeight?.Meters.Round(),
            DescentHeightMeters = model.Elevation?.DescentHeight?.Meters.Round(),
        };
        //心率
        metrics.Heartrate = new()
        {
            AvgBpm = model.Heartrate?.Avg?.BeatsPerMinute.Round(),
            MinBpm = model.Heartrate?.Min?.BeatsPerMinute.Round(),
            MaxBpm = model.Heartrate?.Max?.BeatsPerMinute.Round(),
        };
        //功率
        metrics.Power = new()
        {
            AvgWatts = model.Power?.Avg?.Watts,
            MaxWatts = model.Power?.Max?.Watts,
            NpWatts = model.Power?.Np?.Watts,

            If = model.Power?.If,
            Tss = model.Power?.Tss,
        };
        //坡度
        metrics.Slope = new()
        {
            MaxUpslope = model.Slope?.MaxUpslope?.Round(),
            MaxDownslope = model.Slope?.MaxDownslope?.Round(),

            AvgUpslope = model.Slope?.AvgUpslope?.Round(),
            AvgDownslope = model.Slope?.AvgDownslope?.Round(),
        };
        //速度
        metrics.Speed = new()
        {
            AvgKph = model.Speed?.Avg?.KilometersPerHour.Round(),
            MaxKph = model.Speed?.Max?.KilometersPerHour.Round(),
        };
        //天气
        metrics.Weather = new()
        {
            MaxCelsius = model.Temperature?.Max?.DegreesCelsius,
            AvgCelsius = model.Temperature?.Avg?.DegreesCelsius
        };
    }
    //Fit记录点拷贝到实体
    public static void CopyToEntity(this FitRecord model, ActivityRecordEntity entity)
    {
        entity.UnixTimeSeconds = model.Timestamp?.ToUnixTimeSeconds();
        entity.Longitude = model.Longitude;
        entity.Latitude = model.Latitude;

        entity.AltitudeMeters = model.Altitude?.Meters.Round();
        entity.DistanceMeters = model.Distance?.Meters.Round();
        entity.HeartrateBpm = (int?)model.Heartrate?.BeatsPerMinute;
        entity.SpeedBpm = model.Speed?.MetersPerSecond.Round();
        entity.PowerWatts = (int?)model.Power?.Watts.Round();
        entity.TemperatureCelsius = model.Temperature?.DegreesCelsius.Round();
    }


    //训练记录拷贝到实体
    public static void CopyToEntity(this WorkoutDetail model, ActivityEntity entity)
    {
        entity.Type = model.Type.ToStrackActivityType();
        entity.Title = model.Title;
        entity.CaloriesKilocalories = model.Calories?.Kilocalories.Round();

        entity.FinishUnixTimeSeconds = model.FinishTime.ToUnixTimeSeconds();
        entity.BeginUnixTimeSeconds = model.BeginTime.ToUnixTimeSeconds();

        //数据
        var metrics = entity.Metrics;

        //海拔
        metrics.Altitude = new()
        {
            AvgMeters = model.Altitude?.Avg?.Meters.Round(),
            MaxMeters = model.Altitude?.Max?.Meters.Round(),
        };
        //踏频
        metrics.Cadence = new()
        {
            AvgCpm = model.Cadence?.Avg?.CyclesPerMinute.Round(),
            MaxCpm = model.Cadence?.Max?.CyclesPerMinute.Round()
        };
        //距离
        metrics.Distance = new()
        {
            TotalMeters = model.Distance?.Total?.Meters.Round(),
            FlatMeters = model.Distance?.Flat?.Meters.Round(),
            UpslopeMeters = model.Distance?.Upslope?.Meters.Round(),
            DownslopeMeters = model.Distance?.Downslope?.Meters.Round(),
        };
        //时间
        metrics.Duration = new()
        {
            TotalSeconds = model.Duration?.Total?.TotalSeconds.Round(),
            FlatSeconds = model.Duration?.Flat?.TotalSeconds.Round(),
            UpslopeSeconds = model.Duration?.Upslope?.TotalSeconds.Round(),
            DownslopeSeconds = model.Duration?.Downslope?.TotalSeconds.Round(),
        };
        //时间
        metrics.Duration = new()
        {
            TotalSeconds = model.Duration?.Total?.TotalSeconds.Round()
        };
        //心率
        metrics.Heartrate = new()
        {
            AvgBpm = model.Heartrate?.Avg?.BeatsPerMinute.Round(),
            MaxBpm = model.Heartrate?.Max?.BeatsPerMinute.Round()
        };
        //功率
        metrics.Power = new()
        {
            AvgWatts = model.Power?.Avg?.Watts.Round(),
            MaxWatts = model.Power?.Max?.Watts.Round(),
            FtpWatts = model.Power?.Ftp?.Watts.Round(),
            NpWatts = model.Power?.Np?.Watts.Round(),

            If = model.Power?.If?.Round(),
            Vi = model.Power?.Vi?.Round(),
            Tss = model.Power?.Tss,
        };
        //坡度
        metrics.Slope = new()
        {
            Avg = model.Slope?.Avg?.Round(),
            Max = model.Slope?.Max?.Round(),
            Min = model.Slope?.Min?.Round(),
        };
        //速度
        metrics.Speed = new()
        {
            AvgKph = model.Speed?.Avg?.KilometersPerHour.Round(),
            MaxKph = model.Speed?.Max?.KilometersPerHour.Round()
        };
        //温度
        metrics.Weather = new()
        {
            MinCelsius = model.Temperature?.Min?.DegreesCelsius,
            MaxCelsius = model.Temperature?.Max?.DegreesCelsius,
            AvgCelsius = model.Temperature?.Avg?.DegreesCelsius
        };
    }
    //行者记录点拷贝到实体
    public static void CopyToEntity(this XingZheRecord model, ActivityRecordEntity entity)
    {
        entity.UnixTimeSeconds = model.Timestamp.ToUnixTimeSeconds();
        entity.Longitude = model.Longitude;
        entity.Latitude = model.Latitude;

        entity.AltitudeMeters = model.Altitude?.Meters.Round();
        entity.DistanceMeters = model.Distance?.Meters.Round();
        entity.HeartrateBpm = (int?)model.Heartrate?.BeatsPerMinute;
        entity.SpeedBpm = model.Speed?.MetersPerSecond.Round();
        entity.PowerWatts = (int?)model.Power?.Watts.Round();
        entity.TemperatureCelsius = model.Temperature?.DegreesCelsius.Round();
    }
}