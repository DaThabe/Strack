using IGPSport.Model.User.Activity.Summary;
using Strack.Model.Entity.Enum;
using XingZhe.Model.User.Workout.Summary;

namespace Strack.Service.Sync.Activity;

/// <summary>
/// 活动简要
/// </summary>
public interface IActivitySummary
{
    /// <summary>
    /// 平台
    /// </summary>
    PlatformType Platform { get; }

    /// <summary>
    /// 活动Id
    /// </summary>
    long Id { get; }
}


/// <summary>
/// 行者活动简要
/// </summary>
public class XingZheActivitySummary(WorkoutSummary workout) : IActivitySummary
{
    public PlatformType Platform => PlatformType.XingZhe;

    public long Id => workout.Id;
}

/// <summary>
/// 迹驰活动简要
/// </summary>
public class IGPSportActivitySummary(ActivitySummary activity) : IActivitySummary
{
    public PlatformType Platform => PlatformType.IGPSport;

    public long Id => activity.Id;
}