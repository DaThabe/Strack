using FluentFrame.Service.Shell.Dialog;
using FluentFrame.Service.Shell.Menu;
using FluentFrame.Service.Shell.Message;
using FluentFrame.Service.Shell.Navigation;
using FluentFrame.Service.Shell.Notify;
using FluentFrame.UI.Shell;
using FluentFrame.ViewModel.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Ui;

namespace FluentFrame;

public static class HostBuilder
{
    public static IHostBuilder UseFluentFrame(this IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            //窗口
            services.AddSingleton<FluentShell>();
            services.AddSingleton<FluentShellViewModel>();

            //导航
            services.AddSingleton<IPageNavigationService, PageNavigationService>();
            //信息
            services.AddSingleton<IMessageService, MessageService>();
            //通知
            services.AddSingleton<INotifyService, NotifyService>();
            //菜单
            services.AddSingleton<IMenuService, MenuService>();
            //弹窗
            services.AddSingleton<IDialogService, DialogService>();
            //主题
            services.AddSingleton<IThemeService, ThemeService>();
        });

        return builder;
    }
}