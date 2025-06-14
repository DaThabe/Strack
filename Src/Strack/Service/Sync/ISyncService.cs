using IGPSport.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Strack.Data;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Enum;
using Strack.Model.Sync;
using Strack.Service.Import;
using Strack.Service.Sync.Activity;
using XingZhe.Service;

namespace Strack.Service.Sync;


/// <summary>
/// 同步服务
/// </summary>
public interface ISyncService
{
    /// <summary>
    /// 同步所有训练
    /// </summary>
    /// <param name="getter"></param>
    /// <param name="progress"></param>
    /// <returns></returns>
    Task<int> SyncAsync(IProgress<SyncInfo<IActivitySummary>>? progress = null);
}

public class SyncService(
    IServiceProvider services,
    PlatformType platformType,
    IActivitiesGetterService getter,
    IActivityAdderService adder
    ) : ISyncService
{
    public async Task<int> SyncAsync(IProgress<SyncInfo<IActivitySummary>>? progress = null)
    {
        logger.LogTrace("正在获取同步列表");

        var syncList = await GetNotSyncActivityList();
        logger.LogTrace("{count}个元素等待同步", syncList.Count);

        //成功数量
        int completedCount = 0;

        for (int i = 0; i < syncList.Count; i++)
        {
            var activity = syncList[i];

            try
            {
                await adder.AddAsync(activity);

                progress?.Report(new()
                {
                    Total = syncList.Count,
                    Completed = i + 1,
                    Item = activity,
                    IsSuccess = true
                });
                completedCount++;
            }
            catch (Exception ex)
            {
                progress?.Report(new()
                {
                    Total = syncList.Count,
                    Completed = i,
                    IsSuccess = false,
                    Message = ex.Message
                });

                logger.LogError(ex, "活动同步失败:{activity}", activity);
            }
        }

        logger.LogInformation("活动同步完成,共{count}个", completedCount);
        return completedCount;
    }

    /// <summary>
    /// 获取未同步列表
    /// </summary>
    /// <returns></returns>
    private async Task<List<IActivitySummary>> GetNotSyncActivityList()
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        return await getter.GetActivitiesAsync()
            .WhereAwait(async x => !await dbContext.IsActivitySourceExistAsync(x.Id, platformType))
            .ToListAsync();
    }


    private readonly IDbContextFactory<StrackDbContext> dbFactory = services.GetRequiredService<IDbContextFactory<StrackDbContext>>();
    private readonly ILogger<SyncService> logger = services.GetRequiredService<ILogger<SyncService>>();
}