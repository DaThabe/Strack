using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 速度数据
/// </summary>
[Owned]
public class SpeedMetrics
{
    /// <summary>
    /// 平均速度（千米/小时）
    /// </summary>
    [Column("SpeedAvgKph")]
    public double? AvgKph { get; set; }

    /// <summary>
    /// 最大速度（千米/小时）
    /// </summary>
    [Column("SpeedMaxKph")]
    public double? MaxKph { get; set; }

    /// <summary>
    /// 平均上升速度（米/小时）
    /// </summary>
    [Column("SpeedAvgAscentMph")]
    public double? AvgAscentSpeed { get; set; }

    /// <summary>
    /// 最快上升速度（米/小时）
    /// </summary>
    [Column("SpeedMaxAscentMph")]
    public double? MaxAscentSpeed { get; set; }

    /// <summary>
    /// 平均下降速度（米/小时）
    /// </summary>
    [Column("SpeedAvgDescentMph")]
    public double? AvgDescentSpeed { get; set; }

    /// <summary>
    /// 最快下降速度（米/小时）
    /// </summary>
    [Column("SpeedMaxDescentMph")]
    public double? MaxDescentSpeed { get; set; }
}
