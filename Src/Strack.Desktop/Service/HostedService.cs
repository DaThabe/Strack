using Common.Model.Hosted;
using FluentFrame.Service.Navigation;
using FluentFrame.Service.Shell;
using FluentFrame.UI.Shell;
using Microsoft.Extensions.Logging;
using Strack.Desktop.UI.View.Account;
using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.UI.View.Setting;
using Strack.Desktop.UI.View.Sync;
using System.Windows;
using Wpf.Ui.Controls;

namespace Strack.Desktop.Service;

internal class HostedService(
    ILogger<HostedService> logger,
    App app, 
    IStrackDesktopSetting setting,
    IFluentShellService fluentShellService,
    INavigationService navigationService,
    FluentShell fluentShell
    ) : OneTimeHostedService
{
    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            fluentShellService.SetSourceProvider(fluentShell.ViewModel);
            navigationService.SetSourceProvider(fluentShell.ViewModel);

            fluentShellService.IsDarkTheme = setting.IsDarkTheme;
            logger.LogTrace("已设置主题");


            var menus = fluentShell.ViewModel.NavigationMenus;
            var footerMenus = fluentShell.ViewModel.NavigationFooterMenus;



            //mainShellService.AddMenu<ActivityView>(SymbolRegular.DataLine20, "活动");
            //mainShellService.AddMenu<ImportView>(SymbolRegular.ArrowCircleUp20, "账户");


            //mainShellService.AddMenu<MainShell>(SymbolRegular.Bug24, "测试");

            //menus.Add(new() { Content = "主页", Icon = new SymbolIcon(SymbolRegular.GlanceHorizontal20), TargetPageType = typeof(DashboardView) });
            menus.Add(new() { Content = "同步", Icon = new SymbolIcon(SymbolRegular.Cloud24), TargetPageType = typeof(SyncView) });
            footerMenus.Add(new() { Content = "设置", Icon = new SymbolIcon(SymbolRegular.Settings24), TargetPageType = typeof(SettingView) });
            

            app.MainWindow = fluentShell;
            app.MainWindow.Show();
            logger.LogInformation("主窗口已显示");
        });

        


        

        //foreach (var i in xingZheSetting.SessionIds)
        //{
        //    var client = xingZheClientProvider.GetOrCreateFromSessionId(i);
        //    var item = await services.CreateAccountCardViewModelFromXingZheAsync(x => accountView.ViewModel.ItemsSource.Remove(x), client.GetUserInfoAsync);
        //    if (!item.IsVerified) continue;

        //    accountView.ViewModel.ItemsSource.Add(item);
        //}
        //logger.LogTrace("行者账户已添加");
    }
}
