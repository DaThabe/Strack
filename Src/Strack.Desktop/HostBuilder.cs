using FluentFrame.UI.View;
using FluentFrame.ViewModel.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strack.Desktop.Service.Hosted;
using Strack.Desktop.Service.Setting;
using Strack.Desktop.UI.Page.Activity.User;
using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Setting;
using Strack.Desktop.ViewModel.Page.Activity.User;
using Strack.Desktop.ViewModel.View.Activity;
using Strack.Desktop.ViewModel.View.Activity.User;
using Strack.Desktop.ViewModel.View.Setting;


namespace Strack.Desktop;

public static class HostBuilder
{
    public static IHostBuilder UseStrackDesktop(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            //Hosted
            services.AddHostedService<HostedService>();

            //设置
            services.AddSingleton<IStrackDesktopSetting, StrackDesktopSetting>();

            services.AddSingleton<DemoView>();
            services.AddSingleton<DemoViewModel>();



            //活动页面
            services.AddTransient<ActivityPage>();
            services.AddTransient<ActivityPageViewModel>();
            services.AddTransient<UserViewModel>();

            //用户添加
            services.AddTransient<UserAdderView>();
            services.AddTransient<UserAdderViewModel>();




            //设置视图
            services.AddTransient<SettingPage>();
            services.AddTransient<SettingPageViewModel>();
        });

        return builder;
    }
}
