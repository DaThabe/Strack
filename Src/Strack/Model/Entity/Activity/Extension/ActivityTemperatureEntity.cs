using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity.Activity.Extension;

/// <summary>
/// 温度数据
/// </summary>
[Table("ActivityTemperature")]
public class ActivityTemperatureEntity : EntityBase
{
    /// <summary>
    /// 平均温度 (摄氏度)
    /// </summary>
    public double? AvgCelsius { get; set; }

    /// <summary>
    /// 最低温度  (摄氏度)
    /// </summary>
    public double? MinCelsius { get; set; }

    /// <summary>
    /// 最高温度  (摄氏度)
    /// </summary>
    public double? MaxCelsius { get; set; }


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
