using Common.Model;
using Microsoft.Extensions.DependencyInjection;
using Strack.Desktop.Model.Shell.Navigation;
using Strack.Desktop.ViewModel.Shell.Menu;
using Strack.Desktop.ViewModel.Shell.Navigation;
using Wpf.Ui.Controls;

namespace Strack.Desktop.Service.Shell;


/// <summary>
/// 主窗口业务
/// </summary>
public interface IMainShellService : IInfrastructure
{
    /// <summary>
    /// 当前导航元素
    /// </summary>
    NavigationItemViewModel? CurrentNavigationItem { get; }
    /// <summary>
    /// 导航前进记录
    /// </summary>
    IEnumerable<NavigationItemViewModel> NavigationItems { get; }

    /// <summary>
    /// 跳转
    /// </summary>
    /// <param name="navigationItem"></param>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    public Task<bool> NavigateToAsync(NavigationItemViewModel navigationItem, NavigationCallback? dataContext = null);

    /// <summary>
    /// 返回到上一个
    /// </summary>
    /// <returns></returns>
    public Task<bool> NavigateBackAsync(NavigationCallback? callback = null);



    /// <summary>
    /// 选中的菜单
    /// </summary>
    MenuItemViewModel? SelectedMenuItem { get; }
    /// <summary>
    /// 菜单
    /// </summary>
    IEnumerable<MenuItemViewModel> MenuItems { get; }
    /// <summary>
    /// 页脚菜单
    /// </summary>
    IEnumerable<MenuItemViewModel> FooterMenuItems { get; }

    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    public void AddMenu(MenuItemViewModel menuItem);
    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    public bool RemoveMenu(MenuItemViewModel menuItem);

    /// <summary>
    /// 添加页脚菜单
    /// </summary>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    public void AddFooterMenu(MenuItemViewModel menuItem);
    /// <summary>
    /// 删除页脚菜单
    /// </summary>
    /// <param name="menuItem"></param>
    /// <returns></returns>
    public bool RemoveFooterMenu(MenuItemViewModel menuItem);
}


/// <summary>
/// 主窗口Vm扩展
/// </summary>
public static class MainShellViewModelExtension
{
    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static MenuItemViewModel AddMenu<T>(this IMainShellService service, IconElement icon, string title)
    {
        MenuItemViewModel viewModel = new()
        {
            Title = title,
            Icon = icon,
            TargetPageType = typeof(T)
        };

        service.AddMenu(viewModel);
        return viewModel;
    }
    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static MenuItemViewModel AddMenu<T>(this IMainShellService service, SymbolRegular icon, string title)
    {
        MenuItemViewModel viewModel = new()
        {
            Icon = new SymbolIcon(icon),
            Title = title,
            TargetPageType = typeof(T)
        };

        service.AddMenu(viewModel);
        return viewModel;
    }


    /// <summary>
    /// 添加页脚菜单
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static MenuItemViewModel AddFooterMenu<T>(this IMainShellService service, IconElement icon, string title)
    {
        MenuItemViewModel viewModel = new()
        {
            Icon = icon,
            Title = title,
            TargetPageType = typeof(T)
        };

        service.AddFooterMenu(viewModel);
        return viewModel;
    }
    /// <summary>
    /// 添加页脚菜单那
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static MenuItemViewModel AddFooterMenu<T>(this IMainShellService service, SymbolRegular icon, string title)
    {
        MenuItemViewModel viewModel = new()
        {
            Icon = new SymbolIcon(icon),
            Title = title,
            TargetPageType = typeof(T)
        };

        service.AddFooterMenu(viewModel);
        return viewModel;
    }
}