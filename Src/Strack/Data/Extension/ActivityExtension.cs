using Microsoft.EntityFrameworkCore;
using Strack.Data.Queryable;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Enum;


namespace Strack.Data.Extension;

public static class ActivityExtension
{
    /// <summary>
    /// 添加活动实体
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userEntityId"></param>
    /// <param name="detail"></param>
    /// <param name="records"></param>
    /// <returns></returns>
    public static async Task<ActivityEntity> AddActivityEntityAsync(this StrackDbContext dbContext, ActivityEntity entity, CancellationToken cancellation = default)
    {
        await dbContext.AddAsync(entity, cancellation);
        await dbContext.SaveChangesAsync(cancellation);

        return entity;
    }


    /// <summary>
    /// 根据实体Id查询活动
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="entityId"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static Task<ActivityEntity> FindActivityByEntityIdAsync(this StrackDbContext dbContext, Guid entityId, EntityQueryableOptionHandler<ActivityEntity>? option = null, CancellationToken cancellation = default)
    {
        return dbContext.Activities
            .AsNoTracking()
            .Option(option)
            .FirstAsync(x => x.Id == entityId, cancellationToken: cancellation);
    }

    /// <summary>
    /// 根据第三方平台活动Id查询
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="externalId"></param>
    /// <param name="platform"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static Task<ActivityEntity> FindActivityByExternalIdAsync(this StrackDbContext dbContext, long externalId, PlatformType platform, EntityQueryableOptionHandler<ActivityEntity>? option = null, CancellationToken cancellation = default)
    {
        return dbContext.Activities
            .Include(x => x.User)
            .Option(option)
            .FirstAsync(x => x.User.ExternalId == externalId && x.User.Platform == platform, cancellationToken: cancellation);
    }

    /// <summary>
    /// 查询指定用户下的所有活动
    /// </summary>
    /// <param name="userEntityId"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static IAsyncEnumerable<ActivityEntity> FindAllActivityByUserEntityIdAsync(this StrackDbContext dbContext, Guid userEntityId, EntityQueryableOptionHandler<ActivityEntity>? option = null)
    {
        return dbContext.Activities
            .Include(x => x.User)
            .Option(option)
            .Where(x => x.UserId == userEntityId)
            .AsAsyncEnumerable();
    }
}
