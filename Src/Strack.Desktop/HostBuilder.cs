using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.Desktop.Service;
using Strack.Desktop.Service.Shell;
using Strack.Desktop.UI.Shell.Main;
using Strack.Desktop.UI.View.Account;
using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.UI.View.Dashboard.User;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.UI.View.Setting;
using Strack.Desktop.UI.View.Track;
using Strack.Desktop.ViewModel.Shell;
using Strack.Desktop.ViewModel.Shell.Menu;
using Strack.Desktop.ViewModel.Shell.Navigation;
using Strack.Desktop.ViewModel.View.Account;
using Strack.Desktop.ViewModel.View.Dashboard;
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

                //提示
                services.AddSingleton<ISnackbarService, SnackbarService>();
                //弹窗
                services.AddSingleton<IContentDialogService, ContentDialogService>();
                //主题
                services.AddSingleton<IThemeService, ThemeService>();
                //设置
                services.AddSingleton<IStrackDesktopSetting, StrackDesktopSetting>();


                //程序和窗口
                services.AddSingleton(app);
                services.AddSingleton<MainShell>();
                services.AddSingleton<MainShellViewModel>();
                services.AddSingleton<IMainShellService>(x => x.GetRequiredService<MainShellViewModel>());

                //主视图
                services.AddSingleton<DashboardView>();
                services.AddTransient<DashboardViewModel>();
                services.AddTransient<AddUserView>();

                //轨迹视图
                services.AddSingleton<TrackView>();

                //导入视图
                services.AddSingleton<ImportView>();

                //账户视图
                services.AddSingleton<AccountView>();
                services.AddTransient<AccountViewModel>();

                //活动视图
                services.AddSingleton<ActivityView>();
                //services.AddTransient<ActivityViewModel>();

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


    public static App GetApp(this IServiceProvider service) => 
        service.GetRequiredService<App>();
    public static MainShell GetMainShell(this IServiceProvider service) => 
        service.GetRequiredService<MainShell>();
    public static MainShellViewModel GetMainWindowViewModel(this IServiceProvider service) => 
        service.GetRequiredService<MainShellViewModel>();
    public static ISnackbarService GetISnackbarService(this IServiceProvider service) => 
        service.GetRequiredService<ISnackbarService>();

    /// <summary>
    /// 根据菜单导航创建导航元素
    /// </summary>
    /// <param name="service"></param>
    /// <param name="menu"></param>
    /// <returns></returns>
    public static NavigationItemViewModel GetNavigationItemViewModel(this IServiceProvider service, MenuItemViewModel menu)
    {
        var page = service.GetRequiredService(menu.TargetPageType);

        return new NavigationItemViewModel()
        {
            Icon = menu.Icon,
            Title = menu.Title,
            Content = page,
        };
    }
}
