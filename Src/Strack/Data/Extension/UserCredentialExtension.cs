using Microsoft.EntityFrameworkCore;
using Strack.Model.Entity.Enum;
using Strack.Model.Entity.User.Credential;

namespace Strack.Data.Extension;


public static class UserCredentialExtension
{
    /// <summary>
    /// 用户凭证是否存在
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userEntityId"></param>
    /// <param name="type"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static Task<bool> IsUserCredentialExistAsync(this StrackDbContext dbContext, Guid userEntityId, CredentialType type)
    {
        return dbContext.UserCredentials.AnyAsync(x => x.UserId == userEntityId && x.Type == type);
    }

    /// <summary>
    /// 查询用户凭证
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userEntityId"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Task<UserCredentialEntity?> FindUserCredentialAsync(this StrackDbContext dbContext, Guid userEntityId, CredentialType type)
    {
        return dbContext.UserCredentials.FirstOrDefaultAsync(x => x.UserId == userEntityId && x.Type == type);
    }

    /// <summary>
    /// 更新或创建用户凭证
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="userEntityId"></param>
    /// <param name="type"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static async Task<UserCredentialEntity> UpdateOrCreateUserCredentialAsync(this StrackDbContext dbContext, Guid userEntityId, CredentialType type, string content)
    {
        var userCredentialEntity = await dbContext.FindUserCredentialAsync(userEntityId, type);
        if (userCredentialEntity == null)
        {
            userCredentialEntity = new()
            {
                Content = content,
                Type = type,
                UserId = userEntityId
            };

            await dbContext.AddAsync(userCredentialEntity);
        }
        else
        {
            userCredentialEntity.Content = content;
        }

        await dbContext.SaveChangesAsync();
        return userCredentialEntity;
    }

    /// <summary>
    /// 获取用户凭证内容
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="type"></param>
    /// <param name="externalUserId"></param>
    /// <returns></returns>
    public static Task<UserCredentialEntity> FindUserCredentialAsync(this StrackDbContext dbContext, PlatformType platform, CredentialType type, long externalUserId)
    {
        return dbContext.UserCredentials
            .AsNoTracking()
            .Include(x => x.User)
            .FirstAsync(x => x.User.ExternalId == externalUserId && x.User.Platform == platform && x.Type == type);
    }
}