using Strack.Model.Entity.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Source;


/// <summary>
/// 活动来源
/// </summary>
[Table("ActivitySource")]
public class ActivitySourceEntity : EntityBase
{
    /// <summary>
    /// 来源平台类型
    /// </summary>
    public required PlatformType Platform { get; set; }

    /// <summary>
    /// 外部活动 ID
    /// </summary>
    public required long ExternalId  { get; set; }

    /// <summary>
    /// 导入时间（Unix 秒）
    /// </summary>
    public long? ImportUnixTimeSeconds { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();


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
