using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Data.Cycling;

/// <summary>
/// 骑行活动数据
/// </summary>
[Table("ActivityCyclingData")]
public class CyclingActivityDataEntity : ActivityDataEntity
{
    #region --踏频--

    /// <summary>
    /// 平均踏频（圈/分钟）
    /// </summary>
    public double AvgCadenceCyclesPerMinute { get; set; }

    /// <summary>
    /// 最大踏频（圈/分钟）
    /// </summary>
    public double MaxCadenceCyclesPerMinute { get; set; }

    #endregion

    #region --功率--

    /// <summary>
    /// 左腿平均功率 (瓦)
    /// </summary>
    public double? AvgLeftPowerWatts { get; set; }

    /// <summary>
    /// 右腿平均功率 (瓦)
    /// </summary>
    public double? AvgRightPowerWatts { get; set; }


    /// <summary>
    /// 左腿最大功率 (瓦)
    /// </summary>
    public double? MaxLeftPowerWatts { get; set; }

    /// <summary>
    /// 右腿最大功率 (瓦)
    /// </summary>
    public double? MaxRightPowerWatts { get; set; }


    /// <summary>
    /// 左腿功率平均占比 (%)  
    /// 示例：51.2 表示左腿占总输出功率的 51.2%
    /// </summary>
    public double? AvgLeftBalancePercent { get; set; }

    /// <summary>
    /// 右腿功率平均占比 (%)  
    /// 示例：48.8 表示右腿占总输出功率的 48.8%
    /// </summary>
    public double? AvgRightBalancePercent { get; set; }


    /// <summary>
    /// 总功率  (瓦)
    /// </summary>
    public double TotalPowerWatts { get; set; }

    /// <summary>
    /// 最大功率  (瓦)
    /// </summary>
    public double MaxPowerWatts { get; set; }

    /// <summary>
    /// 平均功率  (瓦)
    /// </summary>
    public double AvgPowerWatts { get; set; }

    /// <summary>
    /// 标准化功率 (瓦)
    /// </summary>
    public double NormalizedPowerWatts { get; set; }

    /// <summary>
    /// 变异系数（NP / Avg）
    /// </summary>
    public double VariabilityIndex { get; set; }

    /// <summary>
    /// 强度因子（NP / FTP）
    /// </summary>
    public double IntensityFactor { get; set; }

    /// <summary>
    /// 训练压力得分
    /// </summary>
    public double TrainingStressScore { get; set; }

    /// <summary>
    /// 功能阈值功率（通常为 1 小时 FTP）
    /// </summary>
    public double FunctionalThresholdPowerWatts { get; set; }

    #endregion
}