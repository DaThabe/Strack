namespace XingZhe.Model.User.Workout.Detail.Metrics;

/// <summary>
/// 持续时间
/// </summary>
public class DurationMetrics
{
    /// <summary>
    /// 总时间
    /// </summary>
    public TimeSpan? Total { get; set; }
    /// <summary>
    /// 下降时长
    /// </summary>
    public TimeSpan? Downslope { get; set; }
    /// <summary>
    /// 上升时长
    /// </summary>
    public TimeSpan? Upslope { get; set; }
    /// <summary>
    /// 平路时长
    /// </summary>
    public TimeSpan? Flat { get; set; }
}