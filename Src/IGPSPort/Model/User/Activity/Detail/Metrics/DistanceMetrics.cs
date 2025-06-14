using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail.Metrics;

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
    /// 上坡距离
    /// </summary>
    public Length? Upslope { get; set; }
    /// <summary>
    /// 下坡距离
    /// </summary>
    public Length? Downslope { get; set; }
}