using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 持续时间
/// </summary>
[Owned]
public class DurationMetrics
{
    /// <summary>
    /// 总时间 (秒)
    /// </summary>
    [Column("DurationTotalSeconds")]
    public double? TotalSeconds { get; set; }

    /// <summary>
    /// 移动时间 (秒)
    /// </summary>
    [Column("DurationMovingSeconds")]
    public double? MovingSeconds { get; set; }

    /// <summary>
    /// 暂停时间 (秒)
    /// </summary>
    [Column("DurationPauseSeconds")]
    public double? PauseSeconds { get; set; }

    /// <summary>
    /// 下坡时长 (秒)
    /// </summary>
    [Column("DurationDownslopeSeconds")]
    public double? DownslopeSeconds { get; set; }

    /// <summary>
    /// 上坡时长 (秒)
    /// </summary>
    [Column("DurationUpslopeSeconds")]
    public double? UpslopeSeconds { get; set; }

    /// <summary>
    /// 平路时长 (秒)
    /// </summary>
    [Column("DurationFlatSeconds")]
    public double? FlatSeconds { get; set; }
}