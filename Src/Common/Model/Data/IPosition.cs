namespace Common.Model.Data;


/// <summary>
/// 位置 (经纬度)
/// </summary>
public interface IPosition
{
    /// <summary>
    /// 经度
    /// </summary>
    double Longitude { get; set; }

    /// <summary>
    /// 纬度
    /// </summary>
    double Latitude { get; set; }
}


public static class PositionExtension
{
    /// <summary>
    /// 转为坐标字符串 (如: "34.123456°N, 112.123456°E")
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static string ToCoordinateString(this IPosition position)
    {
        string lat = FormatCoordinate(position.Latitude, true);
        string lon = FormatCoordinate(position.Longitude, false);
        return $"{lat}, {lon}";
    }

    private static string FormatCoordinate(double degrees, bool isLatitude)
    {
        var direction = degrees switch
        {
            < 0 when isLatitude => "S",
            >= 0 when isLatitude => "N",
            < 0 when !isLatitude => "W",
            _ => "E"
        };

        return $"{Math.Abs(degrees):F6}°{direction}";
    }
}
