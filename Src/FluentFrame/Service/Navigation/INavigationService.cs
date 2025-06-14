using FluentFrame.ViewModel.Shell.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace FluentFrame.Service.Navigation;


/// <summary>
/// 导航回调
/// </summary>
/// <param name="to">目标</param>
/// <param name="from">来源</param>
public delegate Task<bool> NavigationCallbackHandler(object to, object? from);


public interface INavigationService
{
    /// <summary>
    /// 设置资源
    /// </summary>
    /// <param name="sourceProvider"></param>
    void SetSourceProvider(INavigationSourceProvider sourceProvider);

    /// <summary>
    /// 导航到
    /// </summary>
    /// <param name="targetPageType"></param>
    /// <returns></returns>
    Task<bool> NavigationToAsync(Type targetPageType, NavigationCallbackHandler? callback = null);

    /// <summary>
    /// 返回
    /// </summary>
    /// <returns></returns>
    Task<bool> BackAsync(NavigationCallbackHandler? callback = null);

    /// <summary>
    /// 前进
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    Task<bool> ForwardAsync(NavigationCallbackHandler? callback = null);
}


public class NavigationService(
    IServiceProvider service,
    ILogger<NavigationService> logger
    ) : INavigationService
{
    public void SetSourceProvider(INavigationSourceProvider sourceProvider) => _source = sourceProvider;

    public async Task<bool> BackAsync(NavigationCallbackHandler? callback = null)
    {
        try
        {
            //尝试获取前一个页面
            if (_record.TryPop(out var page) || page == null) return false;

            //回调阻止导航
            if (callback != null && !await callback.Invoke(page, _source.Content)) return false;

            _source.Content = page;
            //TODO: 导航没有显示数量
            _recycle.Push(page);

            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> ForwardAsync(NavigationCallbackHandler? callback = null)
    {

        try
        {
            //尝试获取前一个页面
            if (_recycle.TryPop(out var page) || page == null) return false;

            //回调阻止导航
            if (callback != null && !await callback.Invoke(page, _source.Content)) return false;

            _source.Content = page;
            //TODO: 导航没有显示数量
            _record.Push(page);

            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> NavigationToAsync(Type targetPageType, NavigationCallbackHandler? callback = null)
    {
        try
        {
            var pageInstance = service.GetRequiredService(targetPageType);

            //回调阻止导航
            if (callback != null && !await callback.Invoke(pageInstance, _source.Content)) return false;

            //如果是菜单则直接选中
            var menu = _source.GetTargetPageMenuItemViewModel(targetPageType);
            if (menu != null)
            {
                menu.IsActive = true;
                _lastSelectedMenuItem?.IsActive = false;
                _lastSelectedMenuItem = menu;
            }

            _source.Content = pageInstance;

            //TODO: 导航没有显示数量
            _record.Push(pageInstance);

            return true;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "导航到目标页面失败");
            return false;
        }
    }



    //记录
    private readonly Stack<object> _record = [];
    //回收站
    private readonly Stack<object> _recycle = [];

    private INavigationSourceProvider _source = null!;

    //上一个选择的菜单
    private MenuItemViewModel? _lastSelectedMenuItem;
}
