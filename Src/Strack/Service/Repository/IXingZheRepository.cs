using Common.Extension;
using Common.Model;
using Microsoft.EntityFrameworkCore;
using Strack.Model.Database;
using Strack.Model.Entity;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Activity.Data;
using Strack.Model.Entity.Activity.Record;
using Strack.Model.Entity.User;
using XingZhe.Model.User.Workout.Detail;
using XingZhe.Model.User.Workout.Record;

namespace Strack.Service.Repository;


public interface IXingZheRepository
{
    /// <summary>
    /// 判断训练是否存在
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<bool> IsExistAsync(long activityId);

    /// <summary>
    /// 获取不存在的ID
    /// </summary>
    /// <param name="activityIds"></param>
    /// <returns></returns>
    Task<IEnumerable<long>> GetNotExistIdsAsync(IEnumerable<long> activityIds);
    /// <summary>
    /// 获取不存在的ID
    /// </summary>
    /// <param name="activityIds"></param>
    /// <returns></returns>
    IAsyncEnumerable<long> GetNotExistIdsAsync(IAsyncEnumerable<long> activityIds);
    /// <summary>
    /// 获取不存在的ID
    /// </summary>
    /// <param name="activityIds"></param>
    /// <returns></returns>
    IAsyncEnumerable<T> GetNotExistIdsAsync<T>(IAsyncEnumerable<T> activityIds) where T : IIdentifier<long>;

    /// <summary>
    /// 添加训练
    /// </summary>
    /// <param name="workoutDetail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    Task AddWorkoutAsync(WorkoutDetail workoutDetail, IEnumerable<Record> records);
}

public class XingZheRepository(IDbContextFactory<StrackDbContext> dbFactory) : IXingZheRepository
{
    public async Task<bool> IsExistAsync(long activityId)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        return await dbContext.XingZheSources.AnyAsync(x => x.WorkoutId == activityId);
    }
    public async Task AddWorkoutAsync(WorkoutDetail workoutDetail, IEnumerable<Record> records)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        await dbContext.EnsureActivityNotExistsAsync(workoutDetail.Id, SourceType.XingZhe);

        var userEntity = await dbContext.GetOrCreateUserAsync(workoutDetail.User.Id, SourceType.XingZhe);
        var activityEntity = CreateActivityEntity(workoutDetail, userEntity, records);

        //添加实体
        await dbContext.AddAsync(activityEntity);
        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task<IEnumerable<long>> GetNotExistIdsAsync(IEnumerable<long> activityIds)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        var existIds = await dbContext.XingZheSources
           .Where(x => activityIds.Contains(x.WorkoutId))
           .Select(x => x.WorkoutId)
           .ToListAsync();

        return activityIds.Except(existIds);
    }
    public async IAsyncEnumerable<long> GetNotExistIdsAsync(IAsyncEnumerable<long> activityIds)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        await foreach (var id in activityIds)
        {
            // 检查当前 id 是否存在于数据库中
            bool exists = await dbContext.XingZheSources
                .AnyAsync(x => x.WorkoutId == id);

            if (!exists)
            {
                yield return id;
            }
        }
    }
    public async IAsyncEnumerable<T> GetNotExistIdsAsync<T>(IAsyncEnumerable<T> activityIds) where T : IIdentifier<long>
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        await foreach (var id in activityIds)
        {
            // 检查当前 id 是否存在于数据库中
            bool exists = await dbContext.XingZheSources
                .AnyAsync(x => x.WorkoutId == id.Id);

            if (!exists)
            {
                yield return id;
            }
        }
    }


    //创建活动实体
    private static ActivityEntity CreateActivityEntity(WorkoutDetail detail, UserEntity user, IEnumerable<Record> records)
    {
        var activity = new ActivityEntity
        {
            User = user,
            UserId = user.Id,
            ExternalId = detail.Id,
            Source = SourceType.XingZhe
        };

        CopyToEntity(detail, activity);

        activity.Records = activity.Records = records.Select(r =>
        {
            var entity = new ActivityRecordEntity
            {
                Activity = activity,
                ActivityId = activity.Id
            };

            CopyToEntity(r, entity);
            return entity;

        }).ToHashSet();

        return activity;
    }
    //训练记录拷贝到实体
    private static void CopyToEntity(WorkoutDetail model, ActivityEntity entity)
    {
        entity.Sport = model.Type.ToStrackActivityType();
        entity.Title = model.Title;
        entity.CaloriesKilocalories = model.Calories?.Kilocalories.Round();

        entity.FinishUnixTimeSeconds = model.FinishTime.ToUnixTimeSeconds();
        entity.BeginUnixTimeSeconds = model.BeginTime.ToUnixTimeSeconds();

        entity.TotalDistanceMeters = model.Distance.Meters.Round();
        entity.DurationSeconds = model.Duration.TotalSeconds.Round();


        //踏频
        entity.Cadence = new CadenceEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgCpm = model.Cadence?.Avg?.CyclesPerMinute.Round(),
            MaxCpm = model.Cadence?.Max?.CyclesPerMinute.Round()
        };

        //高程
        entity.Elevation = new ElevationEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgAltitudeMeters = model.Elevation?.AvgAltitude?.Meters.Round(),
            MinAltitudeMeters = model.Elevation?.MinAltitude?.Meters.Round(),
            MaxAltitudeMeters = model.Elevation?.MaxAltitude?.Meters.Round(),

            AvgGrade = model.Elevation?.AvgGrade?.Round(),
            MaxGrade = model.Elevation?.MaxGrade?.Round(),
            MinGrade = model.Elevation?.MinGrade?.Round(),

            DownslopeDistanceMeters = model.Elevation?.DownslopeDistance?.Meters.Round(),
            UpslopeDistanceMeters = model.Elevation?.UpslopeDistance?.Meters.Round(),
            FlatDistanceMeters = model.Elevation?.FlatDistance?.Meters.Round(),

            DownslopeDurationSeconds = model.Elevation?.DownslopeDuration?.TotalSeconds.Round(),
            UpslopeDurationSeconds = model.Elevation?.UpslopeDuration?.TotalSeconds.Round(),
            FlatDurationSeconds = model.Elevation?.FlatDuration?.TotalSeconds.Round(),
        };

        //心率
        entity.Heartrate = new HeartrateEntity
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgBpm = model.Heartrate?.Avg?.BeatsPerMinute.Round(),
            MaxBpm = model.Heartrate?.Max?.BeatsPerMinute.Round()
        };

        //功率
        entity.Power = new PowerEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgWatts = model.Power?.Avg?.Watts.Round(),
            MaxWatts = model.Power?.Max?.Watts.Round(),
            FtpWatts = model.Power?.Ftp?.Watts.Round(),
            NpWatts = model.Power?.Np?.Watts.Round(),

            If = model.Power?.If?.Round(),
            Vi = model.Power?.Vi?.Round(),
            Tss = model.Power?.Tss,
        };

        //速度
        entity.Speed = new SpeedEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgKph = model.Speed?.Avg?.KilometersPerHour.Round(),
            MaxKph = model.Speed?.Max?.KilometersPerHour.Round()
        };

        //温度
        entity.Temperature = new TemperatureEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            MinCelsius = model.Temperature?.Min?.DegreesCelsius,
            MaxCelsius = model.Temperature?.Max?.DegreesCelsius,
            AvgCelsius = model.Temperature?.Avg?.DegreesCelsius
        };
    }
    //行者记录点拷贝到实体
    private static void CopyToEntity(Record model, ActivityRecordEntity entity)
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
