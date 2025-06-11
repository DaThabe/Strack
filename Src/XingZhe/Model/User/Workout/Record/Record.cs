using Common.Model.Data;
using Common.Model.File.Gpx;
using UnitsNet;

namespace XingZhe.Model.User.Workout.Record;

/// <summary>
/// 记录 (时间,经纬度,海拔,速度,距离,踏频,心率,温度,功率)
/// </summary>
public class Record : IGeoPosition
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

    /// <summary>
    /// 左平衡
    /// </summary>
    public Ratio? LeftBalance { get; set; }

    /// <summary>
    /// 右平衡
    /// </summary>
    public Ratio? RightBalance { get; set; }


    public override string ToString() => $"{Timestamp:yyyy-MM-dd HH:mm:ss}-{this.ToCoordinateString()}";
}

public static class Fuck
{
    /// <summary>
    /// 将采样点附加到 Gpx 轨迹
    /// </summary>
    /// <param name="xingZhePoints"></param>
    /// <param name="gpxPoints"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void AttachToTrackPoint(this IList<Record> xingZhePoints, IList<Common.Model.File.Gpx.TrackPoint> gpxPoints)
    {
        Dictionary<DateTimeOffset, Record> xingZheDict = [];
        foreach (var i in xingZhePoints) xingZheDict[i.Timestamp] = i;

        foreach (var i in gpxPoints)
        {
            if (i.Time is null) continue;
            if (!xingZheDict.TryGetValue(i.Time.Value, out var xingZhe)) continue;


            i.Altitude = xingZhe.Altitude;
            i.Extension = new TrackPointExtension
            {
                Cadence = xingZhe.Cadence,
                Speed = xingZhe.Speed,
                Distance = xingZhe.Distance,
                Heartrate = xingZhe.Heartrate,
                Temperature = xingZhe.Temperature,
                Power = xingZhe.Power,
            };
        }
    }
}