using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail.Metrics;

/// <summary>
/// 海拔
/// </summary>
public class AltitudeMetrics
{
    /// <summary>
    /// 最低海拔
    /// </summary>
    public Length? Min { get; set; }

    /// <summary>
    /// 最高海拔
    /// </summary>
    public Length? Max { get; set; }

    /// <summary>
    /// 平均海拔
    /// </summary>
    public Length? Avg { get; set; }
}
