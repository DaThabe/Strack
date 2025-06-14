using Common.Extension;
using IGPSport.Model.User.Activity.Detail;
using Microsoft.EntityFrameworkCore;
using Strack.Exceptions;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Activity.Record;
using Strack.Model.Entity.Activity.Source;
using Strack.Model.Entity.Enum;
using Strack.Model.Entity.User;
using Strack.Model.Entity.User.Credential;
using XingZhe.Model.User.Workout.Detail;
using FitRecord = Common.Model.File.Fit.Record;
using XingZheRecord = XingZhe.Model.User.Workout.Record.Record;

namespace Strack.Data;

internal static class StrackDbContextExtension
{
    /// <summary>
    /// 确保活动来源不存在
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="externalIActivityId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    /// <exception cref="StrackDbException"></exception>
    public static async Task EnsureActivitySourceNotExistsAsync(this StrackDbContext dbContext, long externalIActivityId, PlatformType platform)
    {
        if (await dbContext.ActivitySources.AnyAsync(x => x.ExternalId == externalIActivityId && x.Platform == platform))
        {
            throw new StrackDbException($"活动记录已存在, Id:{externalIActivityId},平台:{platform}");
        }
    }

    /// <summary>
    /// 判断活动来源是否存在
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="externalIActivityId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static async Task<bool> IsActivitySourceExistAsync(this StrackDbContext dbContext, long externalIActivityId, PlatformType platform)
    {
        return await dbContext.ActivitySources.AnyAsync(x => x.ExternalId == externalIActivityId && x.Platform == platform);
    }


    /// <summary>
    /// 获取或创建用户实体
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static async Task<UserEntity> GetOrCreateUserAsync(this StrackDbContext dbContext, long userId, PlatformType platform)
    {
        var userEntity = await dbContext.Users
            .FirstOrDefaultAsync(x => x.ExternalId == userId);

        if (userEntity != null) return userEntity;

        userEntity = new UserEntity() { ExternalId = userId, Platform = platform };

        await dbContext.AddAsync(userEntity);
        await dbContext.SaveChangesAsync();

        return userEntity;
    }

    /// <summary>
    /// 添加活动来源实体
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="activityEntityId"></param>
    /// <param name="externalId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static async Task<ActivitySourceEntity> AddActivitySourceEntity(this StrackDbContext dbContext, Guid activityEntityId, long externalId, PlatformType platform)
    {
        var sourceEntity = new ActivitySourceEntity()
        {
            ActivityId = activityEntityId,
            ExternalId = externalId,
            Platform = platform
        };

        await dbContext.AddAsync(sourceEntity);
        await dbContext.SaveChangesAsync();

        return sourceEntity;
    }




    /// <summary>
    /// 添加活动实体
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userEntityId"></param>
    /// <param name="detail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    public static async Task<ActivityEntity> AddActivityEntityAsync(this StrackDbContext dbContext, Guid userEntityId, ActivityDetail detail, IEnumerable<FitRecord> records)
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


        await dbContext.AddAsync(activityEntity);
        await dbContext.SaveChangesAsync();

        return activityEntity;
    }

    //活动记录拷贝到实体
    private static void CopyToEntity(ActivityDetail model, ActivityEntity entity)
    {
        entity.Sport = model.Type.ToStrackActivityType();
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
    private static void CopyToEntity(FitRecord model, ActivityRecordEntity entity)
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


    /// <summary>
    /// 添加活动实体
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userEntityId"></param>
    /// <param name="detail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    public static async Task<ActivityEntity> AddActivityEntityAsync(this StrackDbContext dbContext, Guid userEntityId, WorkoutDetail detail, IEnumerable<XingZheRecord> records)
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

        await dbContext.AddAsync(activityEntity);
        await dbContext.SaveChangesAsync();

        return activityEntity;
    }
    //训练记录拷贝到实体
    private static void CopyToEntity(WorkoutDetail model, ActivityEntity entity)
    {
        entity.Sport = model.Type.ToStrackActivityType();
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
    private static void CopyToEntity(XingZheRecord model, ActivityRecordEntity entity)
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
