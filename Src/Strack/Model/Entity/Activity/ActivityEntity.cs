using Strack.Model.Entity.Activity.Metrics;
using Strack.Model.Entity.Activity.Record;
using Strack.Model.Entity.Activity.Source;
using Strack.Model.Entity.Enum;
using Strack.Model.Entity.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity;


/// <summary>
/// 活动
/// </summary>
[Table("Activity")]
public class ActivityEntity : EntityBase
{
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
    /// 消耗热量 (千卡)
    /// </summary>
    public double? CaloriesKilocalories { get; set; }

    /// <summary>
    /// 统计信息
    /// </summary>
    public ActivityMetrics Metrics { get; set; } = new();



    /// <summary>
    /// 用户Id
    /// </summary>
    public required Guid UserId { get; set; }
    /// <summary>
    /// 用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; } = null!;



    /// <summary>
    /// 来源
    /// </summary>
    public ActivitySourceEntity Source { get; set; } = null!;

    /// <summary>
    /// 包含的所有记录点
    /// </summary>
    public ICollection<ActivityRecordEntity> Records { get; set; } = new HashSet<ActivityRecordEntity>();
}