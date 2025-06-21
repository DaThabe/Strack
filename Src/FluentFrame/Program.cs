using FluentFrame.ViewModel.Shell;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;

namespace FluentFrame;

internal static class Program
{
    [STAThread]
    public static void Main()
    {
        Application app = new();
        app.Resources.MergedDictionaries.Add(new ControlsDictionary());
        app.Resources.MergedDictionaries.Add(new ThemesDictionary() { Theme = ApplicationTheme.Dark });

        app.DispatcherUnhandledException += (sender, e) =>
        {
            Debug.WriteLine(e.Exception.ToString(), "Strack");
        };

        var appHost = Host.CreateDefaultBuilder().UseFluentFrame().Build();
        appHost.StartAsync().Wait();

        //var mainShell = appHost.Services.GetFluentShell();
        //app.MainWindow = mainShell;
        
        //if (mainShell.ViewModel is FluentShellViewModel vm)
        //{
        //    vm.Icon = new SymbolIcon(SymbolRegular.Bug24);
        //    vm.Title = "测试标题";

        //    vm.NavigationMenus.Add(new() { Content = "测试1", Icon = new SymbolIcon(SymbolRegular.Bug24), TargetPageType = typeof(SettingView) });
        //    vm.NavigationMenus.Add(new() { Content = "测试2", Icon = new SymbolIcon(SymbolRegular.Bug20), TargetPageType = typeof(SettingView)  });
        //    vm.NavigationMenus.Add(new() { Content = "测试3", Icon = new SymbolIcon(SymbolRegular.Bug16), TargetPageType = typeof(SettingView) });

        //    vm.NavigationRecords.Add(new() { Content = "记录1", TargetPageType = typeof(SettingView) });

        //    vm.Content = new Button() { Content = "测试内容" };
        //    vm.Messages.Add(new() { Severity = InfoBarSeverity.Success, Title = "测试成功" });
        //    vm.Messages.Add(new() { Severity = InfoBarSeverity.Informational, Title = "测试消息" });
        //    vm.Messages.Add(new() { Severity = InfoBarSeverity.Warning, Title = "测试警告" });
        //    vm.Messages.Add(new() { Severity = InfoBarSeverity.Error, Title = "测试错误" });
        //}

     
        app.MainWindow.Show();
        app.Run();
        appHost.StopAsync().Wait();
    }
}