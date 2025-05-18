using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Strack.Desktop.ViewModel.Shell.Navigation.Item;
using Strack.Desktop.ViewModel.Shell.Navigation.Page;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Strack.Desktop.ViewModel.Shell;


public partial class MainWindowViewModel(
    IServiceProvider services,
    ILogger<MainWindowViewModel> logger,
    App app
    ) : ObservableObject
{
    /// <summary>
    /// 主窗口标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; } = $"Strack-{typeof(MainWindowViewModel).Assembly.GetName().Version}";

    /// <summary>
    /// 当前内容
    /// </summary>
    [ObservableProperty]
    public partial FrameworkElement? ViewContent { get; set; }


    /// <summary>
    /// 菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> Menus { get; set; } = [];

    /// <summary>
    /// 页脚菜单
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> FooterMenus { get; set; } = [];


    /// <summary>
    /// 导航到目标类型页面
    /// </summary>
    /// <param name="targetPageType"></param>
    /// <returns></returns>
    [RelayCommand]
    public async Task NavigateAsync(Type targetPageType)
    {
        var menuItem = Menus.FirstOrDefault(x => x.TargetPageType == targetPageType);
        if (menuItem == null) menuItem = FooterMenus.FirstOrDefault(x => x.TargetPageType == targetPageType);

        //菜单不存在
        if (menuItem != null)
        {
            menuItem.IsSelected = true;
        }

        //无效页面类型
        if (!typeof(FrameworkElement).IsAssignableFrom(targetPageType))
        {
            logger.LogWarning("导航失败, 不支持的页面类型:{type}", targetPageType);
            return;
        }

        //无效页面类型
        if (!typeof(FrameworkElement).IsAssignableFrom(targetPageType))
        {
            logger.LogWarning("导航失败, 不支持的页面类型:{type}", targetPageType);
            return;
        }

        //页面
        if (services.GetService(targetPageType) is not FrameworkElement page)
        {
            logger.LogInformation("导航目标为空, 已取消跳转");
            return;
        }

        //设置视图
        ViewContent = page;

        //页面Vm -不支持进入离开
        if (page?.DataContext is not INavigationPageViewModel pageVm)
        {
            return;
        }

        //离开上一个页面
        if (_currentNavigationPageViewModel is not null)
        {
            await _currentNavigationPageViewModel.NavigationFromAsync();
            logger.LogInformation("导航离开页面:{type}", targetPageType);
        }

        //切换页面
        _currentNavigationPageViewModel = pageVm;

        //进入当前页面
        if (_currentNavigationPageViewModel is not null)
        {
            await _currentNavigationPageViewModel.NavigationToAsync();
            logger.LogInformation("导航进入页面:{type}", targetPageType);
        }
    }

    /// <summary>
    /// 导航到目标页面
    /// </summary>
    /// <typeparam name="TPage"></typeparam>
    /// <returns></returns>
    public async Task NavigateAsync<TPage>() where TPage : FrameworkElement
    {
        await NavigateAsync(typeof(TPage));
    }


    //当前导航页面
    private INavigationPageViewModel? _currentNavigationPageViewModel;
}



public static class MainWindowViewModelExtension
{
    extension(ICollection<NavigationItemViewModel> vms)
    {
        /// <summary>
        /// 添加一个菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="icon"></param>
        /// <param name="title"></param>
        public void Add<T>(Geometry icon, string title) where T : FrameworkElement
        {
            NavigationItemViewModel viewModel = new()
            {
                Icon = icon,
                Title = title,
                TargetPageType = typeof(T)
            };

            vms.Add(viewModel);
        }
    }
}