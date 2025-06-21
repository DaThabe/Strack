using IGPSport.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strack.Data;
using Strack.Data.Extension;
using Strack.Exceptions;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Enum;
using XingZhe.Service;

namespace Strack.Service.Activity;


/// <summary>
/// 活动导入业务
/// </summary>
public interface IActivityImportService
{
    /// <summary>
    /// 导入活动
    /// </summary>
    /// <returns></returns>
    Task<ActivityEntity> ImportAsync(PlatformType platform, long userId, long activityId, CancellationToken cancellation = default);
}

/// <summary>
/// 迹驰活动
/// </summary>
/// <param name="client"></param>
public class ActivityImportService(
    ILogger<ActivityImportService> logger,
    IDbContextFactory<StrackDbContext> dbFactory,
    IActivityProvider activityProvider
    ) : IActivityImportService
{
    public async Task<ActivityEntity> ImportAsync(PlatformType platform, long userId, long activityId, CancellationToken cancellation = default)
    {
        //连接数据库
        await using var dbContext = await dbFactory.CreateDbContextAsync(cancellation);
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellation);

        //确保活动不存在
        await dbContext.EnsureActivitySourceNotExistsAsync(activityId, platform, cancellation: cancellation);

        //获取用户
        var userEntity = await dbContext.FindUserByExternalIdAsync(platform, userId, x => x.Include(x => x.Credential), cancellation: cancellation);
        var credential = userEntity.Credential ?? throw new StrackDbException($"用户凭证不存在:{platform}-{userId}");

        //添加活动
        var activityEntity = await activityProvider.GetAsync(platform, userId, activityId, cancellation);
        await dbContext.AddActivityEntityAsync(activityEntity, cancellation: cancellation);
        //添加来源
        _ = await dbContext.AddActivitySourceEntity(activityEntity.Id, activityId, platform, cancellation: cancellation);

        //保存
        await transaction.CommitAsync(cancellation);

        logger.LogInformation("迹驰活动已添加:{entity}", activityEntity);
        return activityEntity;
    }
}