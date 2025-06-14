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
public interface IMainShellService
{
    /// <summary>
    /// 导航路径
    /// </summary>
    IEnumerable<NavigationItemViewModel> NavigationPaths { get; }
    /// <summary>
    /// 导航元素
    /// </summary>
    IEnumerable<NavigationItemViewModel> NavigationItems { get; }
    /// <summary>
    /// 页脚导航元素
    /// </summary>
    IEnumerable<NavigationItemViewModel> FooterNavigationItems { get; }


    /// <summary>
    /// 跳转
    /// </summary>
    /// <param name="navigationItem"></param>
    /// <param name="dataContext"></param>
    /// <returns></returns>
    public Task<bool> NavigateToAsync(Type targetPageType, NavigationCallback? callback = null);

    /// <summary>
    /// 返回到上一个
    /// </summary>
    /// <returns></returns>
    public Task<bool> NavigateBackAsync(NavigationCallback? callback = null);


    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public void AddNavigate(NavigationItemViewModel item);
    /// <summary>
    /// 添加页脚菜单
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public void AddFooterNavigate(NavigationItemViewModel item);


    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool RemoveNavigate(NavigationItemViewModel item);
    /// <summary>
    /// 删除页脚菜单
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool RemoveFooterNavigate(NavigationItemViewModel item);
}


/// <summary>
/// 主窗口Vm扩展
/// </summary>
public static class MainShellViewModelExtension
{
    /// <summary>
    /// 添加导航
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static NavigationItemViewModel Add<T>(this ICollection<NavigationItemViewModel> source, SymbolRegular icon, string title)
    {
        NavigationItemViewModel viewModel = new()
        {
            Icon = new SymbolIcon(icon),
            Title = title,
            TargetPageType = typeof(T)
        };

        source.Add(viewModel);
        return viewModel;
    }


    /// <summary>
    /// 添加导航
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static NavigationItemViewModel AddNavigate<T>(this IMainShellService service, IconElement icon, string title)
    {
        NavigationItemViewModel viewModel = new()
        {
            Icon = icon,
            Title = title,
            TargetPageType = typeof(T)
        };

        service.AddNavigate(viewModel);
        return viewModel;
    }
    /// <summary>
    /// 添加导航
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static NavigationItemViewModel AddNavigate<T>(this IMainShellService service, SymbolRegular icon, string title)
    {
        NavigationItemViewModel viewModel = new()
        {
            Icon = new SymbolIcon(icon),
            Title = title,
            TargetPageType = typeof(T)
        };

        service.AddNavigate(viewModel);
        return viewModel;
    }

    /// <summary>
    /// 添加页脚导航
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static NavigationItemViewModel AddFooterNavigate<T>(this IMainShellService service, IconElement icon, string title)
    {
        NavigationItemViewModel viewModel = new()
        {
            Icon = icon,
            Title = title,
            TargetPageType = typeof(T)
        };

        service.AddFooterNavigate(viewModel);
        return viewModel;
    }
    /// <summary>
    /// 添加页脚导航
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="icon"></param>
    /// <param name="title"></param>
    /// <returns></returns>
    public static NavigationItemViewModel AddFooterNavigate<T>(this IMainShellService service, SymbolRegular icon, string title)
    {
        NavigationItemViewModel viewModel = new()
        {
            Icon = new SymbolIcon(icon),
            Title = title,
            TargetPageType = typeof(T)
        };

        service.AddFooterNavigate(viewModel);
        return viewModel;
    }
}