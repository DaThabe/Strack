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