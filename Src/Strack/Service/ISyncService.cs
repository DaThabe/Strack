using Common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strack.Model.Database;
using Strack.Model.Entity.Activity;

namespace Strack.Service;


public interface ISyncService
{
    /// <summary>
    /// 判断行者训练是否已同步
    /// </summary>
    /// <param name="workoutId"></param>
    /// <returns></returns>
    Task<bool> IsSyncedFromXingZheAsync(long workoutId);
    /// <summary>
    /// 判断迹驰活动是否已同步
    /// </summary>
    /// <param name="activityId"></param>
    /// <returns></returns>
    Task<bool> IsSyncedFromIGPSportAsync(long activityId);

    /// <summary>
    /// 从行者活动Id获取没有同步的Id
    /// </summary>
    /// <param name="workoutIds"></param>
    /// <returns></returns>
    IAsyncEnumerable<T> GetNotSyncFromXingZheAsync<T>(IAsyncEnumerable<T> workoutIds) where T : IIdentifier<long>;
    /// <summary>
    /// 从迹驰活动Id获取没有同步的Id
    /// </summary>
    /// <param name="activityIds"></param>
    /// <returns></returns>
    IAsyncEnumerable<T> GetNotSyncFromIGPSportAsync<T>(IAsyncEnumerable<T> activityIds) where T : IIdentifier<long>;

    /// <summary>
    /// 添加活动
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(ActivityEntity entity);
    /// <summary>
    /// 添加一些活动
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task<int> AddRangeAsync(IAsyncEnumerable<ActivityEntity> entities);
}

public class SyncService(
    ILogger<SyncService> logger,
    IDbContextFactory<StrackDbContext> dbFactory
    ) : ISyncService
{
    public async Task<bool> IsSyncedFromXingZheAsync(long workoutId)
    {
        logger.LogTrace("正在检查行者训练是否同步:{wid}", workoutId);

        await using var dbContext = await dbFactory.CreateDbContextAsync();

        var isSynced = await dbContext.Activities
                   .AsNoTracking()
                   .Include(x => x.Source)
                   .ThenInclude(x => x.IGPSport)
                   .AnyAsync(x => x.Source.XingZhe != null && x.Source.XingZhe.WorkoutId == workoutId);

        logger.LogTrace("行者训练已同步:{value}", isSynced);
        return isSynced;
    }
    public async Task<bool> IsSyncedFromIGPSportAsync(long activityId)
    {
        logger.LogTrace("正在检查迹驰活动是否同步:{wid}", activityId);

        await using var dbContext = await dbFactory.CreateDbContextAsync();

        var isSynced = await dbContext.Activities
                   .AsNoTracking()
                   .Include(x => x.Source)
                   .ThenInclude(x => x.IGPSport)
                   .AnyAsync(x => x.Source.XingZhe != null && x.Source.XingZhe.WorkoutId == activityId);

        logger.LogTrace("迹驰活动已同步:{value}", isSynced);
        return isSynced;
    }

    public async IAsyncEnumerable<T> GetNotSyncFromXingZheAsync<T>(IAsyncEnumerable<T> workoutIds) where T : IIdentifier<long>
    {
        logger.LogTrace("正在获取行者未同步训练Id");

        await using var dbContext = await dbFactory.CreateDbContextAsync();

        await foreach (var id in workoutIds)
        {
            bool isSynced;

            try
            {
                isSynced = await dbContext.SourceXingZhe.AnyAsync(x => x.WorkoutId == id.Id);
            }
            catch(Exception ex)
            {
                isSynced = false;
                logger.LogError(ex, "获取行者训练是否同步失败:{id}", id);
            }

            if (!isSynced) yield return id;
        }

        logger.LogTrace("行者未同步训练Id获取完成");
        yield break;
    }
    public async IAsyncEnumerable<T> GetNotSyncFromIGPSportAsync<T>(IAsyncEnumerable<T> activityIds) where T : IIdentifier<long>
    {
        logger.LogTrace("正在获取迹驰未同步活动Id");

        await using var dbContext = await dbFactory.CreateDbContextAsync();

        await foreach (var id in activityIds)
        {
            bool isSynced = false;

            try
            {
                isSynced = await dbContext.SourceIGSPSport.AnyAsync(x => x.ActivityId == id.Id);
            }
            catch (Exception ex)
            {
                isSynced = false;
                logger.LogError(ex, "获取迹驰活动是否同步失败:{id}", id);
            }

            if (!isSynced) yield return id;
        }

        logger.LogTrace("迹驰未同步活动Id获取完成");
        yield break;
    }

    public async Task AddAsync(ActivityEntity entity)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        try
        {
            //添加实体
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("活动实体已添加:{eid}", entity.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "活动实体添加失败:{eid}", entity.Id);
        }
    }
    public async Task<int> AddRangeAsync(IAsyncEnumerable<ActivityEntity> entities)
    {
        logger.LogTrace("正在添加活动列表");
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        int count = 0;

        await foreach(var entity in entities)
        {
            try
            {
                //添加实体
                await dbContext.AddAsync(entity);
                await dbContext.SaveChangesAsync();

                count++;
                logger.LogInformation("活动实体已添加:{eid}", entity.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "活动实体添加失败:{eid}", entity.Id);
            }
        }

        logger.LogTrace("活动已添加,数量:{count}", count);
        return count;
    }
}