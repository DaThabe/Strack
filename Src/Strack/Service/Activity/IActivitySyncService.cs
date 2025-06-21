using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strack.Data;
using Strack.Data.Extension;
using Strack.Model.Entity.Enum;
using System.Runtime.CompilerServices;

namespace Strack.Service.Activity;


/// <summary>
/// 活动同步业务
/// </summary>
public interface IActivitySyncService
{
    /// <summary>
    /// 同步所有训练
    /// </summary>
    /// <param name="progress"></param>
    /// <returns></returns>
    Task<int> SyncAsync(PlatformType platform, long userId, IProgress<ActivitySyncInfo>? progress = null, CancellationToken cancellation = default);
}

public class ActivitySyncService(
    ILogger<ActivitySyncService> logger,
    IDbContextFactory<StrackDbContext> dbFactory,
    IActivityProvider activityProvider,
    IActivityImportService activityImport
    ) : IActivitySyncService
{
    public async Task<int> SyncAsync(PlatformType platform, long userId, IProgress<ActivitySyncInfo>? progress = null, CancellationToken cancellation = default)
    {
        //同步列表
        var syncIdList = await GetNotSyncActivityIds(platform, userId, cancellation).ToListAsync(cancellationToken: cancellation);
        //成功数量
        int completedCount = 0;
        //全部数量
        int totalCount = syncIdList.Count;

         foreach(var activityId in syncIdList)
        {
            try
            {
                progress?.Report(new()
                {
                    ActivityId = activityId,
                    Platform = platform,

                    State = ActivitySyncState.Syncing,
                    Total = totalCount,
                    Completed = completedCount,
                });

                await activityImport.ImportAsync(platform, userId, activityId, cancellation);
                completedCount++;

                progress?.Report(new()
                {
                    ActivityId = activityId,
                    Platform = platform,

                    State = ActivitySyncState.Completed,
                    Total = totalCount,
                    Completed = completedCount,
                });
            }
            catch (Exception ex)
            {
                progress?.Report(new()
                {
                    ActivityId = activityId,
                    Platform = platform,

                    State = ActivitySyncState.Completed,
                    Total = totalCount,
                    Completed = completedCount,
                    Message = ex.Message
                });

                logger.LogError(ex, "活动同步失败:{platform}-{activity}", platform, activityId);
            }
        }

        logger.LogInformation("活动同步完成,成功{count}个", completedCount);
        return completedCount;
    }

    //获取未同步Id
    private async IAsyncEnumerable<long> GetNotSyncActivityIds(PlatformType platform, long userId, [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync(cancellation);

        var notSyncActivityIds = activityProvider.GetActivityIdsAsync(platform, userId, cancellation)
            .WhereAwait(async x => !await dbContext.IsActivitySourceExistAsync(x, platform, cancellation));

        await foreach (var i in notSyncActivityIds) yield return i;
    }
}



/// <summary>
/// 同步信息
/// </summary>
public class ActivitySyncInfo
{
    /// <summary>
    /// 活动平台
    /// </summary>
    public PlatformType Platform { get; set; }
    /// <summary>
    /// 活动Id
    /// </summary>
    public long ActivityId { get; set; }
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 总数
    /// </summary>
    public required int Total { get; set; }
    /// <summary>
    /// 已完成的数量
    /// </summary>
    public required int Completed { get; set; }
    /// <summary>
    /// 同步状态
    /// </summary>
    public required ActivitySyncState State { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }
}

/// <summary>
/// 同步状态
/// </summary>
public enum ActivitySyncState
{
    /// <summary>
    /// 同步中
    /// </summary>
    Syncing,

    /// <summary>
    /// 已完成
    /// </summary>
    Completed,

    /// <summary>
    /// 同步失败
    /// </summary>
    Failed
}
