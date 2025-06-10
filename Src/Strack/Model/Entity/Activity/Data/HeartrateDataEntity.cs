using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Data;

/// <summary>
/// 心率数据
/// </summary>
[Table("ActivityHeartrate")]
public class HeartrateDataEntity : EntityBase
{
    /// <summary>
    /// 平均心率 (次/分)
    /// </summary>
    public double? AvgBpm { get; set; }

    /// <summary>
    /// 最低心率 (次/分)
    /// </summary>
    public double? MinBpm { get; set; }

    /// <summary>
    /// 最高心率 (次/分)
    /// </summary>
    public double? MaxBpm { get; set; }


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
