using Common.Infrastructure;
using UnitsNet;

namespace XingZhe.Model.User.Workout.Summary;

/// <summary>
/// 训练摘要
/// </summary>
public class WorkoutSummary : IIdentifier<long>
{
    /// <summary>
    /// Id
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public required DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public required WorkoutType Type { get; set; }


    /// <summary>
    /// 缩略图网址
    /// </summary>
    public required string ThumbnailUrl { get; set; }


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


    public override string ToString() => $"[{StartTime:yyyy-MM-dd HH:MM}]-{Title}";
}