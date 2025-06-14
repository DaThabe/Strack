using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Extension;

/// <summary>
/// 踏频数据
/// </summary>
[Table("ActivityCadence")]
public class ActivityCadenceEntity : EntityBase
{
    /// <summary>
    /// 最高踏频 (圈/分)
    /// </summary>
    public double? MaxCpm { get; set; }

    /// <summary>
    /// 平均踏频 (圈/分)
    /// </summary>
    public double? AvgCpm { get; set; }




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