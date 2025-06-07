using Microsoft.Extensions.Logging;
using Strack.Desktop.Extension;
using Strack.Desktop.UI.Shell;
using Strack.Desktop.UI.Shell.Main;
using Strack.Desktop.UI.View;
using Strack.Desktop.UI.View.Dashboard;
using Strack.Desktop.UI.View.Import;
using Strack.Desktop.UI.View.Track;
using Strack.Desktop.ViewModel.Shell;
using Strack.Service;
using System.Windows;
using System.Windows.Media;

namespace Strack.Desktop.Service;

internal class HostedService(
    ILogger<HostedService> logger,
    App app, 
    MainShell mainShell) : OneTimeHostedService
{
    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        logger.LogTrace("界面加载中");

        await Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            app.MainWindow = mainShell;
            mainShell.Show();

            logger.LogInformation("主窗口已显示, 标题:{title}", mainShell.Title);



            mainShell.ViewModel.Menus.Add<TestView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Detail"), "测试");

            mainShell.ViewModel.Menus.Add<DashboardView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Dashboard"), "主页");
            mainShell.ViewModel.Menus.Add<TrackView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Track"), "路径");
            mainShell.ViewModel.Menus.Add<ImportView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Track"), "导入");

            //mainWindow.ViewModel.FooterMenus.Add<TestView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Setting"), "设置");

            await mainShell.ViewModel.NavigateAsync<TestView>();
        });

        logger.LogTrace("界面加载完成");
    }
}
