using Microsoft.Extensions.Logging;
using Strack.Desktop.Extension;
using Strack.Desktop.UI.Shell;
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
    MainWindow mainWindow) : OneTimeHostedService
{
    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        await Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            app.MainWindow = mainWindow;
            mainWindow.Show();

            logger.LogInformation("主窗口已显示, 标题:{title}", mainWindow.Title);



            mainWindow.ViewModel.Menus.Add<DashboardView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Dashboard"), "主页");
            mainWindow.ViewModel.Menus.Add<TrackView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Track"), "路径");
            mainWindow.ViewModel.Menus.Add<ImportView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Track"), "导入");
            //mainWindow.ViewModel.Menus.Add<TestView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Detail"), "记录");

            //mainWindow.ViewModel.FooterMenus.Add<TestView>(app.Resources.Find<Geometry>("MainWindow.Navigation.Icon.Setting"), "设置");

            await mainWindow.ViewModel.NavigateAsync<DashboardView>();
        });
    }
}
