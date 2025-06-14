using System.ComponentModel.DataAnnotations.Schema;

namespace XingZhe.Model.User.Workout.Detail.Metrics;

/// <summary>
/// 坡度
/// </summary>
public class SlopeMetrics
{
    /// <summary>
    /// 平均坡度
    /// </summary>
    public double? Avg { get; set; }

    /// <summary>
    /// 最小坡度
    /// </summary>
    public double? Min { get; set; }

    /// <summary>
    /// 最大坡度
    /// </summary>
    public double? Max { get; set; }
}