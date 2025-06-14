//using Common.Model;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Strack.Exceptions;
//using Strack.Model.Database;
//using Strack.Model.Entity.Activity;
//using Strack.Model.Entity.Enum;
//using Strack.Service.Repository;
//using XingZhe.Service;

//namespace Strack.Service;


///// <summary>
///// 同步第三方平台的活动数据
///// </summary>
//public interface IActivitySyncService
//{
//    /// <summary>
//    /// 是否已同步
//    /// </summary>
//    /// <param name="id">第三方平台活动Id</param>
//    /// <returns></returns>
//    Task<bool> IsSyncedAsync(string id);

//    /// <summary>
//    /// 同步单次训练
//    /// </summary>
//    /// <param name="id">第三方平台活动Id</param>
//    /// <returns></returns>
//    Task SyncAsync(string id);

//    /// <summary>
//    /// 同步所有活动
//    /// </summary>
//    Task SyncAllAsync();
//}

//public class XingZheActivitySyncService(
//    IDbContextFactory<StrackDbContext> dbFactory,
//    IXingZheClient zheClient
//    ) : IActivitySyncService
//{
//    public async Task<bool> IsSyncedAsync(string activityId)
//    {
//        await using var dbContext = await dbFactory.CreateDbContextAsync();
//        //await using var transaction = await dbContext.Database.BeginTransactionAsync();

//        return await dbContext.Sources
//            .AnyAsync(x => x.Type == PlatformType.XingZhe && x.ExternalId == activityId);
//    }

//    public async Task SyncAsync(string workoutId)
//    {
//        await using var dbContext = await dbFactory.CreateDbContextAsync();

//        var isSynced = await dbContext.Sources
//            .AnyAsync(x => x.Type == PlatformType.XingZhe && x.ExternalId == workoutId);

//        if (isSynced) throw new StrackDbException($"行者活动已存在,无法同步:{workoutId}");

//        //ActivityEntity activity = new()
//        //{
//        //    SourceId = Guid.Empty,
//        //     UserId  = 
//        //}

//        //CredentialEntity source = new()
//        //{
//        //    ActivityId
//        //    ExternalId = workoutId,
//        //    Type = PlatformType.XingZhe
//        //};
//    }

//    public Task SyncAllAsync()
//    {
//        throw new NotImplementedException();
//    }
//}




//public interface ISyncService
//{



//    /// <summary>
//    /// 判断行者训练是否已同步
//    /// </summary>
//    /// <param name="workoutId"></param>
//    /// <returns></returns>
//    [Obsolete] Task<bool> IsSyncedFromXingZheAsync(long workoutId);
//    /// <summary>
//    /// 判断迹驰活动是否已同步
//    /// </summary>
//    /// <param name="activityId"></param>
//    /// <returns></returns>
//    [Obsolete] Task<bool> IsSyncedFromIGPSportAsync(long activityId);

//    /// <summary>
//    /// 从行者活动Id获取没有同步的Id
//    /// </summary>
//    /// <param name="workoutIds"></param>
//    /// <returns></returns>
//    [Obsolete] IAsyncEnumerable<T> GetNotSyncFromXingZheAsync<T>(IAsyncEnumerable<T> workoutIds) where T : IIdentifier<long>;
//    /// <summary>
//    /// 从迹驰活动Id获取没有同步的Id
//    /// </summary>
//    /// <param name="activityIds"></param>
//    /// <returns></returns>
//    [Obsolete] IAsyncEnumerable<T> GetNotSyncFromIGPSportAsync<T>(IAsyncEnumerable<T> activityIds) where T : IIdentifier<long>;

//    /// <summary>
//    /// 添加活动
//    /// </summary>
//    /// <param name="entity"></param>
//    /// <returns></returns>
//    [Obsolete] Task AddAsync(ActivityEntity entity);

//    /// <summary>
//    /// 添加一些活动
//    /// </summary>
//    /// <param name="entities"></param>
//    /// <returns></returns>
//    [Obsolete] Task<int> AddRangeAsync(IAsyncEnumerable<ActivityEntity> entities);

//}


//public class SyncService(
//    ILogger<SyncService> logger,
//    IDbContextFactory<StrackDbContext> dbFactory,
//    IXingZheClientProvider xingZheClientProvider
//    ) : ISyncService
//{

//    public async Task SyncFromXingZheUserIdAsync(long userId)
//    {
//        await using var dbContext = await dbFactory.CreateDbContextAsync();
//        await using var transaction = await dbContext.Database.BeginTransactionAsync();

