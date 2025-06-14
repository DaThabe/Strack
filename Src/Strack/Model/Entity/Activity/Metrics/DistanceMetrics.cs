using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 距离
/// </summary>
[Owned]
public class DistanceMetrics
{
    /// <summary>
    /// 总距离（米）
    /// </summary>
    [Column("DistanceTotalMeters")]
    public double? TotalMeters { get; set; }

    /// <summary>
    /// 下坡距离（米）
    /// </summary>
    [Column("DistanceDownslopeMeters")]
    public double? DownslopeMeters { get; set; }

    /// <summary>
    /// 上坡距离（米）
    /// </summary>
    [Column("DistanceUpslopeMeters")]
    public double? UpslopeMeters { get; set; }

    /// <summary>
    /// 平路距离（米）
    /// </summary>
    [Column("DistanceFlatMeters")]
    public double? FlatMeters { get; set; }
}