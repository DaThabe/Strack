using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Record;

/// <summary>
/// 活动记录
/// </summary>
[Table("ActivityRecord")]
public class ActivityRecordEntity : EntityBase
{
    /// <summary>
    /// 时间（Unix秒）
    /// </summary>
    public long? UnixTimeSeconds { get; set; }

    /// <summary>
    /// 维度（度）
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// 经度（度）
    /// </summary>
    public double? Longitude { get; set; }


    /// <summary>
    /// 海拔高度（米）
    /// </summary>
    public double? AltitudeMeters { get; set; }

    /// <summary>
    /// 速度（米/秒）
    /// </summary>
    public double? SpeedBpm { get; set; }

    /// <summary>
    /// 距离（米）
    /// </summary>
    public double? DistanceMeters { get; set; }

    /// <summary>
    /// 温度（摄氏度）
    /// </summary>
    public double? TemperatureCelsius { get; set; }

    /// <summary>
    /// 心率（次/分钟）
    /// </summary>
    public int? HeartrateBpm { get; set; }

    /// <summary>
    /// 功率 (瓦)
    /// </summary>
    public int? PowerWatts { get; set; }



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