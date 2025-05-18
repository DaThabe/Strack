using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Strack.Model.Entity.Activity;
using Strack.Model.Entity.Sampling.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Sampling;

/// <summary>
/// 采样点
/// </summary>
[Table("Sampling")]
public class SamplingEntity : EntityBase
{
    /// <summary>
    /// 时间（UTC）
    /// </summary>
    public required DateTime TimestampUTC { get; set; }


    /// <summary>
    /// 维度（度）
    /// </summary>
    public double LatitudeDegrees { get; set; }

    /// <summary>
    /// 经度（度）
    /// </summary>
    public double LongitudeDegrees { get; set; }

    /// <summary>
    /// 海拔高度（米）
    /// </summary>
    public double? AltitudeMeters { get; set; }


    /// <summary>
    /// 速度（米/秒）
    /// </summary>
    public double? SpeedMetersPerSecond { get; set; }

    /// <summary>
    /// 距离（米）
    /// </summary>
    public double? DistanceMeters { get; set; }


    /// <summary>
    /// 温度（摄氏度）
    /// </summary>
    public double? TemperatureCelsius { get; set; }

    /// <summary>
    /// 心率（次/分钟）
    /// </summary>
    public int? HeartrateBeatPerMinute { get; set; }




    /// <summary>
    /// 活动Id
    /// </summary>
    public required Guid ActivityId { get; set; }

    /// <summary>
    /// 活动
    /// </summary>
    [ForeignKey(nameof(ActivityId))]
    public required ActivityEntity Activity { get; set; }


    /// <summary>
    /// 骑行扩展数据
    /// </summary>
    public CyclingSamplingDataEntity? CyclingData { get; set; }
}