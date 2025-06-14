using UnitsNet;

namespace XingZhe.Model.User.Workout.Detail.Metrics;

/// <summary>
/// 距离
/// </summary>
public class DistanceMetrics
{
    /// <summary>
    /// 总距离
    /// </summary>
    public Length? Total { get; set; }
    /// <summary>
    /// 下坡距离
    /// </summary>
    public Length? Downslope { get; set; }
    /// <summary>
    /// 上坡距离
    /// </summary>
    public Length? Upslope { get; set; }
    /// <summary>
    /// 平路距离
    /// </summary>
    public Length? Flat { get; set; }
}