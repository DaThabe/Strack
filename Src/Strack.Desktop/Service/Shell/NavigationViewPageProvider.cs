using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace Strack.Desktop.Service.Shell;

internal class NavigationViewPageProvider(IServiceProvider services) : INavigationViewPageProvider
{
    public object? GetPage(Type pageType)
    {
        try
        {
            return services.GetRequiredService(pageType);
        }
        catch (Exception ex)
        {
            return ExceptionContent(ex);
        }
    }


    private static ScrollViewer ExceptionContent(Exception exception)
    {
        return new ScrollViewer()
        {
            HorizontalAlignment= System.Windows.HorizontalAlignment.Center,
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = new InfoBar()
            {
                IsClosable = false,
                IsOpen = true,
                Severity = InfoBarSeverity.Error,
                Message = exception.ToString(),
                Title = "页面创建失败"
            }
        };
    }
}