using IGPSport.Service;
using Microsoft.EntityFrameworkCore;
using Strack.Model.Database;
using Strack.Model.Entity;
using Strack.Service.Repository;
using XingZhe.Service;

namespace Strack.Service;

public interface IUserService
{
    /// <summary>
    /// 从行者SessionId更新 (没有则创建
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    Task UpdateFromXingZheSessionIdAsync(string sessionId);

    /// <summary>
    /// 从迹驰授权码更新 (没有则创建
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task UpdateFromIGPSportAuthToken(string token);
}


public class UserService(
    IDbContextFactory<StrackDbContext> dbFactory,
    IXingZheClientProvider xingZheClientProvider,
    IIGPSportClientProvider iGPSportClientProvider
    ) : IUserService
{
    public async Task UpdateFromXingZheSessionIdAsync(string sessionId)
    {
        var client = xingZheClientProvider.GetOrCreateFromSessionId(sessionId);
        var userInfo = await client.GetUserInfoAsync();

        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();


        var userEntity = await dbContext.GetOrCreateUserAsync(userInfo.Id, SourceType.XingZhe);
        userEntity.CredentialContent = sessionId;
        userEntity.CredentialType = CredentialType.SessionId;

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task UpdateFromIGPSportAuthToken(string token)
    {
        var client = iGPSportClientProvider.GetOrCreateFromAuthToken(token);
        var userInfo = await client.GetUserInfoAsync();

        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();


        var userEntity = await dbContext.GetOrCreateUserAsync(userInfo.Id, SourceType.XingZhe);
        userEntity.CredentialContent = token;
        userEntity.CredentialType = CredentialType.AuthToken;

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
