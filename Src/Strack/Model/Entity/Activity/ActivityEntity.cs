using Strack.Model.Entity.Activity.Extension;
using Strack.Model.Entity.Activity.Record;
using Strack.Model.Entity.Activity.Source;
using Strack.Model.Entity.User;
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
    public ActivityCadenceEntity? Cadence { get; set; }

    /// <summary>
    /// 坡度数据
    /// </summary>
    public ActivityElevationEntity? Elevation { get; set; }

    /// <summary>
    /// 心率数据
    /// </summary>
    public ActivityHeartrateEntity? Heartrate { get; set; }

    /// <summary>
    /// 功率
    /// </summary>
    public ActivityPowerEntity? Power { get; set; }

    /// <summary>
    /// 速度数据
    /// </summary>
    public ActivitySpeedEntity? Speed { get; set; }

    /// <summary>
    /// 温度数据
    /// </summary>
    public ActivityTemperatureEntity? Temperature { get; set; }

    /// <summary>
    /// 时间数据
    /// </summary>
    public ActivityTimeEntity? Time { get; set; }


    /// <summary>
    /// 来源
    /// </summary>
    public ActivitySourceEntity? Source { get; set; }

    /// <summary>
    /// 所属用户
    /// </summary>
    public UserEntity? User { get; set; }

    #endregion


    /// <summary>
    /// 包含的所有记录点
    /// </summary>
    public ICollection<ActivityRecordEntity> Records { get; set; } = new HashSet<ActivityRecordEntity>();
}