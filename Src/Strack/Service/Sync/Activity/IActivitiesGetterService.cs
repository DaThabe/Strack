using IGPSport.Service;
using XingZhe.Service;

namespace Strack.Service.Sync.Activity;

/// <summary>
/// 活动
/// </summary>
public interface IActivitiesGetterService
{
    /// <summary>
    /// 获取活动列表
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<IActivitySummary> GetActivitiesAsync();
}

public class XingZheActivitiesGetterService(IXingZheClient client) : IActivitiesGetterService
{
    public IAsyncEnumerable<IActivitySummary> GetActivitiesAsync()
    {
        return client.GetWorkoutSummaryAsync().Select(x => new XingZheActivitySummary(x));
    }
}

public class IGPSportActivitiesGetterService(IIGPSportClient client) : IActivitiesGetterService
{
    public IAsyncEnumerable<IActivitySummary> GetActivitiesAsync()
    {
        return client.GetActivitySummaryAsync().Select(x => new IGPSportActivitySummary(x));
    }
}