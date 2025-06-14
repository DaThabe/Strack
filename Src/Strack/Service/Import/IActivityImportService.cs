using IGPSport.Model.User.Activity.Detail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strack.Data;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Enum;
using XingZhe.Model.User.Workout.Detail;


using FitRecord = Common.Model.File.Fit.Record;
using XingZheRecord = XingZhe.Model.User.Workout.Record.Record;

namespace Strack.Service.Import;


/// <summary>
/// 活动导入业务
/// </summary>
public interface IActivityImportService
{
    /// <summary>
    /// 添加迹驰活动
    /// </summary>
    /// <param name="activityDetail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    Task<ActivityEntity> AddAsync(ActivityDetail activityDetail, IEnumerable<FitRecord> records);

    /// <summary>
    /// 添加行者训练
    /// </summary>
    /// <param name="workoutDetail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    Task<ActivityEntity> AddAsync(WorkoutDetail workoutDetail, IEnumerable<XingZheRecord> records);
}

public class ActivityImportService(
    ILogger<ActivityImportService> logger,
    IDbContextFactory<StrackDbContext> dbFactory
    ) : IActivityImportService
{
    public async Task<ActivityEntity> AddAsync(ActivityDetail activityDetail, IEnumerable<FitRecord> records)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        await dbContext.EnsureActivitySourceNotExistsAsync(activityDetail.Id, PlatformType.IGPSport);

        var userEntity = await dbContext.GetOrCreateUserAsync(activityDetail.UserId, PlatformType.IGPSport);
        var activityEntity = await dbContext.AddActivityEntityAsync(userEntity.Id, activityDetail, records);
        _ = await dbContext.AddActivitySourceEntity(activityEntity.Id, activityDetail.Id, PlatformType.IGPSport);

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        logger.LogInformation("迹驰活动已添加:{entity}", activityEntity);
        return activityEntity;
    }
    
    public async Task<ActivityEntity> AddAsync(WorkoutDetail workoutDetail, IEnumerable<XingZheRecord> records)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        await dbContext.EnsureActivitySourceNotExistsAsync(workoutDetail.Id, PlatformType.XingZhe);

        var userEntity = await dbContext.GetOrCreateUserAsync(workoutDetail.User.Id, PlatformType.XingZhe);
        var activityEntity = await dbContext.AddActivityEntityAsync(userEntity.Id, workoutDetail, records);
        _ = await dbContext.AddActivitySourceEntity(activityEntity.Id, workoutDetail.Id, PlatformType.XingZhe);

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        logger.LogInformation("行者活动已添加:{entity}", activityEntity);
        return activityEntity;
    }

}