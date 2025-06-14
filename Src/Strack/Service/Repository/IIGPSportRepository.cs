using Common.Extension;
using Common.Model.File.Fit;
using IGPSport.Model.User.Activity.Detail;
using Microsoft.EntityFrameworkCore;
using Strack.Exceptions;
using Strack.Model.Database;
using Strack.Model.Entity;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Activity.Data;
using Strack.Model.Entity.Activity.Record;
using Strack.Model.Entity.User;

namespace Strack.Service.Repository;


/// <summary>
/// 
/// </summary>
public interface IIGPSportRepository
{
    /// <summary>
    /// 判断活动是否存在
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<bool> IsExistAsync(long activityId);

    /// <summary>
    /// 添加活动
    /// </summary>
    /// <param name="activityDetail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    Task AddAsync(ActivityDetail activityDetail, IEnumerable<Record> records);
}


public class IGPSportRepository(IDbContextFactory<StrackDbContext> dbFactory) : IIGPSportRepository
{
    public async Task<bool> IsExistAsync(long activityId)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        return await dbContext.IGPSportSources.AnyAsync(x => x.ActivityId == activityId);
    }

    public async Task AddAsync(ActivityDetail activityDetail, IEnumerable<Record> records)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        await dbContext.EnsureActivityNotExistsAsync(activityDetail.Id, SourceType.IGPSport);

        var userEntity = await dbContext.GetOrCreateUserAsync(activityDetail.UserId, SourceType.IGPSport);
        var activityEntity = CreateActivityEntity(activityDetail, userEntity, records);

        //添加实体
        await dbContext.AddAsync(activityEntity);
        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }





    //创建活动实体
    private static ActivityEntity CreateActivityEntity(ActivityDetail detail, UserEntity user, IEnumerable<Record> records)
    {
        var activity = new ActivityEntity
        {
            User = user,
            UserId = user.Id,
            Source = SourceType.IGPSport,
            ExternalId = detail.Id
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


    //活动记录拷贝到实体
    private static void CopyToEntity(ActivityDetail model, ActivityEntity entity)
    {
        entity.Sport = model.Type.ToStrackActivityType();
        entity.Title = model.Title;
        entity.CaloriesKilocalories = model.Calories?.Kilocalories.Round();

        entity.FinishUnixTimeSeconds = model.FinishTime.ToUnixTimeSeconds();
        entity.BeginUnixTimeSeconds = model.BeginTime.ToUnixTimeSeconds();

        entity.TotalDistanceMeters = model.Distance?.Meters.Round();
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

            MinAltitudeMeters = model.Elevation?.MinAltitude?.Meters.Round(),
            MaxAltitudeMeters = model.Elevation?.MaxAltitude?.Meters.Round(),
            AvgAltitudeMeters = model.Elevation?.AvgAltitude?.Meters.Round(),

            MaxUpslopeGrade = model.Elevation?.MaxUpslopeGrade?.Round(),
            AvgUpslopeGrade = model.Elevation?.AvgUpslopeGrade?.Round(),
            MaxDownslopeGrade = model.Elevation?.MaxDownslopeGrade?.Round(),
            AvgDownslopeGrade = model.Elevation?.AvgDownslopeGrade?.Round(),

            UpslopeDistanceMeters = model.Elevation?.UpslopeDistance?.Meters.Round(),
            DownslopeDistanceMeters = model.Elevation?.DownslopeDistance?.Meters.Round(),

            AscentHeightMeters = model.Elevation?.AscentHeight?.Meters.Round(),
            DescentHeightMeters = model.Elevation?.DescentHeight?.Meters.Round(),

            MaxAscentSpeed = model.Elevation?.MaxAscentSpeed?.MetersPerHour.Round(),
            AvgAscentSpeed = model.Elevation?.AvgAscentSpeed?.MetersPerHour.Round(),
            MaxDescentSpeed = model.Elevation?.AvgDescentSpeed?.MetersPerHour.Round(),
            AvgDescentSpeed = model.Elevation?.AvgDescentSpeed?.MetersPerHour.Round()
        };

        //心率
        entity.Heartrate = new HeartrateEntity
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgBpm = model.Heartrate?.Avg?.BeatsPerMinute.Round(),
            MinBpm = model.Heartrate?.Min?.BeatsPerMinute.Round(),
            MaxBpm = model.Heartrate?.Max?.BeatsPerMinute.Round(),
        };

        //功率
        entity.Power = new PowerEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgWatts = model.Power?.Avg?.Watts,
            MaxWatts = model.Power?.Max?.Watts,
            NpWatts = model.Power?.Np?.Watts,

            If = model.Power?.If,
            Tss = model.Power?.Tss,
        };

        //速度
        entity.Speed = new SpeedEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgKph = model.Speed?.Avg?.KilometersPerHour.Round(),
            MaxKph = model.Speed?.Max?.KilometersPerHour.Round(),
        };

        //温度
        entity.Temperature = new TemperatureEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            MaxCelsius = model.Temperature?.Max?.DegreesCelsius,
            AvgCelsius = model.Temperature?.Avg?.DegreesCelsius
        };
    }
    //Fit记录点拷贝到实体
    private static void CopyToEntity(Record model, ActivityRecordEntity entity)
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
}