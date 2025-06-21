using Microsoft.EntityFrameworkCore;
using Strack.Data.Queryable;
using Strack.Model.Entity.Activity.Record;

namespace Strack.Data.Extension;

public static class ActivityRecordExtension
{
    /// <summary>
    /// 查询某个活动的记录点
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="activityEntityId"></param>
    /// <param name="interval"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static Task<List<ActivityRecordEntity>> FindActivityRecordsAsync(this StrackDbContext dbContext, Guid activityEntityId, EntityQueryableOptionHandler<ActivityRecordEntity>? option= null)
    {
        return dbContext.ActivityRecords
            .Where(x => x.ActivityId == activityEntityId)
            .Option(option)
            .OrderBy(x => x.UnixTimeSeconds)
            .ToListAsync();
    }

    public static async Task<List<(double Lon, double Lat)>> FindActivityRecordPotisionAsync(this StrackDbContext dbContext, Guid activityEntityId, EntityQueryableOptionHandler<ActivityRecordEntity>? option = null)
    {
        var points = await dbContext.ActivityRecords
                .AsNoTracking()
                .Where(x => x.ActivityId == activityEntityId && x.Latitude != null && x.Longitude != null)
                .OrderBy(x => x.UnixTimeSeconds)
                .Option(option)
                .ToListAsync();

        return [.. points.Select(x => (x.Longitude!.Value, x.Latitude!.Value))];
    }
}
