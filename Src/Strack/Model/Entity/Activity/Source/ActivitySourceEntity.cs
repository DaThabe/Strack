using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Source;


/// <summary>
/// 活动来源
/// </summary>
[Table("ActivitySource")]
[Index(nameof(Type), nameof(ExternalId), IsUnique = true)]
public class ActivitySourceEntity : EntityBase
{
    /// <summary>
    /// 来源类型
    /// </summary>
    public required SourceType Type { get; set; }

    /// <summary>
    /// 外部活动Id
    /// </summary>
    public required string ExternalId { get; set; }


    /// <summary>
    /// 活动Id
    /// </summary>
    public required Guid ActivityId { get; set; }

    /// <summary>
    /// 活动
    /// </summary>
    [ForeignKey(nameof(ActivityId))]
    public ActivityEntity Activity { get; set; } = null!;
}
