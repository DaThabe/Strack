using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Strack.Desktop.Extension;
using Strack.Desktop.Model.Shell.Navigation;
using Strack.Desktop.Service.Shell;
using Strack.Desktop.ViewModel.Shell.Menu;
using Strack.Desktop.ViewModel.Shell.Navigation;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;

namespace Strack.Desktop.ViewModel.Shell;


public partial class MainShellViewModel(
    IServiceProvider services,
    ILogger<MainShellViewModel> logger,
    ISnackbarService snackbarService
    ) : ObservableObject, IMainShellService
{
    /// <summary>
    /// 主窗口标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; } = $"Strack-{typeof(MainShellViewModel).Assembly.GetName().Version}";


    /// <summary>
    /// 当前导航元素
    /// </summary>
    [ObservableProperty]
    public partial NavigationItemViewModel? CurrentNavigationItem { get; set; }
    /// <summary>
    /// 导航记录
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> NavigationItems { get; set; } = [];


    /// <summary>
    /// 当前菜单元素
    /// </summary>
    [ObservableProperty]
    public partial MenuItemViewModel? SelectedMenuItem { get; set; }
    /// <summary>
    /// 菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MenuItemViewModel> MenuItems { get; set; } = [];
    /// <summary>
    /// 页脚菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<MenuItemViewModel> FooterMenuItems { get; set; } = [];


    /// <summary>
    /// 选择菜单元素
    /// </summary>
    /// <param name="value"></param>
    [RelayCommand]
    private void OnSelectMenuItem(MenuItemViewModel? value)
    {
        SelectedMenuItem = value;
    }



    /// <summary>
    /// 业务容器
    /// </summary>
    public IServiceProvider ServiceProvider => services;

    #region --导航--

    IEnumerable<NavigationItemViewModel> IMainShellService.NavigationItems => NavigationItems;

    public async Task<bool> NavigateBackAsync(NavigationCallback? callback = null)
    {
        if(NavigationItems.Count < 2) return false;

        var from = NavigationItems[^1];
        var to = NavigationItems[^2];

        //回调
        if (callback != null && await callback.Invoke(to, from)) return false;

        //删除
        NavigationItems.Remove(from);
        await OnNavigatedAwareAsync(to, from);

        CurrentNavigationItem = to;
        return true;
    }
    public async Task<bool> NavigateToAsync(NavigationItemViewModel navigationItem, NavigationCallback? callback = null)
    {
        var from = NavigationItems.Count > 0 ? NavigationItems[^1] : null;

        var index = NavigationItems.IndexOf(navigationItem);
        if (index != -1)
        {
            var to = NavigationItems[index];

            //回调
            if (callback != null && await callback.Invoke(to, from)) return false;

            //删除从这里到后面的
            for (int cur = index + 1; cur < NavigationItems.Count; cur++) NavigationItems.RemoveAt(cur);

            await OnNavigatedAwareAsync(to, from);
            CurrentNavigationItem = to;
            return true;
        }
        else
        {
            var to = navigationItem;

            //回调
            if (callback != null && await callback.Invoke(to, from)) return false;

            NavigationItems.Add(navigationItem);
            CurrentNavigationItem = to;
            return true;
        }
    }

   

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task<bool> OnNavigatedAwareAsync(NavigationItemViewModel to, NavigationItemViewModel? from)
    {
        try
        {
            //处理视图模型导航通知
            if (to.Content is FrameworkElement toPage && toPage.DataContext is INavigationAware toNavigationAware) await toNavigationAware.OnNavigatedToAsync();
            if (from?.Content is FrameworkElement fromPage && fromPage.DataContext is INavigationAware fromNavigationAware) await fromNavigationAware.OnNavigatedFromAsync();

            return true;
        }
        catch(Exception ex)
        {
            snackbarService.ShowError(ex.Message, "跳转失败");
            return false;
        }
    }

   

    #endregion

    #region --菜单--

    IEnumerable<MenuItemViewModel> IMainShellService.MenuItems => MenuItems;
    IEnumerable<MenuItemViewModel> IMainShellService.FooterMenuItems => FooterMenuItems;

    public void AddMenu(MenuItemViewModel menuItem)
    {
        MenuItems.Add(menuItem);
    }
    public bool RemoveMenu(MenuItemViewModel menuItem)
    {
        return MenuItems.Remove(menuItem);
    }

    public void AddFooterMenu(MenuItemViewModel menuItem)
    {
        FooterMenuItems.Add(menuItem);
    }
    public bool RemoveFooterMenu(MenuItemViewModel menuItem)
    {
        return FooterMenuItems.Remove(menuItem);
    }

    
    #endregion
}