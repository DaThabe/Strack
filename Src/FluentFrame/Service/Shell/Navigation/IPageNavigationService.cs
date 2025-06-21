using FluentFrame.Service.Shell.Menu;
using FluentFrame.Service.Shell.Notify;
using FluentFrame.ViewModel.Shell.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;
using Wpf.Ui.Abstractions.Controls;

namespace FluentFrame.Service.Shell.Navigation;


/// <summary>
/// 页面导航
/// </summary>
public interface IPageNavigationService
{
    /// <summary>
    /// 是否可以返回
    /// </summary>
    bool CanBack { get; }

    /// <summary>
    /// 是否可以前进
    /// </summary>
    bool CanForward { get; }


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


public class PageNavigationService(
    IServiceProvider service,
    ILogger<PageNavigationService> logger,
    IMenuService menuService,
    INotifyService notifyService
    ) : IPageNavigationService
{
    public bool CanBack => _record.Count > 1;

    public bool CanForward => _recycle.Count > 0;


    public void SetSourceProvider(INavigationSourceProvider sourceProvider) => _sourceProvider = sourceProvider;
    public async Task<bool> BackAsync(NavigationCallbackHandler? callback = null)
    {
        try
        {
            //尝试获取前一个页面
            if (!_record.TryPop(out var page) || page == null) return false;

            //回调阻止导航
            if (callback != null && !await callback.Invoke(page, _sourceProvider.Content)) return false;

     
            _sourceProvider.Content = page;
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
            if (callback != null && !await callback.Invoke(page, _sourceProvider.Content)) return false;

            _sourceProvider.Content = page;
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
            if(_lastPageType == targetPageType)
            {
                logger.LogWarning("导航失败, 已是目标页面");
                return false;
            }
            var pageInstance = service.GetRequiredService(targetPageType);

            if (pageInstance == null)
            {
                logger.LogError("导航失败,目标页面不存在:{targetPageType}", targetPageType);
                return false;
            }

            //回调阻止导航
            if (callback != null && !await callback.Invoke(pageInstance, _sourceProvider.Content)) return false;

            //选择菜单
            menuService.SelectByTargetPageType(targetPageType);

            //更新内容
            _sourceProvider.Content = pageInstance;
            _lastPageType = targetPageType;

            //TODO: 导航没有显示数量
            _record.Push(pageInstance);

            return true;
        }
        catch(Exception ex)
        {
            await notifyService.ShowErrorAsync(ex.Message, "导航失败");
            logger.LogError(ex, "导航到目标页面失败");
            return false;
        }
    }


    //记录
    private readonly Stack<object> _record = [];
    //回收站
    private readonly Stack<object> _recycle = [];
    //上一个页面类型
    private Type? _lastPageType;


    private INavigationSourceProvider _sourceProvider = null!;
}


public static class PageNavigationServiceAwareExtension
{
    public static Task<bool> NavigationToAwareAsync(this IPageNavigationService service, Type targetPageType, NavigationCallbackHandler? callback = null)
    {
        return service.NavigationToAsync(targetPageType, async (to, from) =>
        {
            if(to is FrameworkElement frameworkElement && frameworkElement.DataContext is INavigationAware toAware)
            {
                await toAware.OnNavigatedToAsync();
            }

            if(from is FrameworkElement frameworkElementFrom && frameworkElementFrom.DataContext is INavigationAware fromAware)
            {
                await fromAware.OnNavigatedFromAsync();
            }

            if(callback != null) return await callback.Invoke(to, from);
            return true;
        });
    }
    public static Task<bool> BackAwareAsync(this IPageNavigationService service, NavigationCallbackHandler? callback = null)
    {
        return service.BackAsync(async (to, from) =>
        {
            if (to is FrameworkElement frameworkElement && frameworkElement.DataContext is INavigationAware toAware)
            {
                await toAware.OnNavigatedToAsync();
            }

            if (from is FrameworkElement frameworkElementFrom && frameworkElementFrom.DataContext is INavigationAware fromAware)
            {
                await fromAware.OnNavigatedFromAsync();
            }

            if (callback != null) return await callback.Invoke(to, from);
            return true;
        });
    }
    public static Task<bool> ForwardAwareAsync(this IPageNavigationService service, NavigationCallbackHandler? callback = null)
    {
        return service.ForwardAsync(async (to, from) =>
        {
            if (to is FrameworkElement frameworkElement && frameworkElement.DataContext is INavigationAware toAware)
            {
                await toAware.OnNavigatedToAsync();
            }

            if (from is FrameworkElement frameworkElementFrom && frameworkElementFrom.DataContext is INavigationAware fromAware)
            {
                await fromAware.OnNavigatedFromAsync();
            }

            if (callback != null) return await callback.Invoke(to, from);
            return true;
        });
    }
}
