using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail.Metrics;


/// <summary>
/// 高程
/// </summary>
public class ElevationMetrics
{
    /// <summary>
    /// 总升高度
    /// </summary>
    public Length? AscentHeight { get; set; }
    /// <summary>
    /// 总降高度
    /// </summary>
    public Length? DescentHeight { get; set; }
}