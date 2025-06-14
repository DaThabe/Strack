using UnitsNet;

namespace XingZhe.Model.User.Workout.Detail.Metrics;


/// <summary>
/// 海拔数据
/// </summary>
public class AltitudeMetrics
{
    /// <summary>
    /// 平均海拔
    /// </summary>
    public Length? Avg { get; set; }

    /// <summary>
    /// 最高海拔
    /// </summary>
    public Length? Max { get; set; }
}
