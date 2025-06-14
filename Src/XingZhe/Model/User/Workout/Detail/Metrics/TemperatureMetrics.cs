using UnitsNet;

namespace XingZhe.Model.User.Workout.Detail.Metrics;

/// <summary>
/// 温度数据
/// </summary>
public class TemperatureMetrics
{
    /// <summary>
    /// 平均温度
    /// </summary>
    public Temperature? Avg { get; set; }

    /// <summary>
    /// 最低温度
    /// </summary>
    public Temperature? Min { get; set; }

    /// <summary>
    /// 最高温度
    /// </summary>
    public Temperature? Max { get; set; }


    public override string ToString() => $"最低:{Min}, 最高:{Max}, 平均{Avg}";
}