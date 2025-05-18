using UnitsNet;

namespace Strack.External.Xingzhe.Model;

/// <summary>
/// 具体采样点
/// </summary>
public class SamplingDetail : SamplingSummary
{
    /// <summary>
    /// 海拔高度
    /// </summary>
    public Length Altitude { get; set; } = Length.Zero;

    /// <summary>
    /// 速度
    /// </summary>
    public Speed Speed { get; set; } = Speed.Zero;

    /// <summary>
    /// 距离
    /// </summary>
    public Length Distance { get; set; } = Length.Zero;

    /// <summary>
    /// 踏频
    /// </summary>
    public Frequency Cadence { get; set; } = Frequency.Zero;

    /// <summary>
    /// 心率
    /// </summary>
    public Frequency Heartrate { get; set; } = Frequency.Zero;

    /// <summary>
    /// 温度
    /// </summary>
    public Temperature Temperature { get; set; } = Temperature.Zero;

    /// <summary>
    /// 功率
    /// </summary>
    public Power Power { get; init; } = Power.Zero;



    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] 经度{Longitude}, 维度:{Latitude}, 海拔:{Altitude}, 速度:{Speed}, 距离:{Distance}, 踏频:{Cadence}, 心率:{Heartrate}, 温度:{Temperature}, 功率:{Power}";
    }
}
