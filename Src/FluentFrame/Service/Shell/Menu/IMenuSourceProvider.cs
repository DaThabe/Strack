using FluentFrame.ViewModel.Shell.Navigation;
using System.Collections.ObjectModel;

namespace FluentFrame.Service.Shell.Menu;


/// <summary>
/// 菜单资源
/// </summary>
public interface IMenuSourceProvider
{
    /// <summary>
    /// 菜单
    /// </summary>
    ObservableCollection<MenuItemViewModel> ItemsSource { get; }

    /// <summary>
    /// 页脚菜单
    /// </summary>
    ObservableCollection<MenuItemViewModel> FooterItemsSource { get; }
}