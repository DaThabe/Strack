namespace Strack.External.Xingzhe.Model;

/// <summary>
/// 简略采样点
/// </summary>
public class SamplingSummary
{
    /// <summary>
    /// 时间
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// 纬度
    /// </summary>
    public double Latitude { get; set; }


    /// <summary>
    /// 转为字符串
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] 经度{Longitude}, 维度:{Latitude}";
    }
}
