using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Strack.Desktop.Extension;
using Strack.Desktop.Model.Shell.Navigation;
using Strack.Desktop.Service.Shell;
using Strack.Desktop.ViewModel.Shell.Navigation;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Navigation;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace Strack.Desktop.ViewModel.Shell;


public partial class MainShellViewModel(
    IServiceProvider services,
    ILogger<MainShellViewModel> logger,
    ISnackbarService snackbarService,
    INavigationService navigationService
    ) : ObservableObject
{
    /// <summary>
    /// 标题
    /// </summary>
    [ObservableProperty]
    public partial string Title { get; set; } = $"Strack-{typeof(MainShellViewModel).Assembly.GetName().Version}";
    /// <summary>
    /// 内容
    /// </summary>
    [ObservableProperty]
    public partial object? Content { get; set; }


    /// <summary>
    /// 导航元素
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationViewItem> NavigationItems { get; set; } = [];

    /// <summary>
    /// 页脚导航
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationViewItem> FooterNavigationItems { get; set; } = [];




    /// <summary>
    /// 导航
    /// </summary>
    [ObservableProperty]
    public partial NavigationViewModel Navigation { get; set; } = new();



    /// <summary>
    /// 导航路径
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> NavigationPaths { get; set; } = [];

    /// <summary>
    /// 导航元素
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> NavigationViewModels { get; set; } = [];

    /// <summary>
    /// 页脚导航
    /// </summary>
    [ObservableProperty]
    public partial ObservableCollection<NavigationItemViewModel> FooterNavigationViewModels { get; set; } = [];




    public void AddNavigate(NavigationItemViewModel item)
    {
        NavigationViewModels.Add(item);
    }
    public bool RemoveNavigate(NavigationItemViewModel item)
    {
        return NavigationViewModels.Remove(item);
    }

    public void AddFooterNavigate(NavigationItemViewModel item)
    {
        FooterNavigationViewModels.Add(item);
    }
    public bool RemoveFooterNavigate(NavigationItemViewModel item)
    {
        return FooterNavigationViewModels.Remove(item);
    }


    public async Task<bool> NavigateToAsync(Type targetPageType, NavigationCallback? callback = null)
    {
        

        NavigationItemViewModel[] topNavItems = [.. NavigationViewModels, .. FooterNavigationViewModels];
        var path = topNavItems.Select(x =>
        {
            List<NavigationItemViewModel> path = [];
            FindItemPath(ref path, x, targetPageType);
            return path;

        }).Where(x => x.Count != 0).FirstOrDefault();

        if(path == null || path.Count == 0)
        {
            snackbarService.ShowError("目标页面不存在于导航");
            return false;
        }

        foreach (var i in NavigationPaths) i.ISelected = false;
        NavigationPaths = [.. path];
        foreach (var i in NavigationPaths) i.ISelected = true;

        var navItem = NavigationPaths[^1];
        var pageType = navItem.TargetPageType;
        Content = services.GetService(pageType);

        _navigationHistory.Push(navItem);
        return true;


        static bool FindItemPath(ref List<NavigationItemViewModel> path, NavigationItemViewModel root, Type targetPageType)
        {
            path.Add(root);

            if (root.TargetPageType == targetPageType)
                return true;

            //foreach (var child in root.ChildPageTypes)
            //{
            //    if (FindItemPath(ref path, child, targetPageType)) return true;
            //}

            path.RemoveAt(path.Count - 1); // 回溯
            return false;
        }
    }
    public async Task<bool> NavigateBackAsync(NavigationCallback? callback = null)
    {
        if(NavigationItems.Count < 2) return false;

        var from = NavigationViewModels[^1];
        var to = NavigationViewModels[^2];

        //回调
        if (callback != null && await callback.Invoke(to, from)) return false;

        if(_navigationHistory.TryPop(out var result))
        {
            await NavigateToAsync(result.TargetPageType, callback);
        }

        return true;
    }




    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task<bool> OnNavigatedAwareAsync(object to, object? from)
    {
        try
        {
            //处理视图模型导航通知
            if (to is FrameworkElement toPage && toPage.DataContext is INavigationAware toNavigationAware) await toNavigationAware.OnNavigatedToAsync();
            if (from is FrameworkElement fromPage && fromPage.DataContext is INavigationAware fromNavigationAware) await fromNavigationAware.OnNavigatedFromAsync();

            return true;
        }
        catch (Exception ex)
        {
            snackbarService.ShowError(ex.Message, "跳转失败");
            return false;
        }
    }

    /// <summary>
    /// 导航记录
    /// </summary>
    public readonly Stack<NavigationItemViewModel> _navigationHistory = [];
}