using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Metrics;

/// <summary>
/// 功率数据
/// </summary>
[Owned]
public class PowerMetrics
{
    /// <summary>
    /// 最大功率 (瓦)
    /// </summary>
    [Column("PowerMaxWatts")]
    public double? MaxWatts { get; set; }

    /// <summary>
    /// 平均功率 (瓦)
    /// </summary>
    [Column("PowerAvgWatts")]
    public double? AvgWatts { get; set; }

    /// <summary>
    /// 阈值功率 (瓦)
    /// </summary>
    [Column("PowerFtpWatts")]
    public double? FtpWatts { get; set; }

    /// <summary>
    /// Normalized Power（正常化功率）
    /// 4√(avg(P^4 over 30s rolling window))
    /// </summary>
    /// <remarks>
    /// 将每秒功率进行 30 秒滑动平均
    /// 对这些结果的 4 次幂求平均
    /// 再对平均值开 4 次方根
    /// </remarks>
    [Column("PowerNpWatts")]
    public double? NpWatts { get; set; }

    /// <summary>
    /// Intensity Factor（强度因子）
    /// IF = NP / FTP
    /// </summary>
    /// <remarks>
    /// IF = 1.00 表示等于你的阈值强度
    /// IF > 1.0 表示超阈值训练（非常疲劳）
    /// IF < 0.85 通常表示恢复性训练
    /// </remarks>
    [Column("PowerIf")]
    public double? If { get; set; }

    /// <summary>
    /// Variability Index（变异指数）
    /// VI = NP / AvgPower
    /// </summary>
    /// <remarks>
    /// VI = 1：非常稳定（如铁人三项）
    /// VI > 1.1：表明强度变化大（如间歇训练）
    /// </remarks>
    [Column("PowerVi")]
    public double? Vi { get; set; }

    /// <summary>
    /// Training Stress Score（训练压力得分）
    /// (Duration(s) × NP × IF) / (FTP × 3600) × 100
    /// </summary>
    /// <remarks>
    /// 100 == 1 小时在 FTP 进行全力训练
    /// < 50：恢复性训练
    /// 100-150：中高强度
    /// >150：中高强度
    /// </remarks>
    [Column("PowerTss")]
    public int? Tss { get; set; }
}
