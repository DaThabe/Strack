using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Strack.Service;
using System.Net;

namespace Strack.External.Xingzhe;

public interface IXingzheHttpClient : IHostedService
{
    Task<JToken> GetJsonAsync(long userId, string url);
    Task<string> GetStringAsync(long userId, string url);
}

internal class XingzheHttpClient(
    ILogger<XingzheHttpClient> logger,
    IOptionsMonitor<HttpClientStrings> setting
    ) : OneTimeHostedService, IXingzheHttpClient
{
    public async Task<JToken> GetJsonAsync(long userId, string url)
    {
        var jsonString = await GetStringAsync(userId, url);
        return JsonConvert.DeserializeObject<JToken>(jsonString) ?? throw new InvalidOperationException($"Url内容无法解析为Json, Url:{url}");
    }
    public Task<string> GetStringAsync(long userId, string url)
    {
        return GetAsync(userId, req => req.GetStringAsync(url));
    }


    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        //更新Sessions
        await UpdateAsync(setting.CurrentValue.SessionIds);
        setting.OnChange(async (values, change) => await UpdateAsync(setting.CurrentValue.SessionIds));
    }

    private Task<T> GetAsync<T>(long userId, Func<HttpClient, Task<T>> action)
    {
        var context = _sessionContexts.Values
            .FirstOrDefault(x => x.UserId == userId)
            ?? throw new InvalidOperationException("当前用户不存在会话Id");

        //if (!_sessionContexts.TryGetValue(sessionId, out var context))
        //{
        //    throw new InvalidOperationException("当前用户不存在会话Id");
        //}

        return action(context.HttpClient);
    }
    private async Task UpdateAsync(string[] sessions)
    {
        foreach(var sessionId in sessions)
        {
            try
            {
                await UpdateSessionContext(sessionId);
            }
            catch
            {
                logger.LogWarning("无效会话Id:{id}", sessionId);
            }
        }
    }
    private async Task UpdateSessionContext(string sessionId)
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

        var content = await client.GetStringAsync("https://imxingzhe.com/api/v1/user/user_info/");
        long userId = ((dynamic?)JsonConvert.DeserializeObject<JToken>(content))?.data.id;

        _sessionContexts[sessionId] = new(sessionId, userId, client);
        logger.LogInformation("会话信息更新成功 Id:{}, 用户Id:{uid}", sessionId, userId);
    }


    public record SessionContext(string SessionId, long UserId, HttpClient HttpClient);
    private readonly Dictionary<string, SessionContext> _sessionContexts = [];
}

internal class HttpClientStrings
{
    public string[] SessionIds { get; set; } = [];
}