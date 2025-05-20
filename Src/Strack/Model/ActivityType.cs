namespace Strack.Model;


/// <summary>
/// 训练类型
/// </summary>
public enum ActivityType
{
    /// <summary>
    /// 其他
    /// </summary>
    Other,

    /// <summary>
    /// 散步
    /// </summary>
    Walk = 101,

    /// <summary>
    /// 徒步
    /// </summary>
    Hike = 102,

    /// <summary>
    /// 跑步
    /// </summary>
    Run = 103,

    /// <summary>
    /// 越野跑
    /// </summary>
    TrailRun = 104,

    /// <summary>
    /// 骑行
    /// </summary>
    Ride = 201,

    /// <summary>
    /// 游泳
    /// </summary>
    Swim = 301,

    /// <summary>
    /// 滑雪
    /// </summary>
    Ski = 401,
}

public static class ActivityTypeExtension
{
    public static string ToName(this ActivityType type)
    {
        return type switch
        {
            ActivityType.Walk => "散步",
            ActivityType.Hike => "徒步",
            ActivityType.Run => "跑步",
            ActivityType.TrailRun => "越野跑",
            ActivityType.Ride => "骑行",
            ActivityType.Swim => "游泳",
            ActivityType.Ski => "滑雪",
            _ => "其他"
        };
    }
}