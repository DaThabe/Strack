using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace Common.Service.Setting;


/// <summary>
/// 表示一个设置组
/// </summary>
/// <typeparam name="TGroup"></typeparam>
public interface ISetter<TGroup>
{
    /// <summary>
    /// 获取一个值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="default"></param>
    /// <param name="memberName"></param>
    /// <returns></returns>
    T Get<T>(T @default, [CallerMemberName] string memberName = "");

    /// <summary>
    /// 设置一个值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="memberName"></param>
    void Set<T>(T value, [CallerMemberName] string memberName = "");

    /// <summary>
    /// 删除一个值
    /// </summary>
    /// <param name="memberName"></param>
    void Remove([CallerMemberName] string memberName = "");
}


internal class Setter<TGroup>(ISettingProvider settingService) : ISetter<TGroup>
{
    public T Get<T>(T @default, [CallerMemberName] string memberName = "")
    {
        var path = GetPath(memberName);
        if (settingService.Get(path) is JToken jToken && jToken.ToObject<T>() is T value)
        {
            return value;
        }

        return @default;
    }

    public void Set<T>(T value, [CallerMemberName] string memberName = "")
    {
        var path = GetPath(memberName);
        settingService.Set(path, value);
    }
    
    public void Remove([CallerMemberName] string memberName = "")
    {
        var path = GetPath(memberName);
        settingService.Remove(path);
    }



    private static string GetPath(string memberName = "")
    {
        var type = typeof(TGroup);

        if (string.IsNullOrWhiteSpace(type.Namespace))
        {
            if (string.IsNullOrWhiteSpace(type.Name))
            {
                if(string.IsNullOrWhiteSpace(memberName))
                {
                    return string.Empty;
                }
                return $"{memberName}";
            }
            return $"{type.Name}.{memberName}";
        }
        return $"{type.Namespace}.{type.Name}.{memberName}";
    }
}