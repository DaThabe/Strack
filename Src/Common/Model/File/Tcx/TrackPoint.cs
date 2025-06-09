namespace Common.Model.File.Tcx;

/// <summary>
/// 轨迹点
/// </summary>
public class TrackPoint
{
    /// <summary>
    /// 时间
    /// </summary>
    public DateTimeOffset Time { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public required Position Position { get; set; }

    /// <summary>
    /// 海拔 (米)
    /// </summary>
    public double AltitudeMeters { get; set; }

    /// <summary>
    /// 距离 (米)
    /// </summary>
    public double DistanceMeters { get; set; }
}

public class Position
{
    /// <summary>
    /// 经度 (度)
    /// </summary>
    public double LongitudeDegrees { get; set; }

    /// <summary>
    /// 纬度 (度)
    /// </summary>
    public double LatitudeDegrees { get; set; }
}
