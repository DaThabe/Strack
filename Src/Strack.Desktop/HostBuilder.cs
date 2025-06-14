using FluentFrame.Service.Navigation;
using FluentFrame.UI.Shell;
using FluentFrame.ViewModel.Shell;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.Desktop.Service;
using Strack.Desktop.UI.Shell.Main;
using Strack.Desktop.UI.View.Setting;
using Strack.Desktop.UI.View.Sync;
using Strack.Desktop.ViewModel.Shell;
using Strack.Desktop.ViewModel.View.Setting;
using Strack.Desktop.ViewModel.View.Sync;


namespace Strack.Desktop;

public static class HostBuilder
{
    public static IHostBuilder UseStrackDesktop(this IHostBuilder builder)
    {
        builder
            .UseStrack()
            .ConfigureServices((context, services) =>
            {
                //Hosted
                services.AddHostedService<HostedService>();

                //设置
                services.AddSingleton<IStrackDesktopSetting, StrackDesktopSetting>();

                //窗口
                services.AddSingleton<FluentShell>();
                services.AddSingleton<FluentShellViewModel>();
                //导航
                services.AddSingleton<INavigationService, NavigationService>();


                //同步页面
                services.AddTransient<SyncView>();
                services.AddTransient<SyncViewModel>();

                //设置视图
                services.AddTransient<SettingView>();
                services.AddTransient<SettingViewModel>();

                ////提示
                //services.AddSingleton<ISnackbarService, SnackbarService>();
                ////弹窗
                //services.AddSingleton<IContentDialogService, ContentDialogService>();
                ////主题
                //services.AddSingleton<IThemeService, ThemeService>();
                ////导航
                //services.AddSingleton<INavigationService, NavigationService>();
                //services.AddSingleton<INavigationViewPageProvider, NavigationViewPageProvider>();

                ////主窗口
                //services.AddSingleton<MainShell>();
                //services.AddTransient<MainShellViewModel>();

                ////主视图
                //services.AddSingleton<DashboardView>();
                //services.AddTransient<DashboardViewModel>();
                //services.AddTransient<AddUserView>();

                ////轨迹视图
                //services.AddSingleton<TrackView>();

                ////导入视图
                //services.AddSingleton<ImportView>();

                ////账户视图
                //services.AddSingleton<AccountView>();
                //services.AddTransient<AccountViewModel>();

                ////活动视图
                //services.AddSingleton<ActivityView>();
                ////services.AddTransient<ActivityViewModel>();


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
}
