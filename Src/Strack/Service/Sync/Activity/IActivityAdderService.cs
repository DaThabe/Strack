using IGPSport.Service;
using Strack.Service.Import;
using XingZhe.Service;

namespace Strack.Service.Sync.Activity;


/// <summary>
/// 活动添加业务
/// </summary>
public interface IActivityAdderService
{
    /// <summary>
    /// 添加
    /// </summary>
    Task AddAsync(IActivitySummary activity);
}

/// <summary>
/// 行者活动添加器
/// </summary>
/// <param name="activityImportService"></param>
/// <param name="client"></param>
public class XingZheActivityAdderService(
    IActivityImportService activityImportService,
    IXingZheClient client) : IActivityAdderService
{
    public async Task AddAsync(IActivitySummary activity)
    {
        await activityImportService.AddAsync(client, activity.Id);
    }
}

/// <summary>
/// 迹驰活动添加器
/// </summary>
/// <param name="activityImportService"></param>
/// <param name="client"></param>
public class IGPSportActivityAdderService(
    IActivityImportService activityImportService,
    IIGPSportClient client) : IActivityAdderService
{
    public async Task AddAsync(IActivitySummary activity)
    {
        await activityImportService.AddAsync(client, activity.Id);
    }
}