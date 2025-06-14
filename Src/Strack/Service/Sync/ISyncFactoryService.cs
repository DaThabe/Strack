using IGPSport.Service;
using Strack.Model.Entity.Enum;
using Strack.Service.Import;
using Strack.Service.Sync.Activity;
using XingZhe.Service;

namespace Strack.Service.Sync;

public interface ISyncFactoryService
{
    /// <summary>
    /// 创建行者同步器
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    ISyncService CreateXingZheSyncService(string sessionId);

    /// <summary>
    /// 创建迹驰同步器
    /// </summary>
    /// <param name="authToken"></param>
    /// <returns></returns>
    ISyncService CreateIGPSportSyncService(string authToken);
}


public class SyncFactoryService(
    IServiceProvider services,
    IActivityImportService activityImportService,
    IXingZheClientProvider xingZheClientProvider,
    IIGPSportClientProvider iGPSportClientProvider
    ) : ISyncFactoryService
{
    public ISyncService CreateIGPSportSyncService(string authToken)
    {
        var client = iGPSportClientProvider.GetOrCreateFromAuthToken(authToken);

        return new SyncService(
            services,
            PlatformType.IGPSport,
            new IGPSportActivitiesGetterService(client),
            new IGPSportActivityAdderService(activityImportService, client));
    }

    public ISyncService CreateXingZheSyncService(string sessionId)
    {
        var client = xingZheClientProvider.GetOrCreateFromSessionId(sessionId);

        return new SyncService(
            services,
            PlatformType.XingZhe,
            new XingZheActivitiesGetterService(client),
            new XingZheActivityAdderService(activityImportService, client));
    }
}