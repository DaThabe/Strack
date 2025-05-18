using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Sampling.Data;


/// <summary>
/// 骑行采样点
/// </summary>
[Table("SamplingCyclingData")]
public partial class CyclingSamplingDataEntity : SamplingDataEntity
{
    #region --踏频--

    /// <summary>
    /// 踏频（次/分钟）
    /// </summary>
    public double? CadenCyclesPerMinute { get; set; }

    #endregion

    #region --功率--

    /// <summary>
    /// 实时功率（瓦）
    /// </summary>
    public double? PowerWatts { get; set; }

    /// <summary>
    /// 实时踏频（转/分）
    /// </summary>
    public double? CadenceRpm { get; set; }

    /// <summary>
    /// 左腿功率百分比（%）
    /// </summary>
    public double? LeftPowerPercent { get; set; }

    /// <summary>
    /// 右腿功率百分比（%）
    /// </summary>
    public double? RightPowerPercent { get; set; }

    /// <summary>
    /// 扭矩效率（%）
    /// </summary>
    public double? TorqueEfficiencyPercent { get; set; }

    /// <summary>
    /// 踏频平滑度（%）
    /// </summary>
    public double? PedalSmoothnessPercent { get; set; }

    /// <summary>
    /// 左腿扭矩（牛·米）
    /// </summary>
    public double? LeftTorqueNm { get; set; }

    /// <summary>
    /// 右腿扭矩（牛·米）
    /// </summary>
    public double? RightTorqueNm { get; set; }

    #endregion
}
