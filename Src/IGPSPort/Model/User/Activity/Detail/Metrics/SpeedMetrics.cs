using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail.Metrics;

/// <summary>
/// 速度数据
/// </summary>
public class SpeedMetrics
{
    /// <summary>
    /// 最大速度
    /// </summary>
    public Speed? Max { get; set; } = Speed.Zero;

    /// <summary>
    /// 平均速度
    /// </summary>
    public Speed? Avg { get; set; } = Speed.Zero;


    /// <summary>
    /// 最快上升速度
    /// </summary>
    public Speed? MaxAscent { get; set; }
    /// <summary>
    /// 平均上升速度
    /// </summary>
    public Speed? AvgAscent { get; set; }


    /// <summary>
    /// 最快下降速度
    /// </summary>
    public Speed? MaxDescent { get; set; }
    /// <summary>
    /// 平均下降速度
    /// </summary>
    public Speed? AvgDescent { get; set; }



    public override string ToString() => $"最大:{Max}, 平均{Avg}";
}