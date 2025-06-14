using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Extension;

/// <summary>
/// 功率数据
/// </summary>
[Table("ActivityPower")]
public class ActivityPowerEntity : EntityBase
{
    /// <summary>
    /// 最大功率 (瓦)
    /// </summary>
    public double? MaxWatts { get; set; }

    /// <summary>
    /// 平均功率 (瓦)
    /// </summary>
    public double? AvgWatts { get; set; }

    /// <summary>
    /// 阈值功率 (瓦)
    /// </summary>
    public double? FtpWatts { get; set; }


    /// <summary>
    /// Normalized Power（正常化功率）<br/>
    /// 4√(avg(P^4 over 30s rolling window))
    /// </summary>
    /// <remarks>
    /// 将每秒功率进行 30 秒滑动平均 <br/>
    /// 对这些结果的 4 次幂求平均 <br/>
    /// 再对平均值开 4 次方根
    /// </remarks>
    public double? NpWatts { get; set; }

    /// <summary>
    /// Intensity Factor（强度因子）<br/>
    /// IF = NP / FTP <br/>
    /// </summary>
    /// <remarks>
    /// IF = 1.00 表示等于你的阈值强度  <br/>
    /// IF &gt; 1.0 表示超阈值训练（非常疲劳） <br/>
    /// IF &lt; 0.85 通常表示恢复性训练
    /// </remarks>
    public double? If { get; set; }

    /// <summary>
    /// Variability Index（变异指数）
    /// VI = NP / AvgPower <br/>
    /// </summary>
    /// <remarks>
    /// VI = 1：非常稳定（如铁人三项） <br/>
    /// VI > 1.1：表明强度变化大（如间歇训练）
    /// </remarks>
    public double? Vi { get; set; }

    /// <summary>
    /// Training Stress Score（训练压力得分）<br/>
    /// (Duration(s) × NP × IF) / (FTP × 3600) × 100
    /// </summary>
    /// <remarks>
    /// 100 == 1 小时在 FTP 进行全力训练<br/>
    /// &lt; 50：恢复性训练<br/>
    /// 100-150：中高强度<br/>
    /// &gt;150：中高强度
    /// </remarks>
    public int? Tss { get; set; }


    /// <summary>
    /// 活动Id
    /// </summary>
    public required Guid ActivityId { get; set; }

    /// <summary>
    /// 活动
    /// </summary>
    [ForeignKey(nameof(ActivityId))]
    public required ActivityEntity Activity { get; set; }
}
