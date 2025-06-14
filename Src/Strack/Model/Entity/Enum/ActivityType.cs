using XingZhe.Model.User.Workout;

namespace Strack.Model.Entity.Enum;


/// <summary>
/// 内部活动类型  (骑,跑,游,)
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
    public static string ToChineseName(this ActivityType type)
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
    public static ActivityType ToStrackActivityType(this IGPSport.Model.User.Activity.ActivityType type)
    {
        return type switch
        {
            IGPSport.Model.User.Activity.ActivityType.Ride => ActivityType.Ride,
            IGPSport.Model.User.Activity.ActivityType.Run => ActivityType.Run,
            _ => ActivityType.Other
        };
    }

    public static ActivityType ToStrackActivityType(this WorkoutType type)
    {
        return type switch
        {
            WorkoutType.Hike => ActivityType.Hike,
            WorkoutType.Run => ActivityType.Run,
            WorkoutType.Ride => ActivityType.Ride,
            WorkoutType.IndoorCycling => ActivityType.Ride,
            WorkoutType.VirtualRide => ActivityType.Ride,
            WorkoutType.Swim => ActivityType.Swim,
            WorkoutType.Ski => ActivityType.Ski,
            _ => ActivityType.Other
        };
    }
}