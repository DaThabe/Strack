using System.Net;


namespace XingZhe.Service;

/// <summary>
/// 行者 HttpClient 提供者
/// </summary>
public interface IXingZheClientProvider
{
    /// <summary>
    /// 根据会话Id获取请求或创建客户端
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    IXingZheClient GetOrCreateFromSessionId(string sessionId);

    /// <summary>
    /// 从SessionId 删除
    /// </summary>
    /// <returns></returns>
    bool RemoveFromSessionId(string sessionId);
}

/// <summary>
/// 行者 HttpClient 提供者
/// </summary>
public class XingZheClientProvider(
    IServiceProvider services, 
    IXingZheSetting setting) : IXingZheClientProvider
{
    public IXingZheClient GetOrCreateFromSessionId(string sessionId)
    {
        sessionId = sessionId.Trim();
        if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentNullException("会话Id为空");

        if (_cache.TryGetValue(sessionId, out var client)) return client;

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

        //创建新的
        var httpClient = new HttpClient(new HttpClientHandler { CookieContainer = cookieContainer });
        client = new XingZheClient(services, httpClient);
        _cache[sessionId] = client;
        setting.SessionIds = [.. _cache.Keys];

        return client;
    }

    public bool RemoveFromSessionId(string sessionId)
    {
        if(_cache.Remove(sessionId))
        {
            setting.SessionIds = [.. _cache.Keys];
            return true;
        }

        return false;
    }


    private readonly Dictionary<string, IXingZheClient> _cache = [];
}