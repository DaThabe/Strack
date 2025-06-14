using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 坡度
/// </summary>
[Owned]
public class SlopeMetrics
{
    /// <summary>
    /// 平均坡度
    /// </summary>
    [Column("SlopeAvg")]
    public double? Avg { get; set; }

    /// <summary>
    /// 最小坡度
    /// </summary>
    [Column("SlopeMin")]
    public double? Min { get; set; }

    /// <summary>
    /// 最大坡度
    /// </summary>
    [Column("SlopeMax")]
    public double? Max { get; set; }

    /// <summary>
    /// 平均上坡度
    /// </summary>
    [Column("SlopeAvgUpslope")]
    public double? AvgUpslope { get; set; }

    /// <summary>
    /// 平均下坡度
    /// </summary>
    [Column("SlopeAvgDownslope")]
    public double? AvgDownslope { get; set; }

    /// <summary>
    /// 最大上坡度
    /// </summary>
    [Column("SlopeMaxUpslope")]
    public double? MaxUpslope { get; set; }

    /// <summary>
    /// 最大下坡度
    /// </summary>
    [Column("SlopeMaxDownslope")]
    public double? MaxDownslope { get; set; }
}