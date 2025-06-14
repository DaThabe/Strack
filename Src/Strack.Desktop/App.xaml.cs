using Common;
using FluentFrame;
using IGPSport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using XingZhe;

namespace Strack.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider => _host.Services;

    public App()
    {
        //拦截未处理异常
        DispatcherUnhandledException += OnDispatcherUnhandledException;

        try
        {
            //创建Host
            _host = Host.CreateDefaultBuilder()
                .UseCommon()
                .UseXingZhe()
                .UseIGPSport()
                .UseStrack()
                .UseFluentFrame()
                .UseStrackDesktop()
                .ConfigureServices(x => x.AddSingleton(this))
                .Build();

            //日志
            _logger = _host.Services.GetLogger<App>();
        }
        catch(Exception ex)
        {
            Directory.CreateDirectory("Logs");
            var fileName = Path.Combine("Logs", $"StartupFailed_{DateTime.Now:yyyyMMdd_HHmmss}.log");
            File.WriteAllText(fileName, ex.ToString());

            throw;
        }
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            base.OnStartup(e);
            await _host.StartAsync();

            _logger.LogInformation("程序已启动");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "启动程序时发生错误");
            MessageBox.Show(ex.ToString(), $"启动失败: {ex.Message}", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        try
        {
            await _host.StopAsync();
            base.OnExit(e);

            _logger.LogInformation("程序已关闭");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,  "关闭程序时发生错误");
        }
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        _logger.LogError(e.Exception, "未处理的异常");
        e.Handled = true;
    }


    private readonly IHost _host;
    private readonly ILogger<App> _logger;
}
