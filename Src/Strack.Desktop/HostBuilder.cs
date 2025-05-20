using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Strack.Desktop.Service;
using Strack.Desktop.UI.Shell;
using Strack.Desktop.UI.View;
using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.UI.View.Track;
using Strack.Desktop.ViewModel.Shell;
using Strack.Desktop.ViewModel.Shell.Navigation.Item;
using Strack.Desktop.ViewModel.View;
using Strack.Desktop.ViewModel.View.Dashboard;

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
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                //导航元素
                services.AddTransient<NavigationItemViewModel>();


                //轨迹视图
                services.AddSingleton<DashboardView>();
                services.AddTransient<DashboardViewModel>();

                services.AddSingleton<TrackView>();
                services.AddSingleton<ImportView>();

                services.AddSingleton<TestView>();
                services.AddTransient<TestViewModel>();
            });

        return builder;
    }

    public static ILogger<T> GetLogger<T>(this IServiceProvider service) => service.GetRequiredService<ILogger<T>>();

    public static App GetApp(this IServiceProvider service) => service.GetRequiredService<App>();

    public static MainWindow GetMainWindow(this IServiceProvider service) => service.GetRequiredService<MainWindow>();
    public static MainWindowViewModel GetMainWindowViewModel(this IServiceProvider service) => service.GetRequiredService<MainWindowViewModel>();


    public static NavigationItemViewModel GetNavigationItemViewModel(this IServiceProvider service) => service.GetRequiredService<NavigationItemViewModel>();




    public static TestView GetTestView(this IServiceProvider service) => service.GetRequiredService<TestView>();





}
