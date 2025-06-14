using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;


/// <summary>
/// 高程
/// </summary>
[Owned]
public class ElevationMetrics
{
    /// <summary>
    /// 总升高度 (米)
    /// </summary>
    [Column("ElevationAscentMeters")]
    public double? AscentHeightMeters { get; set; }

    /// <summary>
    /// 总降高度 (米)
    /// </summary>
    [Column("ElevationDescentMeters")]
    public double? DescentHeightMeters { get; set; }
}