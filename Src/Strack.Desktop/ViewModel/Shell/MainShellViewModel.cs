using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using Wpf.Ui.Controls;

namespace Strack.Desktop.ViewModel.Shell;


public partial class MainShellViewModel : ObservableObject
{
    /// <summary>
    /// 主窗口标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; } = $"Strack-{typeof(MainShellViewModel).Assembly.GetName().Version}";

    /// <summary>
    /// 菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationViewItem> MenuItemsSource { get; set; } = [];

    /// <summary>
    /// 页脚菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationViewItem> FooterMenuItemsSource { get; set; } = [];
}

/// <summary>
/// 主窗口Vm扩展
/// </summary>
public static class MainShellViewModelExtension
{
    /// <summary>
    /// 添加一个菜单
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    public static void Add<T>(this ICollection<NavigationViewItem> vms, IconElement icon, string title) where T : FrameworkElement
    {
        NavigationViewItem viewModel = new()
        {
            Icon = icon,
            Content = title,
            TargetPageType = typeof(T)
        };

        vms.Add(viewModel);
    }
}