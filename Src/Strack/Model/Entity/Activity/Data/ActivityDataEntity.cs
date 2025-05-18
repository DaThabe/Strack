using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Data;


/// <summary>
/// 活动数据
/// </summary>
[Table("ActivityData")]
public abstract class ActivityDataEntity : EntityBase
{
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
