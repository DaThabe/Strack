using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 心率数据
/// </summary>
[Owned]
public class HeartrateMetrics
{
    /// <summary>
    /// 平均心率 (次/分)
    /// </summary>
    [Column("HeartrateAvgBpm")]
    public double? AvgBpm { get; set; }

    /// <summary>
    /// 最低心率 (次/分)
    /// </summary>
    [Column("HeartrateMinBpm")]
    public double? MinBpm { get; set; }

    /// <summary>
    /// 最高心率 (次/分)
    /// </summary>
    [Column("HeartrateMaxBpm")]
    public double? MaxBpm { get; set; }
}
