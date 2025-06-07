using UnitsNet;

namespace Common.Model.File.Fit;


/// <summary>
/// 采样点
/// </summary>
public class Record
{
    /// <summary>
    /// 时间
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// 纬度
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// 海拔高度
    /// </summary>
    public Length? Altitude { get; set; }


    /// <summary>
    /// 速度
    /// </summary>
    public Speed? Speed { get; set; }

    /// <summary>
    /// 距离
    /// </summary>
    public Length? Distance { get; set; }

    /// <summary>
    /// 踏频
    /// </summary>
    public Frequency? Cadence { get; set; }

    /// <summary>
    /// 心率
    /// </summary>
    public Frequency? Heartrate { get; set; }

    /// <summary>
    /// 温度
    /// </summary>
    public Temperature? Temperature { get; set; }

    /// <summary>
    /// 功率
    /// </summary>
    public Power ? Power { get; set; }
}
