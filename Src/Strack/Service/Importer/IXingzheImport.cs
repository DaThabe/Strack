//using Common.Extension;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Strack.Extension;
//using Strack.Model.Database;
//using Strack.Model.Entity.Activity;
//using Strack.Model.Entity.Source;
//using XingZhe.Model.User.Workout;
//using XingZhe.Model.User.Workout.Track;
//using XingZhe.Model.Workout.Detail;
//using XingZhe.Service;

//namespace Strack.Service.Migrate;

//public interface IXingzheImport
//{
//    /// <summary>
//    /// 导入
//    /// </summary>
//    /// <param name="userId"></param>
//    /// <param name="activityId"></param>
//    /// <returns></returns>
//    Task ImportAsync(long userId, long activityId);
//}

//public class XingzheImport(
//    ILogger<XingzheImport> logger,
//     IXingZheClientProvider clientProvider,
//     StrackDbContext dbContext
//    ) : IXingzheImport
//{
//    public async Task ImportAsync(long userId, long workoutId)
//    {
//        logger.LogTrace("正在同步行者活动, Uid:{uid}, WorkoutId:{wid}", userId, workoutId);

//        var sourceGuid = workoutId.ToHashedGuid();

//        if(await dbContext.Activities.Include(x=>x.Source).Where(x => x.Source.Id  == sourceGuid && x.Source.Type == SourceType.XingZhe).AnyAsync())
//        {
//            throw new InvalidOperationException("活动已存在, 无法同步");
//        }

//        var api = clientProvider.WithUserId(userId);

//        var detail = await api.GetWorkoutDataAsync(workoutId);
//        var points = await api.GetWorkoutTrackPointsAsync(workoutId);

//        switch(detail.Sport)
//        {
//            case WorkoutType.Ride:
//                await ImportCyclingAsync(userId, detail, points); break;
//        }

//        throw new InvalidOperationException($"无法保存活动, 不支持的活动类型:{detail.Sport}");
//    }


//    protected async Task ImportCyclingAsync(long userId, WorkoutData activity, List<TrackPoint> samplings)
//    {
//        //CreateEntity(out var activityEntity, out var activityDataEntity);


//        //activityEntity.Title = activity.Title;
//        //activityEntity.CaloriesKilocalories = activity.Calories.Kilocalories;
        
//        ////时间
//        //(activityEntity.BeginTimeUtc, activityEntity.FinishTimeUtc) = (activity.BeginTime.UtcDateTime, activity.FinishTime.UtcDateTime);
//        ////总时间
//        //activityEntity.TotalTime = activity.FinishTime - activity.BeginTime;
//        ////活动时间
//        //activityEntity.MovingTime = samplings.Select(x => x.Timestamps.UtcDateTime).CalcTotalSpan();
//        ////暂停时间
//        //activityEntity.PauseTime = activityEntity.TotalTime - activityEntity.MovingTime;


//        ////温度
//        //(activityEntity.MinTemperatureCelsius, activityEntity.MaxTemperatureCelsius, activityEntity.AvgTemperatureCelsius, _) = samplings.Select(x => x.Temperature?.DegreesCelsius ?? 0).CalcMinMaxAvgSum();

//        ////爬升
//        //(activityEntity.TotalAscentMeters, activityEntity.TotalDescentMeters) = samplings.Select(x => x.Altitude?.Meters ?? 0).CalcCumulativeGainLoss();

//        ////速度
//        //(activityEntity.AvgSpeedKilometersPerHour, activityEntity.MaxSpeedKilometersPerHour) = (activity.AvgSpeed.KilometersPerHour, activity.MaxSpeed.KilometersPerHour);

//        ////心率
//        //(activityEntity.AvgHeartrateBeatsPerMinute, activityEntity.MaxBeatsPerMinute) = (activity.AvgHeartrate.BeatsPerMinute, activity.MaxHeartrate.BeatsPerMinute);
        
