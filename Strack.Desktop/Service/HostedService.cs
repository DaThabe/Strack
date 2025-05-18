using Microsoft.Extensions.Logging;
using Strack.Desktop.UI.Shell;
using Strack.Service;
using System.Windows;

namespace Strack.Desktop.Service;

internal class HostedService(
    ILogger<HostedService> logger,
    App app, 
    MainWindow mainWindow) : OneTimeHostedService
{
    protected override async Task ExecuteOnceAsync(CancellationToken cancellationToken)
    {
        await Application.Current.Dispatcher.InvokeAsync( () =>
        {
            app.MainWindow = mainWindow;
            mainWindow.Show();

            logger.LogInformation("主窗口已显示, 标题:{title}", mainWindow.Title);
        });
    }
}
