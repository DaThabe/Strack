using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Data;

/// <summary>
/// 速度数据
/// </summary>
[Table("ActivitySpeed")]
public class SpeedDataEntity : EntityBase
{
    /// <summary>
    /// 平均速度 (千米/时)
    /// </summary>
    public double? AvgKph { get; set; }

    /// <summary>
    /// 最大速度 (千米/时)
    /// </summary>
    public double? MaxKph { get; set; }



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
