using Common.Extension;

namespace Common.Model.File.Gpx;


/// <summary>
/// 轨迹
/// </summary>
public class Track
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 段点
    /// </summary>
    public List<TrackPoint> Points { get; set; } = [];


    public override string ToString()
    {
        return this.ToStringBuilder()
            .AddParam(Name, "名称")
            .AddParam(Points.Count, "轨迹点数量")
            .ToString();
    }
}
