using Microsoft.Extensions.Logging;
using Wpf.Ui.Abstractions;

namespace Strack.Desktop.Service.Navigation;


public class NavigationViewPageProvider(
    IServiceProvider services,
    ILogger<NavigationViewPageProvider> logger
    ) : INavigationViewPageProvider
{
    public object? GetPage(Type pageType)
    {
        try
        {
            return services.GetService(pageType);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "页面视图获取失败,类型:{type}", pageType);
            return null;
        }
    }
}
