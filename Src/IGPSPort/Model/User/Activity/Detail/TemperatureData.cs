using UnitsNet;

namespace IGPSport.Model.User.Activity.Detail;

public class TemperatureData
{
    /// <summary>
    /// 平均温度
    /// </summary>
    public Temperature? Avg { get; set; }

    /// <summary>
    /// 最高温度
    /// </summary>
    public Temperature? Max { get; set; }


    public override string ToString() => $"最高:{Max}, 平均{Avg}";
}