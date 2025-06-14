using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Extension;

/// <summary>
/// 时间数据
/// </summary>
[Table("ActivityTime")]
public class ActivityTimeEntity : EntityBase
{
    /// <summary>
    /// 移动时间 (秒)
    /// </summary>
    public double? MovingDurationSeconds { get; set; }

    /// <summary>
    /// 暂停时间 (秒)
    /// </summary>
    public double? PauseDurationSeconds { get; set; }



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
