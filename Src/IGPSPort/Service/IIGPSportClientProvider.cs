using Common;
using Common.Model.Hosted;
using Common.Service.Setting;
using IGPSport.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using XingZhe.Model.Exception;

namespace IGPSport.Service;

public interface IIGPSportClientProvider : IHostedService
{
    /// <summary>
    /// 所有会话
    /// </summary>
    IEnumerable<Session> Sessions { get; }

    /// <summary>
    /// 根据条件使用请求客户端
    /// </summary>
    /// <param name="matcher">条件</param>
    /// <returns></returns>
    IIGPSportClient With(Func<Session, bool> matcher);

    /// <summary>
    /// 根据 授权码 获取请求客户端
    /// </summary>
    /// <param name="token">授权码</param>
    /// <returns></returns>
    Task<IIGPSportClient> SetAuthorizationAsync(string token);
}

public class IGPSportClientProvider(
    IServiceProvider services,
    ILogger<IGPSportClientProvider> logger,
    ISetter<IGPSportClientProvider> setter
    ) : OneTimeHostedService, IIGPSportClientProvider
{
    public IEnumerable<Session> Sessions => _sessions.Values;

    public IIGPSportClient With(Func<Session, bool> matcher)
    {
        var session = _sessions.Values.FirstOrDefault(matcher) ?? throw new IGPSportException("没有找到匹配的请求客户端");
        return session.Client;
    }
    public async Task<IIGPSportClient> SetAuthorizationAsync(string token)
    {
        var client = CreateClient();
        var userInfo = await client.GetUserInfoAsync();

        _sessions[userInfo.Id] = new(token , userInfo, client);
        logger.LogInformation("请求客户端更新成功, 授权码:{token}, 用户Id:{uid}, 用户名:{name}", token , userInfo.Id, userInfo.NickName);

        //存储授权码
        setter.Set(_sessions.Values.Select(x => x.Authorization).ToArray(), "Authorization");

        return client;


        //创建Api请求客户端
        IGPSportClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token );

            return new IGPSportClient(services, client);
        }
    }


    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        var authorizations = setter.Get<string[]>([], "Authorization");

        foreach (var i in authorizations)
        {
            try
            {
                await SetAuthorizationAsync(i);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Authorization 验证失败: {authorizations}", i);
            }
        }
    }


    //所有会话
    private readonly Dictionary<long, Session> _sessions = [];
}

public static class IGPSportClientProviderExtension
{
    /// <summary>
    /// 根据 Authorization 获取请求客户端
    /// </summary>
    /// <param name="authorization"></param>
    /// <returns></returns>
    public static IIGPSportClient WithAuthorizationId(this IIGPSportClientProvider service, string authorization)
    {
        return service.With(x => x.Authorization.Equals(authorization, StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// 根据用户Id获取请求客户端
    /// </summary>
    /// <param name="service"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static IIGPSportClient WithUserId(this IIGPSportClientProvider service, long userId)
    {
        return service.With(x => x.UserInfo.Id == userId);
    }
}