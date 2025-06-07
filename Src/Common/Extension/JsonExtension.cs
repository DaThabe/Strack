using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Common.Extension;

public static class JsonExtension
{
    /// <summary>
    /// 获取Json路径的值并转为目标类型
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="token">Json</param>
    /// <param name="path">路径</param>
    /// <returns>目标值</returns>
    /// <exception cref="ArgumentException">路径异常</exception>
    /// <exception cref="InvalidCastException">转换异常</exception>
    public static T GetValue<T>(this JToken token, string path)
    {
        var node = token.SelectToken(path) ?? throw new ArgumentException($"Json路径不存在: {path}", nameof(path));

        try
        {
            return node.ToObject<T>() ?? throw new InvalidCastException($"无法将 JSON 值（路径：{path}）转换为类型 {typeof(T).Name}，结果为 null");
        }
        catch (JsonException ex)
        {
            throw new InvalidCastException($"无法将 JSON 值（路径：{path}）转换为类型 {typeof(T).Name}。", ex);
        }
    }

    /// <summary>
    /// 获取Json路径的值并转为目标类型，如果不存在则返回默认值
    /// </summary>
    /// <typeparam name="T">目标值</typeparam>
    /// <param name="token">Json</param>
    /// <param name="path">路径</param>
    /// <param name="default">默认值</param>
    /// <returns>目标值</returns>
    public static T GetValueOrDefault<T>(this JToken token, string path, T @default) where T : notnull
    {
        try
        {
            return token.GetValue<T>(path);
        }
        catch
        {
            return @default;
        }
    }

    /// <summary>
    /// 获取Json路径的值并转为目标类型，如果不存在则返回默认值
    /// </summary>
    /// <typeparam name="T">目标值</typeparam>
    /// <param name="token">Json</param>
    /// <param name="path">路径</param>
    /// <returns>目标值或默认值</returns>
    public static T? GetValueOrDefault<T>(this JToken token, string path) where T : notnull
    {
        try
        {
            return token.GetValue<T>(path);
        }
        catch
        {
            return default;
        }
    }
}
