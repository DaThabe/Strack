using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 海拔
/// </summary>
[Owned]
public class AltitudeMetrics
{
    /// <summary>
    /// 平均海拔 (米)
    /// </summary>
    [Column("AltitudeAvgMeters")]
    public double? AvgMeters { get; set; }

    /// <summary>
    /// 最低海拔 (米)
    /// </summary>
    [Column("AltitudeMinMeters")]
    public double? MinMeters { get; set; }

    /// <summary>
    /// 最高海拔 (米)
    /// </summary>
    [Column("AltitudeMaxMeters")]
    public double? MaxMeters { get; set; }
}