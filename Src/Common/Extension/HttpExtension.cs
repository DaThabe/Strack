using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Extension;

public static class HttpExtension
{
    /// <summary>
    /// 获取Json数据
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task<JToken> GetJsonAsync(this HttpClient httpClient, string url)
    {
        var resp = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<JToken>(resp) ?? throw new InvalidOperationException($"Url内容无法解析为Json, Url:{url}");
    }
    
    /// <summary>
    /// 获取Json数据
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task<JToken?> GetJsonOrDefaultAsync(this HttpClient httpClient, string url)
    {
        try
        {
            return await httpClient.GetJsonAsync(url);
        }
        catch
        {
            return default;
        }
    }


    public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string url)
    {
        var resp = await httpClient.GetStringAsync(url);
        return JsonConvert.DeserializeObject<T>(resp) ?? throw new InvalidOperationException($"Url内容无法解析为:{typeof(T)}, Url:{url}");
    }
    public static async Task<T> GetJsonOrDefaultAsync<T>(this HttpClient httpClient, string url, T @default) where T : notnull
    {
        try
        {
            return await httpClient.GetJsonAsync<T>(url);
        }
        catch
        {
            return @default;
        }
    }
    public static async Task<T?> GetJsonOrDefaultAsync<T>(this HttpClient httpClient, string url) where T : notnull
    {
        try
        {
            return await httpClient.GetJsonAsync<T>(url);
        }
        catch
        {
            return default;
        }
    }
}
