using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 踏频数据
/// </summary>
[Owned]
public class CadenceMetrics
{
    /// <summary>
    /// 最高踏频 (圈/分)
    /// </summary>
    [Column("CadenceMaxCpm")]
    public double? MaxCpm { get; set; }

    /// <summary>
    /// 平均踏频 (圈/分)
    /// </summary>
    [Column("CadenceAvgCpm")]
    public double? AvgCpm { get; set; }
}