using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail;

/// <summary>
/// 速度数据
/// </summary>
public class SpeedData
{
    /// <summary>
    /// 最大速度
    /// </summary>
    public Speed? Max { get; set; } = Speed.Zero;

    /// <summary>
    /// 平均速度
    /// </summary>
    public Speed? Avg { get; set; } = Speed.Zero;


    public override string ToString() => $"最大:{Max}, 平均{Avg}";
}
