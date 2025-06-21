using IGPSport.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Strack.Data;
using Strack.Data.Extension;
using Strack.Exceptions;
using Strack.Model.Entity.Enum;
using Strack.Model.Entity.User;
using XingZhe.Service;


namespace Strack.Service.User;

/// <summary>
/// 用户业务
/// </summary>
public interface IUserCredentialService
{
    /// <summary>
    /// 更新或创建凭证
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="type"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    Task<UserEntity> UpsertAsync(PlatformType platform, CredentialType type, string content);
}

public class UserCredentialService(
    ILogger<UserCredentialService> logger,
    IDbContextFactory<StrackDbContext> dbFactory,
    IXingZheClientProvider xingZheClientProvider,
    IIGPSportClientProvider iGPSportClientProvider
    ) : IUserCredentialService
{
    public async Task<UserEntity> UpsertAsync(PlatformType platform, CredentialType type, string content)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();


        UserEntity? user = null;
        logger.LogTrace("正在更新凭证:{platform}-{credential}", platform, type);

        if ((platform, type) == (PlatformType.XingZhe, CredentialType.SessionId))
        {
            var client = xingZheClientProvider.GetOrCreateFromSessionId(content);
            var userInfo = await client.GetUserInfoAsync();

            user = await dbContext.GetOrCreateUserAsync(platform, userInfo.Id, x=>
            {
                x.Name = userInfo.Name;
                x.AvatarUrl = userInfo.AvatarUrl;
            });

            await dbContext.UpdateOrCreateUserCredentialAsync(user.Id, type, content);
        }
        else if ((platform, type) == (PlatformType.IGPSport, CredentialType.AuthToken))
        {
            var client = iGPSportClientProvider.GetOrCreateFromAuthToken(content);
            var userInfo = await client.GetUserInfoAsync();

            user = await dbContext.GetOrCreateUserAsync(platform, userInfo.Id, x =>
            {
                x.Name = userInfo.NickName;
                x.AvatarUrl = userInfo.AvatarUrl;
            });

            await dbContext.UpdateOrCreateUserCredentialAsync(user.Id, type, content);
        }
        else
        {
            throw new StrackDbException($"不支持的平台凭证:{platform}-{type}");
        }

        await transaction.CommitAsync();
        logger.LogTrace("正在已凭证:{platform}-{credential}", platform, type);

        return user ?? throw new StrackDbException("未能更新用户凭证");
    }
}