//        ////海拔
//        //(activityEntity.AvgAltitudeMeters, activityEntity.MaxAltitudeMeters) = (activity.AvgAltitude.Meters, activity.MaxAltitude.Meters);

        
//        ////踏频
//        //(activityDataEntity.AvgCadenceCyclesPerMinute, activityDataEntity.MaxCadenceCyclesPerMinute) = (activity.AvgCadence.CyclesPerMinute, activity.MaxCadence.CyclesPerMinute);

//        ////功率
//        //(_, activityDataEntity.MaxPowerWatts, activityDataEntity.AvgPowerWatts, activityDataEntity.TotalPowerWatts) = samplings.Select(x => x.Power?.Watts ?? 0).CalcMinMaxAvgSum();





//        //void CreateEntity(out ActivityEntity activityEntity, out CyclingActivityDataEntity activityDataEntity)
//        //{
//        //    var sourceDataEntity = new SourceType()
//        //    {
//        //        Source = null!,
//        //        SourceId = Guid.Empty,
//        //        ActivityId = activity.Id,
//        //        UserId = userId
//        //    };

//        //    var sourceEntity = new SourceEntity()
//        //    {
//        //        Activity = null!,
//        //        ActivityId = Guid.Empty,
//        //        Type = Model.SourceType.XingZhe,
//        //        Data = sourceDataEntity
//        //    };

//        //    activityDataEntity = new CyclingActivityDataEntity()
//        //    {
//        //        Activity = null!,
//        //        ActivityId = Guid.Empty
//        //    };

//        //    activityEntity = new ActivityEntity()
//        //    {
//        //        Source = sourceEntity,
//        //        Sport = Model.ActivityType.Ride
//        //    };

//        //    sourceDataEntity.Source = sourceEntity;
//        //    sourceDataEntity.SourceId = sourceEntity.Id;

//        //    sourceEntity.Activity = activityEntity;
//        //    sourceEntity.ActivityId = activityEntity.Id;

//        //    activityDataEntity.Activity = activityEntity;
//        //    activityDataEntity.ActivityId = activityEntity.Id;
//        //}
//    }



//    ///// <summary>
//    ///// 计算未同步的实体Id
//    ///// </summary>
//    ///// <param name="entityIds"></param>
//    ///// <returns></returns>
//    //private async Task<List<T>> CalcNotSyncItemsAsync<T>(IEnumerable<T> values, Func<T, Guid> entityIdGetter)
//    //{
//    //    List<T> results = [];

//    //    foreach (var item in values)
//    //    {
//    //        var id = entityIdGetter(item);

//    //        if (await dbContext.Activities.AnyAsync(x => x.Id == id))
//    //        {
//    //            continue;
//    //        }

//    //        results.Add(item);
//    //    }

//    //    return results;
//    //}

//    ///// <summary>
//    ///// 获取实体Id
//    ///// </summary>
//    //private static Guid GetEntityId(long userId, long workoutId)
//    //{
//    //    return HashExtension.CombineToGuid("imxingzhe.com", userId, workoutId);
//    //}

//    ///// <summary>
//    ///// 拷贝到实体, 不覆盖实体Id
//    ///// </summary>
//    //private static void CopyToEntity(ActivityDetail data, ActivityEntity entity)
//    //{
//    //    entity.Title = data.Title;
//    //    entity.Sport = Convert(data.Sport);

//    //    entity.BeginTimeUtc = data.BeginTime.UtcDateTime;
//    //    entity.FinishTimeUtc = data.FinishTime.UtcDateTime;

//    //    entity.AvgAltitudeMeters = data.AvgAltitude.Meters;
//    //    entity.MaxAltitudeMeters = data.MaxAltitude.Meters;
        

//    //    entity.MaxCadenceCyclesPerMinute = data.MaxCadence.CyclesPerMinute;



//    //    entity.AvgCadenceCyclesPerMinute = data.AvgCadence.CyclesPerMinute;
//    //    entity.AvgHeartrateBeatsPerMinute = data.AvgHeartrate.BeatsPerMinute;
//    //    entity.AvgSpeedKilometersPerHour = data.AvgSpeed.KilometersPerHour;

