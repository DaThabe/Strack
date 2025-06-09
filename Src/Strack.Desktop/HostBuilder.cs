using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.Desktop.Service;
using Strack.Desktop.Service.Navigation;
using Strack.Desktop.UI.Shell.Main;
using Strack.Desktop.UI.View;
using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.UI.View.Setting;
using Strack.Desktop.UI.View.Track;
using Strack.Desktop.ViewModel.Shell;
using Strack.Desktop.ViewModel.View;
using Strack.Desktop.ViewModel.View.Dashboard;
using Strack.Desktop.ViewModel.View.Dashboard.Activity;
using Strack.Desktop.ViewModel.View.Setting;
using Wpf.Ui;
using Wpf.Ui.Abstractions;

namespace Strack.Desktop;

public static class HostBuilder
{
    public static IHostBuilder UseStrackDesktop(this IHostBuilder builder, App app)
    {
        builder
            .UseStrack()
            .ConfigureServices((context, services) =>
            {
                //Hosted
                services.AddHostedService<HostedService>();

                //导航
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<INavigationViewPageProvider, NavigationViewPageProvider>();
                //提示
                services.AddSingleton<ISnackbarService, SnackbarService>();
                //弹窗
                services.AddSingleton<IContentDialogService, ContentDialogService>();
                //主题
                services.AddSingleton<IThemeService, ThemeService>();


                //程序和窗口
                services.AddSingleton(app);
                services.AddSingleton<MainShell>();
                services.AddTransient<MainShellViewModel>();

                //主视图
                services.AddSingleton<DashboardView>();
                services.AddTransient<DashboardViewModel>();

                //轨迹视图
                services.AddSingleton<TrackView>();

                //导入视图
                services.AddSingleton<ImportView>();

                //活动视图
                services.AddSingleton<ActivityView>();
                services.AddTransient<ActivityViewModel>();

                //设置视图
                services.AddSingleton<SettingView>();
                services.AddTransient<SettingViewModel>();
            })
            .ConfigureHostConfiguration(x =>
            {
                x.SetBasePath(AppContext.BaseDirectory);
                x.AddJsonFile("appsetting.json", optional: true);
            });

        return builder;
    }


    public static App GetApp(this IServiceProvider service) => service.GetRequiredService<App>();

    public static MainShell GetMainShell(this IServiceProvider service) => service.GetRequiredService<MainShell>();
    public static MainShellViewModel GetMainWindowViewModel(this IServiceProvider service) => service.GetRequiredService<MainShellViewModel>();
}
