using Common.Extension;
using System.Xml.Linq;
using UnitsNet;

namespace Common.Model.File.Gpx;


/// <summary>
/// 轨迹点
/// </summary>
public class TrackPoint
{
    /// <summary>
    /// 时间
    /// </summary>
    public required DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    public required double Longitude { get; set; }

    /// <summary>
    /// 纬度
    /// </summary>
    public required double Latitude { get; set; }

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
    public Power? Power { get; set; }


    public override string ToString()
    {
        return this.ToStringBuilder()
            .AddParam(Timestamp, "时间")
            .AddParam(Longitude, "经度")
            .AddParam(Latitude, "维度")
            .AddParam(Altitude, "海拔")
            .AddParam(Speed, "速度")
            .AddParam(Distance, "距离")
            .AddParam(Cadence, "踏频")
            .AddParam(Heartrate, "心率")
            .AddParam(Temperature, "温度")
            .AddParam(Power, "功率")
            .ToString();
    }
}