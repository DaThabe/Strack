using UnitsNet;

namespace XingZhe.Model.User.Workout.Detail.Metrics;

public class UserMetrics
{
    /// <summary>
    /// Id
    /// </summary>
    public required long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 头像网址
    /// </summary>
    public required string? AvatarUrl { get; set; }


    /// <summary>
    /// 体重
    /// </summary>
    public Mass? Weight { get; set; }

    /// <summary>
    /// 阈值功率
    /// </summary>
    public Power? Ftp { get; set; }

    /// <summary>
    /// 阈值心率
    /// </summary>
    public Frequency? LtHr { get; set; }

    /// <summary>
    /// 最大心率
    /// </summary>
    public Frequency? MaxHr { get; set; }
}
