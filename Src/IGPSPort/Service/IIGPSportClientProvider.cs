using System.Net.Http.Headers;

namespace IGPSport.Service;

public interface IIGPSportClientProvider
{
    /// <summary>
    /// 根据 授权码 获取请求客户端
    /// </summary>
    /// <param name="token">授权码</param>
    /// <returns></returns>
    IIGPSportClient GetOrCreateFromAuthToken(string token);

    /// <summary>
    /// 根据 授权码 删除请求客户端
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    bool RemoveFromAuthToken(string token);
}

public class IGPSportClientProvider(
    IServiceProvider services,
    IIGPSportSetting setting
    ) : IIGPSportClientProvider
{
    public IIGPSportClient GetOrCreateFromAuthToken(string token)
    {
        if (_clients.TryGetValue(token, out var client)) return client;

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client = new IGPSportClient(services, httpClient);

        //更新
        _clients[token] = client;
        setting.AuthTokens = [.. _clients.Keys];

        return client;
    }

    public bool RemoveFromAuthToken(string token)
    {
        if (_clients.Remove(token))
        {
            setting.AuthTokens = [.. _clients.Keys];
            return true;
        }

        return false;
    }



    private readonly Dictionary<string, IIGPSportClient> _clients = [];
}