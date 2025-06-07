using Common.Extension;

namespace Common.Model.File.Gpx;


/// <summary>
/// 元数据
/// </summary>
public class Metadata
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 作者名称
    /// </summary>
    public string? AuthorName { get; set; } = "Strack";

    /// <summary>
    /// 作者链接
    /// </summary>
    public string? AuthorLink { get; set; } = "https://github.com/DaThabe/Strack";

    /// <summary>
    /// 关键字
    /// </summary>
    public string[]? Keywords { get; set; } = ["Strack"];

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }


    public override string ToString()
    {
        return this.ToStringBuilder()
            .AddParam(Timestamp, "时间")
            .AddParam(Name, "名称")
            .AddParam(Keywords, "关键字")
            .ToString();
    }
}
