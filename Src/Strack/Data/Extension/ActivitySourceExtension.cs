using Microsoft.EntityFrameworkCore;
using Strack.Exceptions;
using Strack.Model.Entity.Activity.Source;
using Strack.Model.Entity.Enum;

namespace Strack.Data.Extension;


/// <summary>
/// 活动来源储存库
/// </summary>
public static class ActivitySourceExtension
{
    /// <summary>
    /// 确保活动来源不存在
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="externalIActivityId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    /// <exception cref="StrackDbException"></exception>
    public static async Task EnsureActivitySourceNotExistsAsync(this StrackDbContext dbContext, long externalIActivityId, PlatformType platform, CancellationToken cancellation = default)
    {
        if (await dbContext.ActivitySources.AnyAsync(x => x.ExternalId == externalIActivityId && x.Platform == platform, cancellationToken: cancellation))
        {
            throw new StrackDbException($"活动记录已存在, Id:{externalIActivityId},平台:{platform}");
        }
    }

    /// <summary>
    /// 判断活动来源是否存在
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="externalIActivityId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static async Task<bool> IsActivitySourceExistAsync(this StrackDbContext dbContext, long externalIActivityId, PlatformType platform, CancellationToken cancellation = default)
    {
        return await dbContext.ActivitySources.AnyAsync(x => x.ExternalId == externalIActivityId && x.Platform == platform, cancellationToken: cancellation);
    }

    /// <summary>
    /// 添加活动来源实体
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="activityEntityId"></param>
    /// <param name="externalActivityId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static async Task<ActivitySourceEntity> AddActivitySourceEntity(this StrackDbContext dbContext, Guid activityEntityId, long externalActivityId, PlatformType platform, CancellationToken cancellation = default)
    {
        var sourceEntity = new ActivitySourceEntity()
        {
            ActivityId = activityEntityId,
            ExternalId = externalActivityId,
            Platform = platform
        };

        await dbContext.AddAsync(sourceEntity, cancellation);
        await dbContext.SaveChangesAsync(cancellation);

        return sourceEntity;
    }
}
