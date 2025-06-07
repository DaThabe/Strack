using Common;
using Common.Model.Hosted;
using Common.Service.Setting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using XingZhe.Model;
using XingZhe.Model.Exception;


namespace XingZhe.Service;

/// <summary>
/// 行者 HttpClient 提供者
/// </summary>
public interface IXingZheClientProvider : IHostedService
{
    /// <summary>
    /// 所有会话信息
    /// </summary>
    IEnumerable<Session> Sessions { get; }

    /// <summary>
    /// 根据条件获取请求客户端
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    IXingZheClient With(Func<Session, bool> matcher);

    /// <summary>
    /// 设置会话Id，并返回对应的请求客户端
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    Task<IXingZheClient> SetSessionIdAsync(string sessionId);
}

/// <summary>
/// 行者 HttpClient 提供者
/// </summary>
/// <param name="services"></param>
/// <param name="logger"></param>
/// <param name="setter"></param>
public class XingZheClientProvider(
    IServiceProvider services,
    ILogger<XingZheClientProvider> logger,
    ISetter<XingZheClientProvider> setter
    ) : OneTimeHostedService, IXingZheClientProvider
{
    public IEnumerable<Session> Sessions => _sessions.Values;

    public IXingZheClient With(Func<Session, bool> matcher)
    {
        var session = _sessions.Values.FirstOrDefault(matcher) ?? throw new XingZheException("没有找到匹配的请求客户端");
        return session.Client;
    }
    public async Task<IXingZheClient> SetSessionIdAsync(string sessionId)
    {
        var client = CreateClient();
        var userInfo = await client.GetUserInfoAsync();

        _sessions[userInfo.Id] = new(sessionId, userInfo, client);
        logger.LogInformation("请求客户端更新成功 会话Id:{sid}, 用户Id:{uid}, 用户名:{name}", sessionId, userInfo.Id, userInfo.Username);

        //存储SessionId
        setter.Set(_sessions.Values.Select(x => x.SessionId).ToArray(), "SessionId");

        return client;


        ////创建Api请求客户端
        XingZheClient CreateClient()
        {
            var cookie = new Cookie
            {
                Name = "sessionid",
                Value = sessionId,
                Path = "/",
                Domain = "imxingzhe.com",
                HttpOnly = true,
                Expires = DateTime.Now.AddMonths(1),
            };

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(new Uri("https://imxingzhe.com/"), cookie);

            var client = new HttpClient(new HttpClientHandler { CookieContainer = cookieContainer });
            return new XingZheClient(services, client);
        }
    }


    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        var sessionIds = setter.Get<string[]>([], "SessionId");

        foreach (var i in sessionIds)
        {
            try
            {
                await SetSessionIdAsync(i);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SessionId添加失败: {SessionId}", i);
            }
        }
    }

    
    //所有会话
    private readonly Dictionary<long, Session> _sessions = [];
}


public static class XingZheClientServiceExtension
{
    /// <summary>
    /// 根据SessionId获取请求客户端
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    public static IXingZheClient WithSessionId(this IXingZheClientProvider service,  string sessionId)
    {
        return service.With(x => x.SessionId.Equals(sessionId, StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// 根据用户Id获取请求客户端
    /// </summary>
    /// <param name="service"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static IXingZheClient WithUserId(this IXingZheClientProvider service, long userId)
    {
        return service.With(x => x.UserInfo.Id == userId);
    }
}