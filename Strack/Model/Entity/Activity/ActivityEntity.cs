using Strack.Model.Entity.Sampling;
using Strack.Model.Entity.Source;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity;


/// <summary>
/// 活动
/// </summary>
[Table("Activity")]
public class ActivityEntity : EntityBase
{
    #region --基础--

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 运动类型
    /// </summary>
    public required ActivityType Sport { get; set; }

    /// <summary>
    /// 消耗热量 (千卡)
    /// </summary>
    public double CaloriesKilocalories { get; set; }

    #endregion

    #region --时间--

    /// <summary>
    /// 开始时间 (UTC)
    /// </summary>
    public DateTime BeginTimeUtc { get; set; }

    /// <summary>
    /// 结束时间 (UTC)
    /// </summary>
    public DateTime FinishTimeUtc { get; set; }

    /// <summary>
    /// 总时间
    /// </summary>
    public TimeSpan TotalTime { get; set; }

    /// <summary>
    /// 移动时间
    /// </summary>
    public TimeSpan MovingTime { get; set; }

    /// <summary>
    /// 暂停时间
    /// </summary>
    public TimeSpan PauseTime { get; set; }


    #endregion

    #region --温度--

    /// <summary>
    /// 最低温度
    /// </summary>
    public double MinTemperatureCelsius { get; set; }

    /// <summary>
    /// 最高温度
    /// </summary>
    public double MaxTemperatureCelsius { get; set; }

    /// <summary>
    /// 平均温度
    /// </summary>
    public double AvgTemperatureCelsius { get; set; }

    #endregion

    #region --爬升--

    /// <summary>
    /// 平均海拔 (米)
    /// </summary>
    public double AvgAltitudeMeters { get; set; }

    /// <summary>
    /// 最低海拔 (米)
    /// </summary>
    public double MinAltitudeMeters { get; set; }

    /// <summary>
    /// 最高海拔 (米)
    /// </summary>
    public double MaxAltitudeMeters { get; set; }

    /// <summary>
    /// 总上升高度 (米)
    /// </summary>
    public double TotalAscentMeters { get; set; }

    /// <summary>
    /// 总下降高度 (米)
    /// </summary>
    public double TotalDescentMeters { get; set; }

    #endregion

    #region --移动--

    /// <summary>
    /// 总距离 (米)
    /// </summary>
    public double TotalDistanceMeters { get; set; }

    /// <summary>
    /// 平均速度 (千米/时)
    /// </summary>
    public double AvgSpeedKilometersPerHour { get; set; }

    /// <summary>
    /// 最大速度 (千米/时)
    /// </summary>
    public double MaxSpeedKilometersPerHour { get; set; }

    #endregion

    #region --心率--

    /// <summary>
    /// 平均心率 (次/分)
    /// </summary>
    public double AvgHeartrateBeatsPerMinute { get; set; }

    /// <summary>
    /// 最大心率 (次/分)
    /// </summary>
    public double MaxBeatsPerMinute { get; set; }

    #endregion

    /// <summary>
    /// 来源
    /// </summary>
    public required SourceEntity Source { get; set; } 

    /// <summary>
    /// 采样点
    /// </summary>
    public ICollection<SamplingEntity> Samplings { get; set; } = new HashSet<SamplingEntity>();
}