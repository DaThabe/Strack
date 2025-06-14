using Common.Extension;
using IGPSport.Model.User.Activity.Detail;
using IGPSport.Service;
using Strack.Model.Entity;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Activity.Data;
using Strack.Model.Entity.Activity.Record;
using Strack.Model.Entity.User.Activity.Source;
using XingZhe.Model.User.Workout.Detail;
using XingZhe.Service;

namespace Strack.Factory.Entity;

/// <summary>
/// 
/// </summary>
public static class ActivityEntityFactory
{
    /// <summary>
    /// 从行者训练明细创建实体
    /// </summary>
    /// <param name="workoutDetail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    public static ActivityEntity FromWorkoutDetail(WorkoutDetail workoutDetail, IEnumerable<XingZhe.Model.User.Workout.Record.Record> records)
    {
        //活动实体
        ActivityEntity activityEntity = new() { Source = null! };
        CredentialEntity sourceEntity = new() { Activity = activityEntity, ActivityId = activityEntity.Id, Type = SourceType.XingZhe };

        sourceEntity.XingZhe = new XingZheSourceEntity() { InternalSource = sourceEntity, InternalSourceId = sourceEntity.InternalId, WorkoutId = workoutDetail.Id, UserId = workoutDetail.User.Id };
        activityEntity.Source = sourceEntity;

        //设置活动数据
        CopyToEntity(workoutDetail, activityEntity);

        //添加记录点
        foreach (var i in records.ToArray())
        {
            ActivityRecordEntity recordEntity = new()
            {
                Activity = activityEntity,
                ActivityId = activityEntity.Id
            };

            CopyToEntity(i, recordEntity);
            activityEntity.Records.Add(recordEntity);
        }

        return activityEntity;
    }

    /// <summary>
    /// 从迹驰活动明细创建实体
    /// </summary>
    /// <param name="activityDetail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    public static ActivityEntity FromActivityDetail(ActivityDetail activityDetail, IEnumerable<Common.Model.File.Fit.Record> records)
    {
        //活动实体
        ActivityEntity activityEntity = new() { Source = null! };
        CredentialEntity sourceEntity = new() { Activity = activityEntity, ActivityId = activityEntity.Id, Type = SourceType.IGPSport };

        sourceEntity.IGPSport = new IGPSportSourceEntity() { InternalSource = sourceEntity, InternalSourceId = sourceEntity.InternalId, ActivityId = activityDetail.Id, UserId = activityDetail.UserId };
        activityEntity.Source = sourceEntity;

        //设置活动数据
        CopyToEntity(activityDetail, activityEntity);

        //添加记录点
        foreach (var i in records.ToArray())
        {
            ActivityRecordEntity recordEntity = new()
            {
                Activity = activityEntity,
                ActivityId = activityEntity.Id
            };

            CopyToEntity(i, recordEntity);
            activityEntity.Records.Add(recordEntity);
        }

        return activityEntity;
    }


    /// <summary>
    /// 从迹驰服务器创建实体
    /// </summary>
    /// <param name="client"></param>
    /// <param name="activityId"></param>
    /// <param name="fitFileUrl"></param>
    /// <returns></returns>
    public static async Task<ActivityEntity> FromIGPSportAsync(IIGPSportClient client, long activityId, string fitFileUrl)
    {
        //活动数据
        var activityDetail = await client.GetActivityDetail(activityId);
        //Fit
        var fitFile = await client.GetActivityFitFileAsync(fitFileUrl);

        return FromActivityDetail(activityDetail, fitFile.Records);
    }

    /// <summary>
    /// 从行者服务器创建实体
    /// </summary>
    /// <param name="client"></param>
    /// <param name="workoutId"></param>
    /// <returns></returns>
    public static async Task<ActivityEntity> FromXingZheAsync(IXingZheClient client, long workoutId)
    {
        //活动数据
        var activityDetail = await client.GetWorkoutDetailAsync(workoutId);
        //Fit
        var records = await client.GetWorkoutRecordAsync(workoutId);

        return FromWorkoutDetail(activityDetail, records);
    }



    //行者训练明细
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
    //行者记录点
    private static void CopyToEntity(XingZhe.Model.User.Workout.Record.Record model, ActivityRecordEntity entity)
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


    //迹驰活动明细
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
    //Fit记录点
    private static void CopyToEntity(Common.Model.File.Fit.Record model, ActivityRecordEntity entity)
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