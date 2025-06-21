using Microsoft.EntityFrameworkCore;
using Strack.Data.Queryable;
using Strack.Model.Entity.Enum;
using Strack.Model.Entity.User;

namespace Strack.Data.Extension;

public static class UserExtension
{
    /// <summary>
    /// 获取或创建用户实体
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static async Task<UserEntity> GetOrCreateUserAsync(this StrackDbContext dbContext, PlatformType platform, long userId, Action<UserEntity>? option = null, CancellationToken cancellation = default)
    {
        var userEntity = await dbContext.Users
            .FirstOrDefaultAsync(x => x.ExternalId == userId, cancellationToken: cancellation);

        if (userEntity != null) return userEntity;

        userEntity = new UserEntity()
        {
            ExternalId = userId,
            Platform = platform
        };
        option?.Invoke(userEntity);

        await dbContext.AddAsync(userEntity, cancellation);
        await dbContext.SaveChangesAsync(cancellation);

        return userEntity;
    }

    /// <summary>
    /// 根据第三方用户Id查询
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="platform"></param>
    /// <param name="userId"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static async Task<UserEntity> FindUserByEntityIdAsync(this StrackDbContext dbContext, Guid userEntityId, EntityQueryableOptionHandler<UserEntity>? option = null, CancellationToken cancellation = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Option(option)
            .FirstAsync(x => x.Id == userEntityId, cancellationToken: cancellation);
    }

    /// <summary>
    /// 根据第三方用户Id查询
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="platform"></param>
    /// <param name="userId"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static async Task<UserEntity> FindUserByExternalIdAsync(this StrackDbContext dbContext, PlatformType platform, long userId, EntityQueryableOptionHandler<UserEntity> ? option = null,CancellationToken cancellation = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Option(option)
            .FirstAsync(x => x.ExternalId == userId && x.Platform == platform, cancellationToken: cancellation);
    }

    /// <summary>
    /// 获取所有用户
    /// </summary>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    public static async Task<List<UserEntity>> GetAllUserAsync(this StrackDbContext dbContext, CancellationToken cancellation = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Include(x => x.Activities)
            .Include(x => x.Credential)
            .ToListAsync(cancellationToken: cancellation);
    }
}