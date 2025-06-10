using Strack.Model.Entity.Activity.Data;
using Strack.Model.Entity.Record;
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
    public string? Title { get; set; }

    /// <summary>
    /// 运动类型
    /// </summary>
    public ActivityType Sport { get; set; } = ActivityType.Other;

    /// <summary>
    /// 来源
    /// </summary>
    public ActivitySourceType Source { get; set; } = ActivitySourceType.None;


    /// <summary>
    /// 开始时间 (Unix秒)
    /// </summary>
    public long? BeginUnixTimeSeconds { get; set; }

    /// <summary>
    /// 结束时间 (Unix秒)
    /// </summary>
    public long? FinishUnixTimeSeconds { get; set; }


    /// <summary>
    /// 总时长 (秒)
    /// </summary>
    public double? DurationSeconds { get; set; }

    /// <summary>
    /// 总距离 (米)
    /// </summary>
    public double? TotalDistanceMeters { get; set; }


    /// <summary>
    /// 消耗热量 (千卡)
    /// </summary>
    public double? CaloriesKilocalories { get; set; }


    #endregion

    #region --扩展数据--

    /// <summary>
    /// 踏频
    /// </summary>
    public CadenceDataEntity? Cadence { get; set; }

    /// <summary>
    /// 坡度数据
    /// </summary>
    public ElevationDataEntity? Elevation { get; set; }

    /// <summary>
    /// 心率数据
    /// </summary>
    public HeartrateDataEntity? Heartrate { get; set; }

    /// <summary>
    /// 功率
    /// </summary>
    public PowerDataEntity? Power { get; set; }

    /// <summary>
    /// 速度数据
    /// </summary>
    public SpeedDataEntity? Speed { get; set; }

    /// <summary>
    /// 温度数据
    /// </summary>
    public TemperatureDataEntity? Temperature { get; set; }

    /// <summary>
    /// 时间数据
    /// </summary>
    public TimeDataEntity? Time { get; set; }

    #endregion


    /// <summary>
    /// 包含的所有记录点
    /// </summary>
    public ICollection<RecordEntity> Records { get; set; } = new HashSet<RecordEntity>();
}