using IGPSport.Service;
using Strack.Model.Entity.Activity;
using XingZhe.Service;

namespace Strack.Service.Import;

public static class ActivityImportServiceExtension
{
    /// <summary>
    /// 添加行者活动
    /// </summary>
    /// <param name="client"></param>
    /// <param name="workoutId"></param>
    /// <returns></returns>
    public static async Task<ActivityEntity> AddAsync(this IActivityImportService service, IXingZheClient client, long workoutId)
    {
        var detail = await client.GetWorkoutDetailAsync(workoutId);
        var records = await client.GetWorkoutRecordAsync(workoutId);

        return await service.AddAsync(detail, records);
    }

    /// <summary>
    /// 添加迹驰活动
    /// </summary>
    /// <param name="service"></param>
    /// <param name="client"></param>
    /// <param name="activityId"></param>
    /// <returns></returns>
    public static async Task<ActivityEntity> AddAsync(this IActivityImportService service, IIGPSportClient client, long activityId)
    {
        var detail = await client.GetActivityDetail(activityId);
        var fitFile = await client.GetActivityFitFileAsync(detail.FitUrl);

        return await service.AddAsync(detail, fitFile.Records);
    }
}
