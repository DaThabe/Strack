using Common.Model.Hosted;
using Microsoft.Extensions.Logging;
using Strack.Desktop.Extension;
using Strack.Desktop.Factory;
using Strack.Desktop.Service.Shell;
using Strack.Desktop.UI.Shell.Main;
using Strack.Desktop.UI.View;
using Strack.Desktop.UI.View.Account;
using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.UI.View.Setting;
using Strack.Desktop.UI.View.Track;
using Strack.Desktop.ViewModel.Shell;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui;
using Wpf.Ui.Controls;
using XingZhe.Service;

namespace Strack.Desktop.Service;

internal class HostedService(
    IServiceProvider services,
    ILogger<HostedService> logger,
    App app, 
    MainShell mainShell,
    IStrackDesktopSetting setting,
    IThemeService themeService,
    AccountView accountView,
    IXingZheSetting xingZheSetting,
    IXingZheClientProvider xingZheClientProvider,
    IMainShellService mainShellService
    ) : OneTimeHostedService
{
    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        await Application.Current.Dispatcher.Invoke(async () =>
        {
            themeService.SetTheme(setting.Theme);
            logger.LogTrace("已设置主题");

            app.MainWindow = mainShell;
            mainShell.Show();
            logger.LogInformation("主窗口已显示");


            //mainShellService.AddMenu<ActivityView>(SymbolRegular.DataLine20, "活动");
            //mainShellService.AddMenu<ImportView>(SymbolRegular.ArrowCircleUp20, "账户");


            //mainShellService.AddMenu<MainShell>(SymbolRegular.Bug24, "测试");

            var main = mainShellService.AddMenu<DashboardView>(SymbolRegular.GlanceHorizontal20, "主页");
            mainShellService.AddMenu<AccountView>(SymbolRegular.People24, "账户");
            mainShellService.AddFooterMenu<SettingView>(SymbolRegular.Settings24, "设置");


            var mainNavigation = services.GetNavigationItemViewModel(main);
            await mainShellService.NavigateToAsync(mainNavigation);
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
