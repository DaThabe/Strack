using Common.Extension;

namespace Common.Model.File.Gpx;


/// <summary>
/// Gpx 文件模型 (GPS Exchange Format)
/// </summary>
public class GpxFile
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public FileInfo FileInfo { get; set; } = new();

    /// <summary>
    /// 元数据
    /// </summary>
    public Metadata Metadata { get; set; } = new();

    /// <summary>
    /// 轨迹点
    /// </summary>
    public List<Track> Tracks { get; set; } = [];


    public override string ToString()
    {
        return this.ToStringBuilder()
            .AddParam(FileInfo, "文件信息")
            .AddParam(Metadata, "元数据")
            .AddParam(Tracks.Count, "轨迹点数量")
            .ToString();

    }
}