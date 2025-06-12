using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Service.Setting;

/// <summary>
/// 设置
/// </summary>
public interface ISettingService : IHostedService
{
    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    T? Get<T>(string key);

    /// <summary>
    /// 添加配置
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    bool Set(string key, object? value);

    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="path"></param>
    bool Remove(string key);

    /// <summary>
    /// 保存
    /// </summary>
    Task SaveAsync();

    /// <summary>
    /// 加载
    /// </summary>
    /// <returns></returns>
    Task ReloadAsync();
}

public class SettingService(ILogger<SettingService> logger) : ISettingService
{
    public T? Get<T>(string key)
    {
        try
        {
            if (!_cache.TryGetValue(key, out var value))
            {
                logger.LogWarning("配置获取失败, 键不存在:{key}", key);
                return default;
            }

            logger.LogTrace("获取配置成功, 键:{Path}, 值:{Value}", key, value);
            return value.ToObject<T>();
        }
        catch
        {
            return default;
        }
    }
    public bool Set(string key, object? value)
    {
        key = key.Trim();
        if(string.IsNullOrWhiteSpace(key))
        {
            logger.LogWarning("配置设置失败,键为空");
            return false;
        }

        //更新缓存
        _cache[key] = value is null ? JValue.CreateNull() : JToken.FromObject(value);

        logger.LogInformation("配置已更新, 键:{Path}, 值:{Value}", key, value);
        return true;
    }
    public bool Remove(string key)
    {
        if(!_cache.TryGetValue(key, out var _))
        {
            logger.LogWarning("配置删除失败, 键不存在:{key}", key);
            return false;
        }

        _cache.Remove(key);
        logger.LogInformation("配置已删除:{key}", key);
        return true;
    }
    
    public async Task ReloadAsync()
    {
        try
        {
            if (!System.IO.File.Exists(_path))
            {
                logger.LogWarning("设置文件不存在");
                return;
            }

            var content = await System.IO.File.ReadAllTextAsync(_path);
            var result = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(content);

            if (result == null)
            {
                logger.LogWarning("设置文件内容为空或格式错误");
                return;
            }

            _cache = result;
            logger.LogInformation("设置文件已加载: {Path}", _path);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "加载设置文件时发生错误");
        }
    }
    public async Task SaveAsync()
    {
        await _saveSemaphoreSlim.WaitAsync();
        try
        {
            _saveCTS.Cancel();
            _saveCTS = new CancellationTokenSource();

            await Task.Delay(5, _saveCTS.Token);

            var json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
            await System.IO.File.WriteAllTextAsync(_path, json);

            logger.LogInformation("设置已保存到文件: {Path}", _path);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "保存设置文件时发生错误");
        }
        finally
        {
            _saveSemaphoreSlim.Release();
        }
    }



    /// <summary>
    /// 启动并加载设置
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await ReloadAsync();
    }

    /// <summary>
    /// 关闭并保存配置
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await SaveAsync();
    }

    
    private Dictionary<string, JToken> _cache = [];

    private CancellationTokenSource _saveCTS = new();

    private readonly SemaphoreSlim _saveSemaphoreSlim = new(1, 1);

    private readonly string _path = Path.Combine(AppContext.BaseDirectory, "setting.json");
}


public static class SettingServiceExtension
{
    /// <summary>
    /// 设置之后保存
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static async Task<bool> SetAndSaveAsync(this ISettingService setting, string key, object? value)
    {
        if (setting.Set(key, value))
        {
            await setting.SaveAsync();
            return true;
        }

        return false;
    }

    /// <summary>
    /// 删除之后保存
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static async Task<bool> RemoveAndSaveAsync(this ISettingService setting, string key)
    {
        if (setting.Remove(key))
        {
            await setting.SaveAsync();
            return true;
        }

        return false;
    }
}