using Common.Model.File.Gpx;
using UnitsNet;

namespace XingZhe.Model.Workout;

/// <summary>
/// 训练采样点明细 (时间,海拔,速度,距离,踏频,心率,温度,功率)
/// </summary>
public class Record
{
    /// <summary>
    /// 时间
    /// </summary>
    public required DateTimeOffset Timestamps { get; set; }

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
        return $"海拔:{Altitude}, 速度:{Speed}, 距离:{Distance}, 踏频:{Cadence}, 心率:{Heartrate}, 温度:{Temperature}, 功率:{Power}";
    }
}

public static class Fuck
{
    /// <summary>
    /// 将采样点附加到 Gpx 轨迹
    /// </summary>
    /// <param name="records"></param>
    /// <param name="points"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void AttachToTrackPoint(this IList<Record> records, IList<TrackPoint> points)
    {
        Dictionary<DateTimeOffset, Record> recordDict = [];
        foreach (var i in records) recordDict[i.Timestamps] = i;

        foreach (var i in points)
        {
            if (!recordDict.TryGetValue(i.Timestamp, out var record))
            {
                //TODO: 缺少数据
                continue;
            }

            i.Cadence = record.Cadence;
            i.Altitude = record.Altitude;
            i.Speed = record.Speed;
            i.Distance = record.Distance;
            i.Heartrate = record.Heartrate;
            i.Temperature = record.Temperature;
            i.Power = record.Power;
        }
    }
}