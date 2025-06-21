using Common.Model.Hosted;
using FluentFrame.Service.Shell.Menu;
using FluentFrame.Service.Shell.Navigation;
using FluentFrame.UI.Shell;
using FluentFrame.UI.View;
using Microsoft.Extensions.Logging;
using Strack.Desktop.Service.Setting;
using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Setting;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Strack.Desktop.Service.Hosted;

internal class HostedService(
    ILogger<HostedService> logger,
    App app, 
    FluentShell fluentShell,
    IStrackDesktopSetting setting,
    IMenuService menuService,
    IThemeService themeService,
    IPageNavigationService pageNavigationService
    ) : OneTimeHostedService
{
    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
       {
           themeService.SetTheme(setting.IsDarkTheme ? ApplicationTheme.Dark : ApplicationTheme.Light);
           logger.LogTrace("已设置主题");

#if DEBUG
           menuService.Add<DemoView>(SymbolRegular.EmojiSparkle24, "演示");
#endif
           menuService.Add<ActivityPage>(SymbolRegular.Cloud24, "活动");
           menuService.AddFooter<SettingPage>(SymbolRegular.Settings24, "设置");

           pageNavigationService.NavigationToAwareAsync(typeof(ActivityPage));

           app.MainWindow = fluentShell;
           app.MainWindow.Show();
           logger.LogInformation("主窗口已显示");
       });




        //mainShellService.AddMenu<ActivityView>(SymbolRegular.DataLine20, "活动");
        //mainShellService.AddMenu<ImportView>(SymbolRegular.ArrowCircleUp20, "账户");


        //mainShellService.AddMenu<MainShell>(SymbolRegular.Bug24, "测试");

        //menus.Add(new() { Content = "主页", Icon = new SymbolIcon(SymbolRegular.GlanceHorizontal20), TargetPageType = typeof(DashboardView) });



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
