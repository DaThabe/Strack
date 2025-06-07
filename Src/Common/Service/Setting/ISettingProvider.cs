using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Service.Setting;

/// <summary>
/// 设置容器
/// </summary>
public interface ISettingProvider : IHostedService
{
    /// <summary>
    /// 获取值
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    JToken? Get(string path);

    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="path"></param>
    /// <param name="value"></param>
    void Set(string path, object? value);

    /// <summary>
    /// 删除值
    /// </summary>
    /// <param name="path"></param>
    void Remove(string path);
}

internal class SettingService(ILogger<SettingService> logger) : ISettingProvider
{
    public JToken? Get(string path)
    {
        try
        {
            var result = _cache.SelectToken(path);

            if (result == null)
            {
                logger.LogWarning("配置路径不存在: {Path}", path);
                return null;
            }

            logger.LogInformation("获取配置成功, 路径:{Path}, 值:{Value}", path, result);
            return result;
        }
        catch
        {
            return null;
        }
    }

    public void Set(string path, object? value)
    {
        var keys = path.Split('.', StringSplitOptions.RemoveEmptyEntries);
        _cache ??= [];
        JToken cur = _cache;


        for (int i = 0; i < keys.Length; i++)
        {
            var key = keys[i];

            if (i == keys.Length - 1)
            {
                // 最后一层，设置值
                ((JObject)cur)[key] = JToken.FromObject(value ?? JValue.CreateNull());
            }
            else
            {
                // 中间层，确保是 JObject 并继续往下
                if (cur[key] is not JObject next)
                {
                    next = [];
                    ((JObject)cur)[key] = next;
                }

                cur = next;
            }
        }

        logger.LogInformation("配置已更新, 路径:{Path}, 值:{Value}", path, value);
        _ = SaveAsync();
    }

    public void Remove(string path)
    {
        var root = _cache.SelectToken(path)?.Parent;
        if (root == null)
        {
            logger.LogWarning("删除失败, 配置路径不存在: {Path}", path);
            return;
        }

        root.Remove();
        logger.LogInformation("配置已删除, 路径:{Path}", path);
    }


    /// <summary>
    /// 启动并加载设置
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await LoadAsync(cancellationToken);
    }

    /// <summary>
    /// 关闭并保存配置
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await SaveAsync();
    }


    //加载
    private async Task LoadAsync(CancellationToken cancellation = default)
    {
        try
        {
            if (!System.IO.File.Exists(_path))
            {
                logger.LogWarning("设置文件不存在");
                return;
            }

            var content = await System.IO.File.ReadAllTextAsync(_path, cancellation);
            var result = JsonConvert.DeserializeObject<JObject>(content);

            if(result == null)
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

    //保存
    private async Task SaveAsync()
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



    private JObject _cache = [];

    private CancellationTokenSource _saveCTS = new();

    private readonly SemaphoreSlim _saveSemaphoreSlim = new(1, 1);

    private readonly string _path = Path.Combine(AppContext.BaseDirectory, "setting.json");
}