using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Numerics;

namespace Common.Extension;

public static class NullableExtension
{
    /// <summary>
    /// 转为 double 或默认值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double? ToDoubleOrDefault(this string? value)
    {
        if(string.IsNullOrWhiteSpace(value)) return null;

        if (double.TryParse(value, out var result)) return result;
        return null;
    }

    /// <summary>
    /// 转为 DateTimeOffset 或默认值
    /// </summary>
    /// <param name="timeString"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static DateTimeOffset? ToDateTimeOffsetOrDefault(this string? timeString, string format = "yyyy-MM-ddTHH:mm:ssZ")
    {
        try
        {
            if (string.IsNullOrWhiteSpace(timeString)) return null;
            var dateTime = DateTimeOffset.ParseExact(timeString, format, CultureInfo.InvariantCulture);
            return dateTime;
        }
        catch
        {
            return null;
        }
    }


    

}