using UnitsNet;

namespace XingZhe.Model.User.Workout.Detail.Metrics;

/// <summary>
/// 速度数据
/// </summary>
public class SpeedMetrics
{
    /// <summary>
    /// 最大速度
    /// </summary>
    public Speed? Max { get; set; }

    /// <summary>
    /// 平均速度
    /// </summary>
    public Speed? Avg { get; set; }

    /// <summary>
    /// 上升速度
    /// </summary>
    public Speed? Ascent { get; set; }


    public override string ToString() => $"最大:{Max}, 平均{Avg}";
}