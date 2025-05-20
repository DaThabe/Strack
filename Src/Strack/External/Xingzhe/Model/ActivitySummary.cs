using UnitsNet;

namespace Strack.External.Xingzhe.Model;

/// <summary>
/// 单次训练简略信息
/// </summary>
public class ActivitySummary
{
    /// <summary>
    /// Id
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    public required DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public required XingzheActivityType Type { get; set; }



    /// <summary>
    /// 均速
    /// </summary>
    public Speed AvgSpeed { get; set; } = Speed.Zero;

    /// <summary>
    /// 持续时间
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// 距离
    /// </summary>
    public Length Distance { get; set; } = Length.Zero;


    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:MM}] 类型:{Type}, Id: {Id}, 标题:{Title}, 均速:{AvgSpeed}, 用时:{Duration}, 距离:{Distance}";
    }
}