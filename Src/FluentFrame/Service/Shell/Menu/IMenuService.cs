using FluentFrame.Service.Shell.Navigation;
using FluentFrame.ViewModel.Shell.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace FluentFrame.Service.Shell.Menu;

/// <summary>
/// 菜单服务
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// 设置资源委托
    /// </summary>
    /// <param name="sourceProvider"></param>
    public void SetSourceProvider(IMenuSourceProvider sourceProvider);

    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <param name="item"></param>
    void Add(MenuItemViewModel item);

    /// <summary>
    /// 添加页脚菜单
    /// </summary>
    /// <param name="item"></param>
    void AddFooter(MenuItemViewModel item);

    /// <summary>
    /// 选择菜单
    /// </summary>
    /// <param name="item"></param>
    bool Select(MenuItemViewModel? item);

    /// <summary>
    /// 根据目标页面类型查找菜单
    /// </summary>
    /// <returns></returns>
    MenuItemViewModel? FindByTargetPageType(Type targetPageType);
}


public class MenuService(IServiceProvider service) : IMenuService
{
    public void SetSourceProvider(IMenuSourceProvider sourceProvider) => _sourceProvider = sourceProvider;


    public void Add(MenuItemViewModel item)
    {
        _sourceProvider.ItemsSource.Add(item);
    }
    public void AddFooter(MenuItemViewModel item)
    {
        _sourceProvider.FooterItemsSource.Add(item);
    }
    public async Task SelectAsync(MenuItemViewModel item)
    {
        if (item == _lastSelected) return;

        var pageNavigationService = service.GetRequiredService<IPageNavigationService>();
        if (await pageNavigationService.NavigationToAsync(item.TargetPageType))
        {
            _lastSelected?.IsActive = false;
            _lastSelected = item;
            _lastSelected?.IsActive = true;
        }
        else
        {
            if (item.IsActive != false) item.IsActive = false;
            if (_lastSelected?.IsActive != true) _lastSelected?.IsActive = true;
        }
    }
    public bool Select(MenuItemViewModel? item)
    {
        if (item == _lastSelected) return false;

        _lastSelected?.IsActive = false;
        _lastSelected = item;
        _lastSelected?.IsActive = true;

        return true;
    }
    public MenuItemViewModel? FindByTargetPageType(Type targetPageType)
    {
        var item = _sourceProvider.ItemsSource.FirstOrDefault(x => x.TargetPageType == targetPageType);
        if (item != null) return item;

        return _sourceProvider.FooterItemsSource.FirstOrDefault(x => x.TargetPageType == targetPageType);
    }


    private IMenuSourceProvider _sourceProvider = null!;
    private MenuItemViewModel? _lastSelected;
}


public static class MenuServiceExtension
{
    public static MenuItemViewModel Add<TPage>(this IMenuService service, SymbolRegular icon, object content)
    {
        var menuItem = new MenuItemViewModel()
        {
            Content = content,
            TargetPageType = typeof(TPage),
            Icon = new SymbolIcon(icon)
        };

        service.Add(menuItem);
        return menuItem;
    }
    public static MenuItemViewModel AddFooter<TPage>(this IMenuService service, SymbolRegular icon, object content)
    {
        var menuItem = new MenuItemViewModel()
        {
            Content = content,
            TargetPageType = typeof(TPage),
            Icon = new SymbolIcon(icon)
        };

        service.AddFooter(menuItem);
        return menuItem;
    }

    public static bool SelectByTargetPageType<TPage>(this IMenuService service)
    {
        return service.SelectByTargetPageType(typeof(TPage));
    }
    public static bool SelectByTargetPageType(this IMenuService service, Type menuTargetPageType)
    {
        var pageType = service.FindByTargetPageType(menuTargetPageType);
        return service.Select(pageType);
    }
}
