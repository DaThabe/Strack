using IGPSport.Model.User.Activity.Detail;
using Strack.Data;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Enum;
using XingZhe.Model.User.Workout.Detail;


using FitRecord = Common.Model.File.Fit.Record;
using XingZheRecord = XingZhe.Model.User.Workout.Record.Record;


namespace Strack.Service.Import.Handler;


/// <summary>
/// 活动导入委托
/// </summary>
/// <typeparam name="TActivityDetail"></typeparam>
/// <typeparam name="TRecord"></typeparam>
public interface IActivityImportHandler<TActivityDetail, TRecord>
{
    /// <summary>
    /// 平台类型
    /// </summary>
    PlatformType Platform { get; }

    /// <summary>
    /// 添加活动
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userEntityId"></param>
    /// <param name="detail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    Task<ActivityEntity> AddActivityAsync(StrackDbContext dbContext, Guid userEntityId, TActivityDetail detail, IEnumerable<TRecord> records);

    /// <summary>
    /// 从活动获取用户Id
    /// </summary>
    /// <param name="activity"></param>
    /// <returns></returns>
    long GetUserId(TActivityDetail activity);
    /// <summary>
    /// 从活动获取Id
    /// </summary>
    /// <param name="activity"></param>
    /// <returns></returns>
    long GetActivityId(TActivityDetail activity);
}



/// <summary>
/// 行者训练导入委托
/// </summary>
public class XingZheActivityImportHandler : IActivityImportHandler<WorkoutDetail, XingZheRecord>
{
    public PlatformType Platform => PlatformType.XingZhe;

    public async Task<ActivityEntity> AddActivityAsync(StrackDbContext dbContext, Guid userEntityId, WorkoutDetail detail, IEnumerable<XingZheRecord> records)
    {
        return await dbContext.AddActivityEntityAsync(userEntityId, detail, records);
    }

    public long GetActivityId(WorkoutDetail activity) => activity.Id;

    public long GetUserId(WorkoutDetail activity) => activity.User.Id;
}

/// <summary>
/// 迹驰活动导入委托
/// </summary>
public class IGPSportActivityImportHandler : IActivityImportHandler<ActivityDetail, FitRecord>
{
    public PlatformType Platform => PlatformType.IGPSport;

    public async Task<ActivityEntity> AddActivityAsync(StrackDbContext dbContext, Guid userEntityId, ActivityDetail detail, IEnumerable<FitRecord> records)
    {
        return await dbContext.AddActivityEntityAsync(userEntityId, detail, records);
    }

    public long GetActivityId(ActivityDetail activity) => activity.Id;

    public long GetUserId(ActivityDetail activity) => activity.UserId;
}