//    //    entity.MaxHeartrateBeatsPerMinute = data.MaxHeartrate.BeatsPerMinute;
//    //    entity.MaxSpeedKilometersPerHour = data.MaxSpeed.KilometersPerHour;

//    //    entity.CaloriesKilocalories = data.Calories.Kilocalories;
//    //    entity.DistanceMeters = data.Distance.Meters;
//    //    entity.DurationSeconds = data.Duration.TotalSeconds;
//    //}
    
//    ///// <summary>
//    ///// 拷贝到实体, 不覆盖实体Id
//    ///// </summary>
//    //private static void CopyToEntity(SamplingDetail data, SamplingEntity entity)
//    //{
//    //    entity.LatitudeDegrees = data.Latitude;
//    //    entity.LongitudeDegrees = data.Longitude;
//    //    entity.AltitudeMeters = data.Altitude.Meters;

//    //    entity.TimestampUTC = data.Timestamp.UtcDateTime;
//    //    entity.SpeedMetersPerSecond = data.Speed.MetersPerSecond;
//    //    entity.DistanceMeters = data.Distance.Meters;

//    //    entity.TemperatureCelsius = data.Temperature.DegreesCelsius;

//    //    entity.CadenCyclesPerMinute = data.Cadence.CyclesPerMinute;
//    //    entity.HeartrateBeatPerMinute = data.Heartrate.BeatsPerMinute;
//    //    entity.PowerWatts = data.Power.Watts;
//    //}

//    ///// <summary>
//    ///// 转换为数据库运动类型
//    ///// </summary>
//    //private static Model.ActivityType Convert(XingzheActivityType sportType)
//    //{
//    //    return sportType switch
//    //    {
//    //        XingzheActivityType.Hike => Model.ActivityType.Hike,
//    //        XingzheActivityType.Run => Model.ActivityType.Run,
//    //        XingzheActivityType.Ride => Model.ActivityType.Ride,
//    //        XingzheActivityType.Swim => Model.ActivityType.Swim,
//    //        XingzheActivityType.Ski => Model.ActivityType.Ski,
//    //        _ => Model.ActivityType.Other
//    //    };
//    //}
//}



////public async Task ImportAllAsync(long userId)
////    {
////        logger.LogTrace("正在同步所有训练明细, Uid:{uid}", userId);

////        //获取所有训练记录
////        var summaries = await xingzheApi.GetWorkoutSummariesAsync(userId);
////        //计算训练记录对应的实体Id
////        var results = summaries.Select(x => (EntityId: GetEntityId(userId, x.Id), WorkoutId: x.Id)).ToArray();
////        //计算未同步的元素
////        var waitSyncList = await CalcNotSyncItemsAsync(results, x => x.EntityId);

////        int successCount = 1;
////        logger.LogTrace("{n}个训练数据等待同步", waitSyncList.Count);

////        //同步剩余Id
////        foreach (var item in waitSyncList.ToArray())
////        {
////            try
////            {
////                await ImportAsync(userId, userId);
////                waitSyncList.Remove(item);

////                logger.LogTrace("同步成功 [{x}/{y}]...", successCount++, waitSyncList.Count);
////            }
////            catch (Exception ex)
////            {
////                logger.LogError(ex, "训练数据同步失败, Id:{id}, WorkoutId:{wid}", item.EntityId, item.WorkoutId);
////            }
////        }

////        if (waitSyncList.Count > 0)
////        {
////            logger.LogWarning("同步完成, 失败{n}个,{items}",
////                waitSyncList.Count,
////                string.Join(",", waitSyncList.Select(x => $"实体Id:{x.EntityId}, WorkoutId:{x.WorkoutId}")));
////        }
////        else
////        {
////            logger.LogInformation("同步完成, 共{n}个", successCount - 1);
////        }
////    }