using Microsoft.EntityFrameworkCore;
using Strack.Exceptions;
using Strack.Model.Database;
using Strack.Model.Entity;
using Strack.Model.Entity.User;

namespace Strack.Service.Repository;

internal static class RepositoryExtension
{
    //获取或创建用户
    public static async Task<UserEntity> GetOrCreateUserAsync(this StrackDbContext dbContext, long userId, SourceType type)
    {
        var userEntity = await dbContext.Users
            .FirstOrDefaultAsync(x => x.ExternalId == userId);

        if (userEntity != null) return userEntity;

        userEntity = new UserEntity() { ExternalId = userId, Source = type };

        await dbContext.AddAsync(userEntity);
        await dbContext.SaveChangesAsync();

        return userEntity;
    }

    //确保活动不存在
    public static async Task EnsureActivityNotExistsAsync(this StrackDbContext dbContext, long activityId, SourceType sourceType)
    {
        if (await dbContext.Activities.AnyAsync(x => x.ExternalId == activityId && x.Source == sourceType))
        {
            throw new StrackDbException("活动记录已存在");
        }
    }


    /// <summary>
    /// 根据外部Id查找用户实体
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userId"></param>
    /// <param name="sourceType"></param>
    /// <returns></returns>
    public static async Task<UserEntity?> FindUserByExternalIdAsync(this StrackDbContext dbContext, long userId, SourceType sourceType)
    {
        return await dbContext.Users.FirstOrDefaultAsync(x => x.ExternalId == userId && x.Source == sourceType);
    }

    /// <summary>
    /// 根据外部Id查询凭证
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userId"></param>
    /// <param name="sourceType"></param>
    /// <param name="credential"></param>
    /// <returns></returns>
    /// <exception cref="StrackDbException"></exception>
    /// <exception cref="StrackException"></exception>
    public static async Task<string> FindUserCredentialByExternalIdAsync(this StrackDbContext dbContext, long userId, SourceType sourceType, CredentialType credential)
    {
        var userEntity = await dbContext.FindUserByExternalIdAsync(userId, SourceType.XingZhe)
       ?? throw new StrackDbException($"行者用户不存在:{userId}");

        if (userEntity.CredentialType != CredentialType.SessionId) throw new StrackException("不支持的凭证类型");
        if (string.IsNullOrWhiteSpace(userEntity.CredentialContent)) throw new StrackException("无效凭证");

        return userEntity.CredentialContent;
    }
}
