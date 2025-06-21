using Common.Infrastructure;
using UnitsNet;

namespace IGPSport.Model.User.Activity.Summary;

/// <summary>
/// 活动列表元素
/// </summary>
public class ActivitySummary : IIdentifier<long>
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Fit 文件网址
    /// </summary>
    public string FitFileUrl { get; set; } = string.Empty;

    /// <summary>
    /// 时间
    /// </summary>
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.MinValue;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 类型
    /// </summary>
    public ActivityType Type { get; set; } = ActivityType.All;


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
        return $"[{StartTime:yyyy-MM-dd HH:MM}] 类型:{Type}, Id: {Id}, 标题:{Title}, 均速:{AvgSpeed}, 用时:{Duration}, 距离:{Distance}";
    }
}