using Common.Extension;
namespace Common.Model.File.Gpx;


/// <summary>
/// 文件信息
/// </summary>
public class FileInfo
{
    /// <summary>
    /// 版本
    /// </summary>
    public string? Version { get; set; } = "1.1";

    /// <summary>
    /// 创建程序
    /// </summary>
    public string? Creator { get; set; } = "Strack.Sdk.Gpx";


    public override string ToString()
    {
        return this.ToStringBuilder()
            .AddParam(Version, "版本")
            .AddParam(Creator, "创建程序")
            .ToString();
    }
}
