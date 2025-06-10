using Common.Extension;
using Common.Model.File.Fit;
using IGPSport.Model.User.Activity.Detail;
using IGPSport.Model.User.Activity.Summary;
using IGPSport.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strack.Model.Database;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Activity.Data;
using Strack.Model.Entity.Record;
using XingZhe.Model.User.Workout.Detail;
using XingZhe.Model.User.Workout.Record;
using XingZhe.Service;

namespace Strack.Service;

public interface ISyncService
{
    /// <summary>
    /// 从行者同步
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    Task FromClient(IXingZheClient client);

    /// <summary>
    /// 从迹驰同步
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    Task FromClient(IIGPSportClient client);
}

public class SyncService(
    ILogger<SyncService> logger,
    IDbContextFactory<StrackDbContext> dbFactory
    ) : ISyncService
{
    public async Task FromClient(IIGPSportClient client)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        logger.LogTrace("正在获取迹驰活动记录同步列表");

        var syncList = await GetSyncListAsync();

        logger.LogTrace("正在同步迹驰活动记录, 数量:{count}", syncList.Count);


        foreach (var (entityId, model) in syncList)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                //活动详情
                var detail = await client.GetActivityDetail(model.Id);
                //活动数据
                var fitFile = await client.GetActivityFitFileAsync(model.FitFileUrl);


                //活动实体
                ActivityEntity activityEntity = new() { Id = entityId };
                //设置活动数据
                CopyToEntity(detail, activityEntity);

                //添加记录点
                foreach (var i in fitFile.Records.ToArray())
                {
                    RecordEntity recordEntity = new()
                    {
                        Activity = activityEntity,
                        ActivityId = activityEntity.Id
                    };

                    CopyToEntity(i, recordEntity);
                    activityEntity.Records.Add(recordEntity);
                }

                //添加实体
                await dbContext.AddAsync(activityEntity);

                //保存
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                logger.LogInformation("迹驰记录已保存,实体Id:{eid}", entityId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "迹驰活动储存失败 ActivityId:{id}", model.Id);
                await transaction.RollbackAsync();
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        //获取同步列表
        async Task<List<(Guid EntityId, ActivitySummary Model)>> GetSyncListAsync()
        {
            //未同步列表
            List<(Guid, ActivitySummary)> syncList = [];

            await foreach (var activity in client.GetActivitySummaryAsync())
            {
                //获取数据库Id
                var entityId = GetIGSportActivityGuid(activity.Id);

                //是否已同步数据
                if (await dbContext.Activities.AnyAsync(x => x.Id == entityId && x.Source == ActivitySourceType.IGSport))
                {
                    continue;
                }

                //添加到同步列表
                syncList.Add((entityId, activity));
            }

            return syncList;
        }
    }

    public async Task FromClient(IXingZheClient client)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        logger.LogTrace("正在获取行者训练记录同步列表");

        var syncList = await GetSyncListAsync();

        logger.LogTrace("正在同步行者训练记录, 数量:{count}", syncList.Count);


        foreach (var (entityId, workoutId) in syncList)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                //活动数据
                var model = await client.GetWorkoutDetailAsync(workoutId);
                //轨迹点
                var records = await client.GetWorkoutRecordPointAsync(workoutId);


                //活动实体
                ActivityEntity activityEntity = new() { Id = entityId };
                //设置活动数据
                CopyToEntity(model, activityEntity);

                //添加记录点
                foreach (var i in records.ToArray())
                {
                    RecordEntity recordEntity = new()
                    {
                        Activity = activityEntity,
                        ActivityId = activityEntity.Id
                    };

                    CopyToEntity(i, recordEntity);
                    activityEntity.Records.Add(recordEntity);
                }

                if (activityEntity.Records.Count == 0)
                {
                    logger.LogError("行者活动储存失败, 不包含记录点, ActivityId:{id}", model.Id);
                    return;
                }

                //添加实体
                await dbContext.AddAsync(activityEntity);

                //保存
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                logger.LogInformation("行者记录已保存,实体Id:{eid}", entityId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "行者活动储存失败 WorkoutId:{workoutId}", workoutId);
                await transaction.RollbackAsync();
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }


        //获取同步列表
        async Task<List<(Guid EntityId, long WorkoutId)>> GetSyncListAsync()
        {
            //未同步列表
            List<(Guid, long)> syncList = [];

            await foreach (var workoutInfo in client.GetWorkoutSummaryAsync())
            {
                //获取数据库Id
                var entityId = GetXingZheWorkoutGuid(workoutInfo.Id);

                //是否已同步数据
                if (await dbContext.Activities.AnyAsync(x => x.Id == entityId && x.Source == ActivitySourceType.XingZhe))
                {
                    continue;
                }

                //添加到同步列表
                syncList.Add((entityId, workoutInfo.Id));
            }

            return syncList;
        }
    }



    private static void CopyToEntity(WorkoutDetail model, ActivityEntity entity)
    {
        entity.Source = ActivitySourceType.XingZhe;
        entity.Sport = model.Type.ToStrackActivityType();
        entity.Title = model.Title;
        entity.CaloriesKilocalories = model.Calories?.Kilocalories.Round();

        entity.FinishUnixTimeSeconds = model.FinishTime?.ToUnixTimeSeconds();
        entity.BeginUnixTimeSeconds = model.BeginTime?.ToUnixTimeSeconds();

        entity.TotalDistanceMeters = model.Distance?.Meters.Round();
        entity.DurationSeconds = model.Duration?.TotalSeconds.Round();


        //踏频
        entity.Cadence = new CadenceDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgCpm = model.Cadence?.Avg?.CyclesPerMinute.Round(),
            MaxCpm = model.Cadence?.Max?.CyclesPerMinute.Round()
        };

        //高程
        entity.Elevation = new ElevationDataEntity()
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
        entity.Heartrate = new HeartrateDataEntity
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgBpm = model.Heartrate?.Avg?.BeatsPerMinute.Round(),
            MaxBpm = model.Heartrate?.Max?.BeatsPerMinute.Round()
        };

        //功率
        entity.Power = new PowerDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgWatts = model.Power?.Avg?.Watts.Round(),
            MaxWatts = model.Power?.Max?.Watts.Round(),
            FtpWatts = model.Power?.Ftp?.Watts.Round(),
            NpWatts =  model.Power?.Np?.Watts.Round(),

            If = model.Power?.If?.Round(),
            Vi = model.Power?.Vi?.Round(),
            Tss = model.Power?.Tss,
        };
        
        //速度
        entity.Speed = new SpeedDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgKph = model.Speed?.Avg?.KilometersPerHour.Round(),
            MaxKph = model.Speed?.Max?.KilometersPerHour.Round()
        };

        //温度
        entity.Temperature = new TemperatureDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            MinCelsius = model.Temperature?.Min?.DegreesCelsius,
            MaxCelsius = model.Temperature?.Max?.DegreesCelsius,
            AvgCelsius = model.Temperature?.Avg?.DegreesCelsius
        };
    }

    private static void CopyToEntity(ActivityDetail model, ActivityEntity entity)
    {
        entity.Source = ActivitySourceType.IGSport;
        entity.Sport = model.Type.ToStrackActivityType();
        entity.Title = model.Title;
        entity.CaloriesKilocalories = model.Calories?.Kilocalories.Round();

        entity.FinishUnixTimeSeconds = model.FinishTime?.ToUnixTimeSeconds();
        entity.BeginUnixTimeSeconds = model.BeginTime?.ToUnixTimeSeconds();

        entity.TotalDistanceMeters = model.Distance?.Meters.Round();
        entity.DurationSeconds = model.Duration?.TotalSeconds.Round();


        //踏频
        entity.Cadence = new CadenceDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgCpm = model.Cadence?.Avg?.CyclesPerMinute,
            MaxCpm = model.Cadence?.Max?.CyclesPerMinute
        };

        //高程
        entity.Elevation = new ElevationDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            MinAltitudeMeters = model.Elevation?.MinAltitude?.Meters.Round(),
            MaxAltitudeMeters = model.Elevation?.MaxAltitude?.Meters.Round(),
            AvgAltitudeMeters = model.Elevation?.AvgAltitude?.Meters.Round(),

            MaxUpslopeGrade =   model.Elevation?.MaxUpslopeGrade?.Round(),
            AvgUpslopeGrade =   model.Elevation?.AvgUpslopeGrade?.Round(),
            MaxDownslopeGrade = model.Elevation?.MaxDownslopeGrade?.Round(),
            AvgDownslopeGrade = model.Elevation?.AvgDownslopeGrade?.Round(),

            UpslopeDistanceMeters = model.Elevation?.UpslopeDistance?.Meters.Round(),
            DownslopeDistanceMeters = model.Elevation?.DownslopeDistance?.Meters.Round(),

            AscentHeightMeters = model.Elevation?.AscentHeight?.Meters.Round(),
            DescentHeightMeters = model.Elevation?.DescentHeight?.Meters.Round(),

            MaxAscentSpeed =  model.Elevation?.MaxAscentSpeed?.MetersPerHour.Round(),
            AvgAscentSpeed =  model.Elevation?.AvgAscentSpeed?.MetersPerHour.Round(),
            MaxDescentSpeed = model.Elevation?.AvgDescentSpeed?.MetersPerHour.Round(),
            AvgDescentSpeed = model.Elevation?.AvgDescentSpeed?.MetersPerHour.Round()
        };

        //心率
        entity.Heartrate = new HeartrateDataEntity
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgBpm = model.Heartrate?.Avg?.BeatsPerMinute.Round(),
            MinBpm = model.Heartrate?.Min?.BeatsPerMinute.Round(),
            MaxBpm = model.Heartrate?.Max?.BeatsPerMinute.Round(),
        };

        //功率
        entity.Power = new PowerDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgWatts = model.Power?.Avg?.Watts,
            MaxWatts = model.Power?.Max?.Watts,
            NpWatts =  model.Power?.Np?.Watts,

            If = model.Power?.If,
            Tss = model.Power?.Tss,
        };

        //速度
        entity.Speed = new SpeedDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            AvgKph = model.Speed?.Avg?.KilometersPerHour.Round(),
            MaxKph = model.Speed?.Max?.KilometersPerHour.Round(),
        };

        //温度
        entity.Temperature = new TemperatureDataEntity()
        {
            Activity = entity,
            ActivityId = entity.Id,

            MaxCelsius = model.Temperature?.Max?.DegreesCelsius,
            AvgCelsius = model.Temperature?.Avg?.DegreesCelsius
        };
    }

    private static void CopyToEntity(RecordPoint model, RecordEntity entity)
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

    private static void CopyToEntity(Record model, RecordEntity entity)
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


    private static Guid GetXingZheWorkoutGuid(long workoutId) => $"xingzhe_{workoutId}".ToHashedGuid();
    private static Guid GetIGSportActivityGuid(long activityId) => $"igsport_{activityId}".ToHashedGuid();
}
