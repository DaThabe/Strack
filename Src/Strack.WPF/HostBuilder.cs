using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Strack.Desktop.Service;
using Strack.Desktop.UI.Shell.Main;
using Strack.Desktop.UI.View;
using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.UI.View.Track;
using Strack.Desktop.ViewModel.Shell;
using Strack.Desktop.ViewModel.Shell.Navigation.Item;
using Strack.Desktop.ViewModel.View;
using Strack.Desktop.ViewModel.View.Dashboard;
using Strack.Desktop.ViewModel.View.Dashboard.Activity;

namespace Strack.Desktop;

public static class HostBuilder
{
    public static IHostBuilder UseStrackDesktop(this IHostBuilder builder, App app)
    {
        builder
            .UseStrack()
            .ConfigureServices((context, services) =>
            {
                //启动后服务
                services.AddHostedService<HostedService>();

                //程序和窗口
                services.AddSingleton(app);
                services.AddSingleton<MainShell>();
                services.AddSingleton<MainShellViewModel>();
                //导航元素
                services.AddTransient<NavigationItemViewModel>();


                //轨迹视图
                services.AddSingleton<DashboardView>();
                services.AddTransient<DashboardViewModel>();

                services.AddSingleton<TrackView>();
                services.AddSingleton<ImportView>();

                services.AddSingleton<ActivityView>();
                services.AddSingleton<ActivityViewModel>();

                services.AddSingleton<TestView>();
                services.AddTransient<TestViewModel>();
            })
            .ConfigureHostConfiguration(x =>
            {
                x.SetBasePath(AppContext.BaseDirectory);
                x.AddJsonFile("config.json", optional: true);
            });

        return builder;
    }

    public static ILogger<T> GetLogger<T>(this IServiceProvider service) => service.GetRequiredService<ILogger<T>>();

    public static App GetApp(this IServiceProvider service) => service.GetRequiredService<App>();

    public static MainShell GetMainShell(this IServiceProvider service) => service.GetRequiredService<MainShell>();
    public static MainShellViewModel GetMainWindowViewModel(this IServiceProvider service) => service.GetRequiredService<MainShellViewModel>();


    public static NavigationItemViewModel GetNavigationItemViewModel(this IServiceProvider service) => service.GetRequiredService<NavigationItemViewModel>();




    public static TestView GetTestView(this IServiceProvider service) => service.GetRequiredService<TestView>();





}
