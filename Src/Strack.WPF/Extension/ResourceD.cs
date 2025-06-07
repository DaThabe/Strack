using System.Windows;

namespace Strack.Desktop.Extension;

public static class ResourceDictionaryExtension
{
    public static T Find<T>(this ResourceDictionary resource, string key) where T : notnull
    {
        if (!resource.Contains(key))
        {
            throw new KeyNotFoundException($"资源不存在, Key:{key}");
        }

        return (T)resource[key];
    }

    public static T? FindOrDefault<T>(this ResourceDictionary resource, string key) where T : notnull
    {
        try
        {
            return resource.Find<T>(key);
        }
        catch
        {
            return default;
        }
    }
}
