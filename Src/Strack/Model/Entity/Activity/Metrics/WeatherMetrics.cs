using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 天气数据
/// </summary>
[Owned]
public class WeatherMetrics
{
    /// <summary>
    /// 平均温度 (摄氏度)
    /// </summary>
    [Column("TemperatureAvgCelsius")]
    public double? AvgCelsius { get; set; }

    /// <summary>
    /// 最低温度 (摄氏度)
    /// </summary>
    [Column("TemperatureMinCelsius")]
    public double? MinCelsius { get; set; }

    /// <summary>
    /// 最高温度 (摄氏度)
    /// </summary>
    [Column("TemperatureMaxCelsius")]
    public double? MaxCelsius { get; set; }
}