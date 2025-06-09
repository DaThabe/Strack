using Common.Model.Hosted;
using Microsoft.Extensions.Logging;
using Strack.Desktop.Extension;
using Strack.Desktop.UI.Shell.Main;
using Strack.Desktop.UI.View;
using Strack.Desktop.UI.View.Activity;
using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.UI.View.Setting;
using Strack.Desktop.UI.View.Track;
using Strack.Desktop.ViewModel.Shell;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Strack.Desktop.Service;

internal class HostedService(
    ILogger<HostedService> logger,
    App app, 
    MainShell mainShell,
    INavigationService navigationService
    ) : OneTimeHostedService
{
    protected override Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("界面加载中");

        app.MainWindow = mainShell;
        mainShell.Show();

        logger.LogInformation("主窗口已显示, 标题:{title}", mainShell.Title);


        mainShell.ViewModel.MenuItemsSource.Add<DashboardView>(new SymbolIcon(SymbolRegular.GlanceHorizontal20), "主页");
        mainShell.ViewModel.MenuItemsSource.Add<ActivityView>(new SymbolIcon(SymbolRegular.DataLine20), "活动");
        //mainShell.ViewModel.MenuItemsSource.Add<ImportView>(new SymbolIcon(SymbolRegular.ArrowCircleUp20), "导入");
        mainShell.ViewModel.FooterMenuItemsSource.Add<SettingView>(new SymbolIcon(SymbolRegular.Settings20), "设置");

        navigationService.Navigate(typeof(SettingView));

        logger.LogTrace("界面加载完成");

        return Task.CompletedTask;
    }
}