//        //凭证
//        var credential = await dbContext.FindUserCredentialByExternalIdAsync(userId, PlatformType.XingZhe, CredentialType.SessionId);
//        var client = xingZheClientProvider.GetOrCreateFromSessionId(credential);
//        var userInfo = await client.GetUserInfoAsync();

//        dbContext.SaveChanges();
//        await transaction.CommitAsync();
//    }



//    public async Task<bool> IsSyncedFromXingZheAsync(long workoutId)
//    {
//        logger.LogTrace("正在检查行者训练是否同步:{wid}", workoutId);

//        await using var dbContext = await dbFactory.CreateDbContextAsync();

//        var isSynced = await dbContext.XingZheSources
//                   .AsNoTracking()
//                   .AnyAsync(x => x.WorkoutId == workoutId);

//        logger.LogTrace("行者训练已同步:{value}", isSynced);
//        return isSynced;
//    }
//    public async Task<bool> IsSyncedFromIGPSportAsync(long activityId)
//    {
//        logger.LogTrace("正在检查迹驰活动是否同步:{wid}", activityId);

//        await using var dbContext = await dbFactory.CreateDbContextAsync();

//        var isSynced = await dbContext.IGPSportSources
//                   .AsNoTracking()
//                   .AnyAsync(x => x.ActivityId == activityId);

//        logger.LogTrace("迹驰活动已同步:{value}", isSynced);
//        return isSynced;
//    }

//    public async IAsyncEnumerable<T> GetNotSyncFromXingZheAsync<T>(IAsyncEnumerable<T> workoutIds) where T : IIdentifier<long>
//    {
//        logger.LogTrace("正在获取行者未同步训练Id");

//        await using var dbContext = await dbFactory.CreateDbContextAsync();

//        await foreach (var id in workoutIds)
//        {
//            bool isSynced;

//            try
//            {
//                isSynced = await dbContext.XingZheSources.AnyAsync(x => x.WorkoutId == id.Id);
//            }
//            catch (Exception ex)
//            {
//                isSynced = false;
//                logger.LogError(ex, "获取行者训练是否同步失败:{id}", id);
//            }

//            if (!isSynced) yield return id;
//        }

//        logger.LogTrace("行者未同步训练Id获取完成");
//        yield break;
//    }
//    public async IAsyncEnumerable<T> GetNotSyncFromIGPSportAsync<T>(IAsyncEnumerable<T> activityIds) where T : IIdentifier<long>
//    {
//        logger.LogTrace("正在获取迹驰未同步活动Id");

//        await using var dbContext = await dbFactory.CreateDbContextAsync();

//        await foreach (var id in activityIds)
//        {
//            bool isSynced = false;

//            try
//            {
//                isSynced = await dbContext.IGPSportSources.AnyAsync(x => x.ActivityId == id.Id);
//            }
//            catch (Exception ex)
//            {
//                isSynced = false;
//                logger.LogError(ex, "获取迹驰活动是否同步失败:{id}", id);
//            }

//            if (!isSynced) yield return id;
//        }

//        logger.LogTrace("迹驰未同步活动Id获取完成");
//        yield break;
//    }

//    [Obsolete]
//    public async Task AddAsync(ActivityEntity entity)
//    {
//        await using var dbContext = await dbFactory.CreateDbContextAsync();

//        try
//        {
//            //添加实体
//            await dbContext.AddAsync(entity);
//            await dbContext.SaveChangesAsync();

//            logger.LogInformation("活动实体已添加:{eid}", entity.Id);
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex, "活动实体添加失败:{eid}", entity.Id);
//        }
//    }
//    [Obsolete]
//    public async Task<int> AddRangeAsync(IAsyncEnumerable<ActivityEntity> entities)
//    {
//        logger.LogTrace("正在添加活动列表");
//        await using var dbContext = await dbFactory.CreateDbContextAsync();
//        int count = 0;

//        await foreach (var entity in entities)
//        {
//            try
//            {
//                //添加实体
//                await dbContext.AddAsync(entity);
//                await dbContext.SaveChangesAsync();

//                count++;
//                logger.LogInformation("活动实体已添加:{eid}", entity.Id);
//            }
//            catch (Exception ex)
//            {
//                logger.LogError(ex, "活动实体添加失败:{eid}", entity.Id);
//            }
//        }

//        logger.LogTrace("活动已添加,数量:{count}", count);
//        return count;
//    }

//}
