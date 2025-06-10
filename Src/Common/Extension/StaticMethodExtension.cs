using UnitsNet;
using UnitsNet.Units;

namespace Common.Extension;

/// <summary>
/// 静态方法转为扩展方法
/// </summary>
public static class StaticMethodExtension
{
    /// <summary>
    /// 将数字设置为小数点后n位
    /// </summary>
    /// <param name="value"></param>
    /// <param name="digits"></param>
    /// <returns></returns>
    public static double Round(this double value, int digits = 3) => Math.Round(value, digits);


    public static Frequency ToFrequency(this double value, FrequencyUnit unit) => new(value, unit);
    public static Frequency ToFrequency(this int value, FrequencyUnit unit) => new(value, unit);


    public static Length ToLength(this double value, LengthUnit unit) => new(value, unit);
    public static Length ToLength(this int value, LengthUnit unit) => new(value, unit);


    public static Speed ToSpeed(this double value, SpeedUnit unit) => new(value, unit);
    public static Speed ToSpeed(this int value, SpeedUnit unit) => new(value, unit);


    public static Power ToPower(this double value, PowerUnit unit = PowerUnit.Watt) => new(value, unit);
    public static Power ToPower(this int value, PowerUnit unit = PowerUnit.Watt) => new(value, unit);

    public static Temperature ToTemperature(this double value, TemperatureUnit unit = TemperatureUnit.DegreeCelsius) => new(value, unit);
    public static Temperature ToTemperature(this int value, TemperatureUnit unit = TemperatureUnit.DegreeCelsius) => new(value, unit);

    public static Energy ToEnergy(this double value, EnergyUnit unit ) => new(value, unit);
    public static Energy ToEnergy(this int value, EnergyUnit unit) => new(value, unit);

    public static TimeSpan ToTimeSpan(this double value, TimeSpanUnit unit)
    {
        return unit switch
        {
            TimeSpanUnit.Days => TimeSpan.FromDays(value),
            TimeSpanUnit.Hours => TimeSpan.FromHours(value),
            TimeSpanUnit.Minutes => TimeSpan.FromMinutes(value),
            TimeSpanUnit.Seconds => TimeSpan.FromSeconds(value),
            TimeSpanUnit.Milliseconds => TimeSpan.FromMilliseconds(value),
            _ => TimeSpan.Zero
        };
    }
    public static TimeSpan ToTimeSpan(this int value, TimeSpanUnit unit) => ToTimeSpan((double)value, unit);


    public static DateTimeOffset ToDateTimeOffset(this long unixTime, UnixTimeUnit unit = UnixTimeUnit.Seconds)
    {
        return unit switch
        {
            UnixTimeUnit.Seconds => DateTimeOffset.FromUnixTimeSeconds(unixTime),
            UnixTimeUnit.Milliseconds => DateTimeOffset.FromUnixTimeMilliseconds(unixTime),
              _ => DateTimeOffset.MinValue
        };
    }
}

public enum UnixTimeUnit
{
    /// <summary>
    /// 秒
    /// </summary>
    Seconds,

    /// <summary>
    /// 毫秒
    /// </summary>
    Milliseconds
}

public enum TimeSpanUnit
{
    /// <summary>
    /// 天
    /// </summary>
    Days,

    /// <summary>
    /// 小时
    /// </summary>
    Hours,

    /// <summary>
    /// 分钟
    /// </summary>
    Minutes,

    /// <summary>
    /// 秒
    /// </summary>
    Seconds,

    /// <summary>
    /// 毫秒
    /// </summary>
    Milliseconds,

    /// <summary>
    /// 微秒
    /// </summary>
    Microseconds
}
